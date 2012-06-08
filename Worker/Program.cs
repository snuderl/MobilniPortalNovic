using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Personalize;
using Worker.Parsers;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)        {

            Scheduler sched = new Scheduler(60*10);
            ParsingService service = ParsingService.getParsingService();


            while (true)
            {
                Console.WriteLine("1: Start worker, 2: Stop worker, 3: Fill database, exit: exit");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        {
                            sched.StartUpdating();
                            break;
                        }
                    case "2":
                        {
                            sched.Stop();
                            break;
                        }
                    case "3":
                        {
                                Console.WriteLine("UserId:");
                                var userId = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Number of clicks");
                                var count = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Category number");
                                var category = Int32.Parse(Console.ReadLine());
                                Random rnd = new Random();
                                new FillDatabase(new MobilniPortalNovicContext12()).SimulateClicks(userId, category, count, () =>
                                {
                                    return DateTime.Now.AddMinutes(rnd.Next(-1000, 1000));
                                }
                                );

                            break;
                        }
                    case "4":
                        {
                            Category cat1 =
    new Category { CategoryId = 1, Name = "Šport", ParentCategoryId = new Nullable<int>() };
                            Category cat2 =
                                new Category { CategoryId = 2, Name = "Novice", ParentCategoryId = new Nullable<int>() };
                            Category cat3 =
                                new Category { CategoryId = 3, Name = "Zimski šport", ParentCategoryId = 1 };
                            Category cat4 = new Category { CategoryId = 4 };
                            Category cat5 = new Category { CategoryId = 5, ParentCategoryId = 3 };
                            Category cat6 = new Category { CategoryId = 6, ParentCategoryId = 3 };

                            IQueryable<NewsFile> news = new List<NewsFile>{
            new NewsFile{ NewsId=1, CategoryId=3,},
            new NewsFile{NewsId=2, CategoryId=2},
            new NewsFile{NewsId=3, CategoryId=1},
            new NewsFile{NewsId=6, CategoryId=2},
            new NewsFile{NewsId=7, CategoryId=1},
            new NewsFile{NewsId=4, CategoryId=3}}.AsQueryable();

                            IQueryable<Category> categories = (new List<Category> { cat1, cat2, cat3, cat4, cat5, cat6 }).AsQueryable();
                            var i = CategoryHelpers.getRowsByCategory(news,3,categories);
                            break;

                        }
                    case "5":
                        {
                            ParsingService ps = service;
                            ps.UpdateFeedsForSites();
                            break;

                        }
                    case "exit":
                        {
                            sched.Stop();
                            Console.WriteLine("Stoping...");
                            while (ParsingService.getParsingService().State != State.Waiting) {
                                Thread.Sleep(1000);
                            }
                            Environment.Exit(0);
                            break;
                        }
                    case "stats":
                        {
                            Console.WriteLine("Total updated {0}.", service.TotalCount);
                            Console.WriteLine("Last run {0}.", service.LastRun);
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
            }



            //while (true)
            //{
            //    ParsingService s = ParsingService.getParsingService();
            //    s.startParse();
            //    Thread.Sleep(10000);
            //}
        }
    }
}
