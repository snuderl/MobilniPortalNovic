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

        public ActionResult Index()
        {
            var articles = context.NewsFiles.OrderBy(pub => pub.PubDate).Take(15).ToList().Select(p => new SyndicationItem(p.Title, p.ShortContent, uriMaker(p.FeedId), p.FeedId.ToString(), p.PubDate));

            var feed = new SyndicationFeed("Novice", "Your source to knowledge", new Uri(Url.Action("Index", "Home", new { }, "http")).SetPort(80), articles);

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

        private Uri uriMaker(int id)
        {
            return new Uri(Url.Action("Details", "NewsFiles", new { id = id }, "http")).SetPort(80);
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

    }
}
