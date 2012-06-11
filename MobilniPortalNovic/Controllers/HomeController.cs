using System.Web.Mvc;
using MobilniPortalNovic.ModelView;
using System.Linq;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();

            //return View(MobilniPortalNovicLib.ParsingService.getParsingService());
            return View(new HomePageModel
            {
                CategoriesCount = context.Categories.Count(),
                NewsFileCount = context.NewsFiles.Count(),
                NewsLastUpdated = context.Feeds.OrderBy(x=>x.LastUpdated).First().LastUpdated
            });
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

        //public ActionResult RunUpdate()
        //{
        //    var service = MobilniPortalNovicLib.ParsingService.getParsingService();
        //    if (service.State == MobilniPortalNovicLib.State.Waiting)
        //    {
        //        new Thread(new ThreadStart(service.startParse)).Start();
        //        return Content("Started update");
        //    }
        //    return Content("Update runnning");
        //}
    }
}