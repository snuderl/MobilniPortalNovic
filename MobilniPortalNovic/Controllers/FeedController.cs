using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovic.Helpers;
using System.Web.Security;

namespace MobilniPortalNovic.Controllers
{
    public class FeedController : Controller
    {
        MobilniPortalNovicLib.Models.MobilniPortalNovicContext12 context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();
        //
        // GET: /Feed/

        public ActionResult Index()
        {
            var articles = context.NewsFiles.OrderBy(pub => pub.PubDate).Take(15).ToList().Select(p => new SyndicationItem(p.Title, p.ShortContent, new Uri(Url.Action("Details", "NewsFiles", new { id = p.FeedId }, "http")).SetPort(80)));

            var feed = new SyndicationFeed("Novice", "Your source to knowledge", new Uri(Url.Action("Index", "Home", new { }, "http")).SetPort(80), articles);

            return new FeedResult(new Rss20FeedFormatter(feed));
        }

    }
}
