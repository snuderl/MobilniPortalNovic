using System.Linq;
using System.Web.Mvc;
using MobilniPortalNovic.ModelView;

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
                NewsLastUpdated = context.Feeds.OrderBy(x => x.LastUpdated).First().LastUpdated
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

        public ActionResult Delete()
        {
            var context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();
            context.Database.ExecuteSqlCommand("truncate table ClickCounters");
            foreach (var x in context.NewsFiles)
            {
                context.NewsFiles.Remove(x);
            }
            foreach (var c in context.Categories.ToList())
            {
                context.Categories.Remove(c);
            }
            context.SaveChanges();
            return RedirectToAction("Index");
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