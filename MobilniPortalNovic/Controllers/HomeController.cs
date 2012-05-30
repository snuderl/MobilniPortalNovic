using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovicLib;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(MobilniPortalNovicLib.ParsingService.getParsingService());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

        public ActionResult RunUpdate()
        {
            var service = MobilniPortalNovicLib.ParsingService.getParsingService();
            if (service.State == MobilniPortalNovicLib.State.Waiting)
            {
                new Thread(new ThreadStart(service.startParse)).Start();
                return Content("Started update");
            }
            return Content("Update runnning");
        }

    }
}
