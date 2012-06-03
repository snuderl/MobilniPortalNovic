using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobilniPortalNovicLib.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using Worker.Parsers;

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

        public ConcurrentDictionary<String, ConcurrentDictionary<String, State>> RunningInfo { get; set; }



        private ParsingService()
        {
            State = State.Waiting;
            RunningInfo = new ConcurrentDictionary<string, ConcurrentDictionary<String, State>>();
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
                var context = new MobilniPortalNovicContext12();
                var sites = new ConcurrentBag<NewsSite>(context.NewsSites.ToList());
                RunningInfo.Clear();
                foreach (var site in sites)
                {
                    var innerDict = new ConcurrentDictionary<String, State>();
                    RunningInfo.TryAdd(site.Name, innerDict);

                    UpdateFeedsForSite(site, innerDict);
                }

                State = State.Waiting;
                watch.Stop();
                Console.WriteLine("Run finished in {0}", watch.Elapsed);

            }
            else
            {
                throw new Exception("Parser already working");
            }
        }


        private int UpdateFeedsForSite(NewsSite site, ConcurrentDictionary<String, State> dict)
        {
            using (var repo = new MobilniPortalNovicContext12())
            {
                IEnumerable<String> titles = repo.NewsFiles.Select(x => x.Title).ToList();
                List<NewsFile> newsList = new List<NewsFile>();
                Parallel.ForEach(site.Feeds, f =>
                {

                    dict.TryAdd(f.url, State.Processing);
                    var time = DateTime.Now;
                    //Get items from feed
                    var feeds = FeedParser.parseFeed(f);
                    //Process items
                    var items = NewsParser.parseItem(feeds);

                    foreach (var item in items)
                    {

                        item.Content = Helper.ExtractText(item.Content);
                        item.ShortContent = Helper.ExtractText(item.ShortContent);
                        item.CategoryId = f.CategoryId;
                        newsList.Add(item);


                    };
                    f.LastUpdated = time;
                    dict.TryUpdate(f.url, State.Finished, State.Processing);
                    repo.SaveChanges();


                });

                var i = 0;
                foreach (var f in newsList)
                {
                    if (!titles.Contains(f.Title))
                    {
                        repo.NewsFiles.Add(f);
                        i += 1;
                    }
                }
                repo.SaveChanges();
                Console.WriteLine("{0} new sites added.", i);
            }
            return 0;
        }

    }
}
