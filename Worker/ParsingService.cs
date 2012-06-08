﻿using System;
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

        public Dictionary<String, int> Categories = new Dictionary<string, int>();



        private ParsingService()
        {

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
            IEnumerable<NewsSite> sites;
            if (State == State.Waiting)
            {
                Console.WriteLine("Starting run.");
                State = State.Processing;

                UpdateFeedsForSites();

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
            using (var repo = new MobilniPortalNovicContext12())
            {




                var t1 = Task.Factory.StartNew(() =>
                {
                    using (var context = new MobilniPortalNovicContext12())
                    {
                        return context.NewsFiles.Select(y => y.Title).ToList();
                    }
                });
                var t2 =Task.Factory.StartNew(() =>
                {
                    using (var context = new MobilniPortalNovicContext12())
                    {
                        Categories = context.Categories.ToDictionary(x=>x.Name, x=>x.CategoryId);
                    }
                });               
                var count = 0;

                List<NewsFileExt> newsList = new List<NewsFileExt>();
                var time = DateTime.Now;
                //Get items from feed
                var feeds = repo.Feeds.Include("Category").ToList();
                feeds.ForEach(x => x.LastUpdated = time);
                var newsFiles = feeds.AsParallel().Select(x => FeedParser.parseFeed(x)).SelectMany(x => x);
                Task.WaitAll(t1);

                newsFiles = newsFiles.Where(x => t1.Result.Contains(x.Title) == false);
                //Process items
                var items = NewsParser.parseItem(newsFiles);

                Task.WaitAll(t2);


                foreach (var item in items.Where(x => x.Categories != null && x.Categories.Count() > 0 && x.Content != "Error while fetching"))
                {
                    String last = item.Categories.Last();
                    var cat = repo.Categories.Where(x => x.Name == last).FirstOrDefault();
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
                            var s = repo.Categories.Where(x => x.Name == c).FirstOrDefault();
                            if (s == null)
                            {
                                var category = repo.Categories.Add(new Category { Name = c, ParentCategoryId = parentId });
                                repo.SaveChanges();
                                parentId = category.CategoryId;
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
            return 0;
        }

    }
}
