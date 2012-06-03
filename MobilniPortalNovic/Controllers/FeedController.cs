﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovic.Helpers;
using System.Web.Security;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Personalize;

namespace MobilniPortalNovic.Controllers
{
    public class FeedController : Controller
    {
        MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        public ActionResult RssAuthorized(int userId, int page = 0)
        {

            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            var articles = personalize.GetNews(context.Users.Find(userId)).Skip(page * 15).Take(15).ToList();
            return new FeedResult(listToRss(articles));
        }

        private Rss20FeedFormatter listToRss(IEnumerable<NewsFile> news)
        {
            var articles = news.Select(p => new SyndicationItem(p.Title, p.ShortContent, uriMaker(p.NewsId), p.NewsId.ToString(), p.PubDate));

            var head = new SyndicationFeed("Novice", "Your source to knowledge", new Uri(Url.Action("Index", "Home", new { }, "http")).SetPort(80), articles);

            return new Rss20FeedFormatter(head);
        }


        //
        // GET: /Feed/
        public ActionResult Index(int page = 0)
        {
            var articles = context.NewsFiles.OrderBy(pub => pub.PubDate).Skip(page * 15).Take(15).ToList();

            return new FeedResult(listToRss(articles));
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
                click.CategoryId = context.NewsFiles.Where(x => x.NewsId == click.NewsId).Select(x => x.CategoryId).First();
                context.Clicks.Add(click);
                context.SaveChanges();
                return "Click saved";
            }


            return "Bad info";
        }

        public JsonResult NewsFile(int id)
        {
            var i = context.NewsFiles.Where(x => x.NewsId == id).FirstOrDefault();
            var d = new { Title = i.Title, PubDate = i.PubDate, Category = i.Category.Name, Content = i.Content };
            return Json(d, JsonRequestBehavior.AllowGet);
        }

    }
}
