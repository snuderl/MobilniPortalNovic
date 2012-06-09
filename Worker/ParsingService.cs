using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobilniPortalNovicLib.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using Worker.Parsers;
using AutoMapper;

namespace Worker
{
    public enum State
    {
        Processing, Waiting, Finished
    }

    public class ParsingService
    {
        private static ParsingService service = null;
        public State State { get; private set; }
        public Stopwatch watch { get; set; }
        public IFeedParser FeedParser { get; set; }
        public INewsParser NewsParser { get; set; }
        public int TotalCount { get; private set; }
        public int LastRun { get; set; }

        private HashSet<Category> Categories = new HashSet<Category>();
        private HashSet<String> Titles = null;



        private ParsingService()
        {
            TotalCount = 0;
            LastRun = 0;
            Mapper.CreateMap<NewsFileExt, NewsFile>();
            State = State.Waiting;
            watch = new Stopwatch();
            FeedParser = new RssFeedParser();
            NewsParser = new GenericNewsParser("article");
        }

        public static ParsingService getParsingService()
        {
            if (service == null) service = new ParsingService();
            return service;
        }

       

        public void startParse()
        {
            watch.Reset();
            watch.Start();
            if (State == State.Waiting)
            {
                Console.WriteLine("Starting run.");
                State = State.Processing;

                LastRun = UpdateFeedsForSites();
                TotalCount += LastRun;

                State = State.Waiting;
                watch.Stop();
                Categories.Clear();
                Console.WriteLine("Run finished in {0}", watch.Elapsed);

            }
            else
            {
                throw new Exception("Parser already working");
            }
        }


        public int UpdateFeedsForSites()
        {

            var count = 0;
            using (var repo = new MobilniPortalNovicContext12())
            {


                Task<int> t1 = null;
                if (Titles == null)
                {
                    t1 = Task.Factory.StartNew(() =>
                    {
                        using (var context = new MobilniPortalNovicContext12())
                        {
                            Titles = new HashSet<String>(context.NewsFiles.Select(y => y.Title));
                            return 1;
                        }
                    });
                }

                Task<int> t2 = null;
                if (Categories == null)
                {
                    t2 = Task.Factory.StartNew(() =>
                    {
                        using (var context = new MobilniPortalNovicContext12())
                        {
                            Categories = new HashSet<Category>(context.Categories);
                            return 1;
                        }
                    });
                }

                List<NewsFileExt> newsList = new List<NewsFileExt>();
                var time = DateTime.Now;
                //Get items from feed
                var feeds = repo.Feeds.Include("Category").ToList();
                feeds.ForEach(x => x.LastUpdated = time);
                var newsFiles = feeds.AsParallel().Select(x => FeedParser.parseFeed(x)).SelectMany(x => x);

                if (t1 != null)
                {
                    Task.WaitAll(t1);
                }

                newsFiles = newsFiles.Where(x => Titles.Contains(x.Title) == false);
                //Process items
                var items = NewsParser.parseItem(newsFiles);

                if (t2 != null)
                {
                    Task.WaitAll(t2);
                }


                foreach (var item in items.Where(x => x.Categories != null && x.Categories.Count() > 0 && x.Content != "Error while fetching"))
                {
                    String last = item.Categories.Last();
                    var cat = Categories.Where(x => x.Name == last).FirstOrDefault();
                    if (cat != null)
                    {
                        item.CategoryId = cat.CategoryId;
                    }
                    else
                    {
                        //Save categories into database
                        int? parentId = null;
                        foreach (var c in item.Categories)
                        {
                            var s = Categories.Where(x => x.Name == c).FirstOrDefault();
                            if (s == null)
                            {
                                var category = repo.Categories.Add(new Category { Name = c, ParentCategoryId = parentId });
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
                    }

                    repo.NewsFiles.Add(AutoMapper.Mapper.Map<NewsFileExt, NewsFile>(item));
                    count += 1;

                }
                repo.SaveChanges();
                Console.WriteLine("{0} new sites added.", count);
            }
            return count;
        }

    }
}
