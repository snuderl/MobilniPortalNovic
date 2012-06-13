using System;
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

        private static void SimulateClicks()
        {
            try
            {
                using (var context = new MobilniPortalNovicContext12())
                {
                    Console.WriteLine("UserId:");
                    int userId;
                    var i1 = Console.ReadLine();
                    if (Int32.TryParse(i1, out userId) == false)
                    {
                        userId = context.Users.Where(x => x.Username == i1).First().UserId;
                    }

                    Console.WriteLine("Number of clicks");
                    var count = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("Category number");

                    int category;
                    var i2 = Console.ReadLine();
                    if (Int32.TryParse(i2, out category) == false)
                    {
                        category = context.Categories.Where(x => x.Name == i2).First().CategoryId;
                    }
                    Console.WriteLine("Hour offset from now:");
                    int offset = Int32.Parse(Console.ReadLine());
                    Random rnd = new Random();
                    var clicks = new FillDatabase(new MobilniPortalNovicContext12()).SimulateClicks(userId, category, count, () =>
                    {
                        return DateTime.Now.AddHours(offset).AddMinutes(rnd.Next(-1000, 1000));
                    }
                    );
                    Console.WriteLine("{0} clicks added.", clicks.Count());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad input");
            }
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
            inputDictionary.Add("simulate", new CommandOption { Description = "Simulate clicks", Action = new Action(() => SimulateClicks()) });
            inputDictionary.Add("check", new CommandOption
            {
                Description = "Check for duplicates",
                Action = new Action(() =>
                {
                    var dateToCheck = DateTime.Now.AddDays(-1);
                    new MobilniPortalNovicContext12().NewsFiles.Where(x => x.PubDate > dateToCheck).GroupBy(x => x.Title).Where(x => x.Count() > 1).ToList().ForEach(x => Console.WriteLine(x.Key));
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