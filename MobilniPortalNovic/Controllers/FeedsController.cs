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
    public class FeedsController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /Feeds/

        public ViewResult Index()
        {
            return View(context.Feeds.Include(feed => feed.Category).Include(feed => feed.NewsSite).ToList());
        }

        //
        // GET: /Feeds/Details/5

        public ViewResult Details(int id)
        {
            Feed feed = context.Feeds.Single(x => x.FeedId == id);
            return View(feed);
        }

        //
        // GET: /Feeds/Create

        public ActionResult Create()
        {
            ViewBag.PossibleCategories = context.Categories;
            ViewBag.PossibleNewsSites = context.NewsSites;
            return View();
        } 

        //
        // POST: /Feeds/Create

        [HttpPost]
        public ActionResult Create(Feed feed)
        {
            if (ModelState.IsValid)
            {
                feed.LastUpdated = DateTime.Now;
                context.Feeds.Add(feed);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossibleCategories = context.Categories;
            ViewBag.PossibleNewsSites = context.NewsSites;
            return View(feed);
        }
        
        //
        // GET: /Feeds/Edit/5
 
        public ActionResult Edit(int id)
        {
            Feed feed = context.Feeds.Single(x => x.FeedId == id);
            ViewBag.PossibleCategories = context.Categories.ToList();
            ViewBag.PossibleNewsSites = context.NewsSites.ToList();
            return View(feed);
        }

        //
        // POST: /Feeds/Edit/5

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Feed feed)
        {
            if (ModelState.IsValid)
            {
                context.Entry(feed).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleCategories = context.Categories.ToList();
            ViewBag.PossibleNewsSites = context.NewsSites.ToList();
            return View(feed);
        }

        //
        // GET: /Feeds/Delete/5
 
        public ActionResult Delete(int id)
        {
            Feed feed = context.Feeds.Single(x => x.FeedId == id);
            return View(feed);
        }

        //
        // POST: /Feeds/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Feed feed = context.Feeds.Single(x => x.FeedId == id);
            context.Feeds.Remove(feed);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}