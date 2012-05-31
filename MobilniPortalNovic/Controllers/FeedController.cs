using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovic.Helpers;
using System.Web.Security;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Controllers
{
    public class FeedController : Controller
    {
        MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();
        //
        // GET: /Feed/

        public ActionResult Index(int page=0)
        {
            var articles = context.NewsFiles.OrderBy(pub => pub.PubDate).Skip(page*15).Take(15).ToList().Select(p => new SyndicationItem(p.Title, p.ShortContent, uriMaker(p.NewsId), p.NewsId.ToString(), p.PubDate));

            var feed = new SyndicationFeed("Novice", "Your source to knowledge", new Uri(Url.Action("Index", "Home", new { }, "http")).SetPort(80), articles);

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

        private Uri uriMaker(int id)
        {
            return new Uri(Url.Action("NewsFile", "Feed", new { id = id }, "http")).SetPort(80);
        }

        [HttpPost]
        public String Click(ClickCounter click)
        {
            if (ModelState.IsValid)
            {
                context.Clicks.Add(click);
                context.SaveChanges();
                return "Click saved";
            }


            return "Bad info";
        }

        public JsonResult NewsFile(int id)
        {
            return Json(context.NewsFiles.Where(x => x.FeedId == id).First(), JsonRequestBehavior.AllowGet);
        }

    }
}
