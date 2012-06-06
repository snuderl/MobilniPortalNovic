using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MobilniPortalNovicLib.Models;
using Worker.Parsers;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.CreateMap<NewsFileExt, NewsFile>();
            Scheduler sched = new Scheduler(120);
            sched.StartUpdating();

            Console.ReadLine();
            sched.Stop();

            System.Environment.Exit(0);

            //ParsingService s = ParsingService.getParsingService();
            //s.startParse();
        }
    }
}
