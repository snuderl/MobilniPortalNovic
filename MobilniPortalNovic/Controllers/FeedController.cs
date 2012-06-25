using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using MobilniPortalNovic.Helpers;
using MobilniPortalNovic.ModelView;
using MobilniPortalNovicLib.Models;
using MobilniPortalNovicLib.Personalize;
using PagedList;

namespace MobilniPortalNovic.Controllers
{
    public class FeedController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        private Rss20FeedFormatter listToRss(IEnumerable<NewsFile> news)
        {
            var set = new HashSet<String>();
            var articles = news.Select(
                p =>
                {
                    var i = new SyndicationItem(p.Title, p.ShortContent, uriMaker(p.NewsId), p.NewsId.ToString(), p.PubDate);
                    var catString = p.Category.Name;
                    if (catString == "Novice")
                    {
                        catString = p.Category.ParentCategory.Name + " novice";
                    }
                    i.Categories.Add(new SyndicationCategory(catString));
                    i.PublishDate = p.PubDate;
                    return i;
                });
            var head = new SyndicationFeed("Novice", "Your source to knowledge", new Uri(Url.Action("Index", "Home", new { }, "http")).SetPort(80), articles);
            set = new HashSet<string>(articles.Select(x => x.Categories.Select(y => y.Name).First()));
            foreach (var i in set)
            {
                head.Categories.Add((new SyndicationCategory(i)));
            }

            return new Rss20FeedFormatter(head);
        }

        //
        // GET: /Feed/
        public ActionResult Index(DateTime? lastDate, int userId = 0)
        {
            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            IQueryable<NewsFile> articles;
            if (userId == 0)
            {
                articles = context.NewsFiles.OrderByDescending(pub => pub.PubDate);
            }
            else
            {
                articles = personalize.GetNews(context.Users.Find(userId)).OrderByDescending(x => x.PubDate);
            }
            if (lastDate != null)
            {
                articles = articles.Where(x => x.PubDate < lastDate);
            }

            return new FeedResult(listToRss(articles.Take(30).ToList()));
        }

        //
        // GET: /Feed/
        public ActionResult IndexHtml(int userId = 0,int page=1)
        {
            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            IQueryable<NewsFile> articles;
            if (userId == 0)
            {
                articles = context.NewsFiles.OrderByDescending(pub => pub.PubDate);
            }
            else
            {
                articles = personalize.GetNews(context.Users.Find(userId)).OrderByDescending(x => x.PubDate);
            }
            var items = articles;
            return View("Index", new RssHtmlModelView
            {
                includedCategories = items.Select(x => x.Category).GroupBy(x => x.CategoryId).Select(x => x.FirstOrDefault()).ToList(),
                newsFiles = items.Take(30).ToList(),
                FilterMessages = personalize.Messages
            });
        }

        public ActionResult PersonalizedHtmlTable(int userId = 0, int page = 1)
        {
            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            IQueryable<NewsFile> articles;
            if (userId == 0)
            {
                articles = context.NewsFiles.OrderByDescending(pub => pub.PubDate);
            }
            else
            {
                articles = personalize.GetNews(context.Users.Find(userId)).OrderByDescending(x => x.PubDate);
            }
            var items = articles.ToList();
            return View("~/Views/NewsFiles/Index.cshtml", items.ToPagedList(page, 25));
        }




        //
        // GET: /Feed/
        public ActionResult IndexSkip(DateTime? lastDate, string format = "rss")
        {

            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            IQueryable<NewsFile> articles;
            articles = context.NewsFiles.OrderByDescending(pub => pub.PubDate);


            return new FeedResult(listToRss(articles.Where(x => x.PubDate < lastDate).Take(30).ToList()));
        }

        public ActionResult CategoryView(int id, int page = 0)
        {
            var articles = context.NewsFiles.Where(x => x.Category.CategoryId == id || x.Category.ParentCategoryId == id).OrderByDescending(x => x.PubDate);
            return new FeedResult(listToRss(paging(articles, page).ToList()));
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

        private IQueryable<NewsFile> paging(IQueryable<NewsFile> query, int page = 0, int pageSize = 30)
        {
            return query.Skip(page * pageSize).Take(pageSize);
        }
    }
}