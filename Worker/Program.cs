﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MobilniPortalNovicLib.Models;
using Worker.Parsers;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)        {

            Scheduler sched = new Scheduler(120);


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
                                new FillDatabase().SimulateClicks(userId, category, count, () =>
                                {
                                    return DateTime.Now.AddMinutes(rnd.Next(-1000, 1000));
                                }
                                );

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
                    default:
                        {
                            break;
                        }

                }
            }


            System.Environment.Exit(0);

            //while (true)
            //{
            //    ParsingService s = ParsingService.getParsingService();
            //    s.startParse();
            //    Thread.Sleep(10000);
            //}
        }
    }
}
