//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using MobilniPortalNovicLib.Models;
//using System.Collections.Concurrent;

//namespace MobilniPortalNovicLib
//{
//    public class Program
//    {




//        public static int UpdateFeeds(NewsSite site, ConcurrentDictionary<String, MobilniPortalNovicLib.State> dict)
//        {

//            {
//                RssParser parser = new RssParser();
//                Parallel.ForEach(site.Feeds, f =>
//                {
//                    using (var repo = new MobilniPortalNovicContext1())
//                    {
//                        dict.TryAdd(f.url, State.Processing);
//                        var time = DateTime.Now;
//                        var feeds = parser.parseRss(f);
//                        foreach (var file in feeds)
//                        {

//                            file.Content = MobilniPortalNovicLib.Helper.ExtractText(file.Content);
//                            file.ShortContent = Helper.ExtractText(file.ShortContent);
//                            if (repo.NewsFiles.Where(x => x.Title != file.Title).Count() == 0)
//                            {
//                                repo.NewsFiles.Add(file);
//                            }

//                        };
//                        f.LastUpdated = time;
//                        dict.TryUpdate(f.url, State.Finished, State.Processing);
//                        repo.SaveChanges();
//                    }
//                });
//            }
//            return 0;

//        }
//    }
//}
