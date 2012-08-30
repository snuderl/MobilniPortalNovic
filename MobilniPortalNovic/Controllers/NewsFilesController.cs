using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MobilniPortalNovicLib.Models;
using PagedList;

namespace Web.Controllers
{
    public class NewsFilesController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /NewsFiles/
                public ViewResult Index(int? page)
        {
            var news = context.NewsFiles.Include(newsfile => newsfile.Feed).OrderByDescending(x => x.PubDate);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = news.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            return View(onePageOfProducts);
        }

        //
        // GET: /NewsFiles/Details/5

        public ViewResult Details(int id)
        {
            NewsFile newsfile = context.NewsFiles.Single(x => x.NewsId == id);
            return View(newsfile);
        }

        //
        // GET: /NewsFiles/Create

        public ActionResult Create()
        {
            ViewBag.PossibleFeeds = context.Feeds;
            return View();
        }

        //
        // POST: /NewsFiles/Create

        [HttpPost]
        public ActionResult Create(NewsFile newsfile)
        {
            if (ModelState.IsValid)
            {
                context.NewsFiles.Add(newsfile);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleFeeds = context.Feeds;
            return View(newsfile);
        }

        //
        // GET: /NewsFiles/Edit/5

        public ActionResult Edit(int id)
        {
            NewsFile newsfile = context.NewsFiles.Single(x => x.NewsId == id);
            return View(newsfile);
        }

        //
        // POST: /NewsFiles/Edit/5

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(NewsFile newsfile)
        {
            if (ModelState.IsValid)
            {
                var news = context.NewsFiles.Find(newsfile.NewsId);
                news.Content = newsfile.Content;
                news.Title = newsfile.Title;
                news.ShortContent = newsfile.ShortContent;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsfile);
        }

        //
        // GET: /NewsFiles/Delete/5

        public ActionResult Delete(int id)
        {
            NewsFile newsfile = context.NewsFiles.Single(x => x.NewsId == id);
            return View(newsfile);
        }

        //
        // GET: /NewsFiles/Delete/5

        public ActionResult DeleteAll()
        {
            foreach (var x in context.NewsFiles)
            {
                context.NewsFiles.Remove(x);
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /NewsFiles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsFile newsfile = context.NewsFiles.Single(x => x.NewsId == id);
            context.NewsFiles.Remove(newsfile);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}