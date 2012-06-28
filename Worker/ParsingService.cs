using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using BondiGeek.Logging;
using MobilniPortalNovicLib.Models;
using Worker.Parsers;

namespace Worker
{
    public enum State
    {
        Processing, WaitingToNextInterval, Stoped
    }

    public class ParsingService
    {
        #region PrivateFields

        private static ParsingService service = null;
        private HashSet<Category> Categories = null;
        private HashSet<String> Titles = null;
        private HashSet<String> FailedTitles = null;

        #endregion PrivateFields

        #region Constructor

        private ParsingService()
        {
            TotalCount = 0;
            LastRun = 0;
            Mapper.CreateMap<NewsFileExt, NewsFile>();
            State = State.WaitingToNextInterval;
            watch = new Stopwatch();
            FeedParser = new RssFeedParser();
            NewsParser = new GenericNewsParser("article");
            FailedTitles = new HashSet<string>();
        }

        #endregion Constructor

        #region PublicFields

        public IFeedParser FeedParser { get; set; }

        public int LastRun { get; set; }

        public INewsParser NewsParser { get; set; }

        public State State { get; private set; }

        public int TotalCount { get; private set; }

        public Stopwatch watch { get; set; }

        #endregion PublicFields

        #region Methods

        public static ParsingService getParsingService()
        {
            if (service == null) service = new ParsingService();
            return service;
        }

        public void startParse()
        {
            watch.Reset();
            watch.Start();
            if (State == State.WaitingToNextInterval)
            {
                Console.WriteLine("Starting run.");
                State = State.Processing;

                LastRun = UpdateFeedsForSites();
                TotalCount += LastRun;

                State = State.WaitingToNextInterval;
                watch.Stop();
                Console.WriteLine("Run finished in {0}", watch.Elapsed);
            }
            else
            {
                throw new Exception("Parser already working");
            }
        }

        private Boolean IsElementok(NewsFileExt x)
        {
            if (x.Categories != null && x.Categories.Count() > 0 && x.Content != "Error while fetching" && x.ParseOk && x.Content != "")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Take last n titles from each feed.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private IEnumerable<String> LastTitlesByFeed(IQueryable<NewsFile> query, int n)
        {
            var list = query.GroupBy(x => x.FeedId).Select(x => x.OrderByDescending(y => y.PubDate).Take(n).Select(b => b.Title)).SelectMany(x => x);
            return list;
        }

        public int UpdateFeedsForSites()
        {
            var count = 0;
            using (var repo = new MobilniPortalNovicContext12())
            {
                #region InitializeRequiredFields

                if (Titles == null)
                {
                    ///Select only last 30 from each list
                    var list = repo.NewsFiles.Select(x => x.Title);
                    /// Select all
                    /// var list = context.NewsFiles.Select(y => y.Title));
                    Titles = new HashSet<String>(list);
                }
                if (Categories == null)
                {
                    var cat = repo.Categories.ToList();
                    cat.ForEach(x =>
                    {
                        if (x.ParentCategoryId.HasValue == false)
                        {
                            x.ParentCategoryId = null;
                        }
                    });
                    Categories = new HashSet<Category>(cat);
                }

                #endregion InitializeRequiredFields

                #region ParseFeed

                List<NewsFileExt> newsList = new List<NewsFileExt>();
                var time = DateTime.Now;

                //Get items from feed
                var feeds = repo.Feeds.ToList();
                feeds.ForEach(x => x.LastUpdated = time);
                var newsFiles = feeds.AsParallel().Select(x => FeedParser.parseFeed(x)).SelectMany(x => x).ToList();

                //remove duplicates
                newsFiles = newsFiles.GroupBy(x => x.Title).Select(x => x.First()).ToList();

                #endregion ParseFeed

                #region ParseItems

                //Remove already parsed ones
                var newsFilesList = newsFiles.Where(x => FailedTitles.Contains(x.Title) == false && Titles.Contains(x.Title) == false).ToList();
                //Process items
                var items = NewsParser.parseItem(newsFilesList);

                items.Where(x => !IsElementok(x)).ToList().ForEach(x =>
                {
                    LogWriter.Instance.Log("Error parsing: " + x.Link);
                    FailedTitles.Add(x.Title);
                });

                #endregion ParseItems

                #region SaveToDatabase

                foreach (var item in items.Where(x => IsElementok(x)).ToList())
                {
                    #region GetCategoryIdOrCreateNew

                    //Save categories into database
                    int? parentId = null;
                    foreach (var c in item.Categories.Take(2))
                    {
                        //Skip if already exists or parsed
                        if (Titles.Contains(item.Title) || FailedTitles.Contains(item.Title))
                        {
                            continue;
                        }


                        var s = Categories.Where(x => x.Name.Equals(c) && x.ParentCategoryId == parentId).FirstOrDefault();
                        if (s == null)
                        {
                            var category = repo.Categories.Add(new Category { Name = c, ParentCategoryId = parentId });
                            Console.WriteLine("Adding category {0}", c);
                            LogWriter.Instance.WriteToLog("Adding category " + c);
                            repo.SaveChanges();
                            parentId = category.CategoryId;
                            Categories.Add(category);
                        }
                        else
                        {
                            parentId = s.CategoryId;
                        }
                    }
                    item.CategoryId = parentId.Value;

                    #endregion GetCategoryIdOrCreateNew


                    Titles.Add(item.Title);
                    var i = AutoMapper.Mapper.Map<NewsFileExt, NewsFile>(item);
                    repo.NewsFiles.Add(i);
                    count += 1;
                }
                repo.SaveChanges();
                Console.WriteLine("{0} new sites added.", count);

                #endregion SaveToDatabase
            }
            return count;
        }

        #endregion Methods
    }
}