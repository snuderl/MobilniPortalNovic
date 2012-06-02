using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovicLib.Models;

namespace Web.Controllers
{
    public class NewsFilesController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /NewsFiles/

        public ViewResult Index()
        {
            return View(context.NewsFiles.Include(newsfile => newsfile.Feed).OrderByDescending(x => x.PubDate).ToList());
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
            ViewBag.PossibleFeeds = context.Feeds;
            return View(newsfile);
        }

        //
        // POST: /NewsFiles/Edit/5

        [HttpPost]
        public ActionResult Edit(NewsFile newsfile)
        {
            if (ModelState.IsValid)
            {
                context.Entry(newsfile).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleFeeds = context.Feeds.ToList();
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