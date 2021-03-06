﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MobilniPortalNovicLib.Models;

namespace Worker
{
    internal class Program
    {
        private static Dictionary<String, CommandOption> inputDictionary = null;

        private class CommandOption
        {
            public String Description { get; set; }

            public Action Action { get; set; }
        }

       

        private static void Main(string[] args)
        {
            ParsingService service = ParsingService.getParsingService();
            Scheduler sched = new Scheduler(60 * 10, service);
            inputDictionary = new Dictionary<String, CommandOption>();
            inputDictionary.Add("start", new CommandOption { Description = "Start updating", Action = new Action(() => sched.StartUpdating()) });
            inputDictionary.Add("stop", new CommandOption
            {
                Description = "Stop automatic updating.",
                Action = new Action(
                    () =>
                    {
                        sched.Stop();
                        Console.WriteLine("Parser stopped");
                    })
            });
            inputDictionary.Add("simulate", new CommandOption { Description = "Simulate clicks", Action = new Action(() => FillDatabaseCmd.SimulateClicks()) });
            inputDictionary.Add("check", new CommandOption
            {
                Description = "Check for duplicates",
                Action = new Action(() =>
                {
                    var dateToCheck = DateTime.Now.AddDays(-1);
                    var repo = new MobilniPortalNovicContext12();
                    var i = repo.NewsFiles.Where(x => x.PubDate > dateToCheck).GroupBy(x => x.Title).Where(x => x.Count() > 1).ToList();
                    var count = 0;
                    i.ForEach(x =>
                        {
                            count += x.Count() - 1;
                            x.Skip(1).ToList().ForEach(y=>repo.NewsFiles.Remove(y));
                        });
                    repo.SaveChanges();
                    Console.WriteLine("Removed {0} duplicates", count);
                })
            });
            inputDictionary.Add("run",
                new CommandOption
                {
                    Description = "Run single update.",
                    Action = new Action(() =>
                {
                    service.startParse();
                })
                });
            inputDictionary.Add("exit", new CommandOption
            {
                Description = "Exit the program",
                Action = new Action(() =>
                {
                    sched.Stop();
                    Console.WriteLine("Stoping...");
                    while (ParsingService.getParsingService().State != State.WaitingToNextInterval)
                    {
                        Thread.Sleep(1000);
                    }
                    Environment.Exit(0);
                })
            });
            inputDictionary.Add("stats", new CommandOption
            {
                Description = "Show parsers statistics",
                Action = new Action(() =>
                {
                    Console.WriteLine("Scheduler is {0}, with interval {1}.", sched.State, sched.RepeatInterval);
                    Console.WriteLine("Parsing service in currently: {0}", service.State);
                    Console.WriteLine("Total updated {0}.", service.TotalCount);
                    Console.WriteLine("Last run {0}.", service.LastRun);
                })
            });
            inputDictionary.Add("AddNews", new CommandOption
            {
                Description="Manuly add news file",
                Action = new Action(() =>
                    {
                        try
                        {
                            Console.WriteLine("CategoryName");
                            var i = Console.ReadLine();
                            var context = new MobilniPortalNovicContext12();
                            var cat = context.Categories.Where(x => x.Name == i).FirstOrDefault();
                            var catId = cat.CategoryId;
                            NewsFile f = new NewsFile { 
                                CategoryId = catId,
                                FeedId = context.Feeds.First().FeedId,
                                Content = "Poljubna vsebina.",
                                Title = "Filler news",
                                ShortContent = "Ta novica je samo za testiranje",
                                PubDate=DateTime.Now,
                                Link="http//www.fake.si"
                            };
                            
                            context.NewsFiles.Add(f);
                            context.SaveChanges();
                            Console.WriteLine("Added new news file");
                        }
                        catch (Exception e)
                        {
                            
                            Console.WriteLine("Bad info");
                        }
                    })
            });
            inputDictionary.Add("clear", new CommandOption
            {
                Description = "Clear filler news",
                Action = new Action(() =>
                {
                    var c = new MobilniPortalNovicContext12();
                    c.NewsFiles.Where(x => x.Title == "Filler news").ToList().ForEach(x => c.NewsFiles.Remove(x));
                    c.SaveChanges();
                })
            });

            while (true)
            {
                var input = Console.ReadLine();
                if (inputDictionary.ContainsKey(input))
                {
                    inputDictionary[input].Action();
                }
                else
                {
                    DisplayChoices();
                }
                Console.WriteLine();
            }
        }

        public static void DisplayChoices()
        {
            foreach (var i in inputDictionary)
            {
                Console.WriteLine("{0}: {1}", i.Key, i.Value.Description);
            }
        }
    }
}