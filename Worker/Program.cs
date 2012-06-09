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
            Console.WriteLine("UserId:");
            var userId = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Number of clicks");
            var count = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Category number");
            var category = Int32.Parse(Console.ReadLine());
            Random rnd = new Random();
            var clicks = new FillDatabase(new MobilniPortalNovicContext12()).SimulateClicks(userId, category, count, () =>
            {
                return DateTime.Now.AddMinutes(rnd.Next(-1000, 1000));
            }
            );
            Console.WriteLine("{0} clicks added.", clicks.Count());
        }

        private static void Main(string[] args)
        {
            ParsingService service = ParsingService.getParsingService();
            Scheduler sched = new Scheduler(60 * 10, service);
            inputDictionary = new Dictionary<String, CommandOption>();
            inputDictionary.Add("start", new CommandOption { Description = "Start updating", Action = new Action(() => sched.StartUpdating()) });
            inputDictionary.Add("stop", new CommandOption { Description = "Stop automatic updating.", Action = new Action(() => sched.Stop()) });
            inputDictionary.Add("simulate", new CommandOption { Description = "Simulate clicks", Action = new Action(() => SimulateClicks()) });
            inputDictionary.Add("run",
                new CommandOption
                {
                    Description = "Run single update.",
                    Action = new Action(() =>
                {
                    ParsingService ps = service;
                    ps.startParse();
                })
                });
            inputDictionary.Add("exit", new CommandOption
            {
                Description = "Exit the program",
                Action = new Action(() =>
                {
                    sched.Stop();
                    Console.WriteLine("Stoping...");
                    while (ParsingService.getParsingService().State != State.Waiting)
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