using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovic.ModelView;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Controllers
{
    public class ClickCountersController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /ClickCounters/

        public ViewResult Index(bool limit = true)
        {
            var dict = CategoryHelpers.categoryChildrenLookup(context.Categories.ToList());
            IQueryable<ClickCounter> clicks = context.Clicks.OrderByDescending(x=>x.ClickId);
            if(limit){
                clicks = clicks.Take(100);
            }
            return View(new ClickModelView
            {
                Clicks = clicks.ToList(),
                CategoryParents = dict
            });
        }

        //
        // GET: /ClickCounters/Details/5

        public ViewResult Details(int id)
        {
            ClickCounter clickcounter = context.Clicks.Single(x => x.ClickId == id);
            return View(clickcounter);
        }

        //
        // GET: /ClickCounters/Create

        public ActionResult Create()
        {
            ViewBag.PossibleUsers = context.Users;
            return View();
        }

        //
        // POST: /ClickCounters/Create

        [HttpPost]
        public ActionResult Create(ClickCounter clickcounter)
        {
            if (ModelState.IsValid)
            {
                context.Clicks.Add(clickcounter);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PossibleUsers = context.Users;
            return View(clickcounter);
        }

        //
        // GET: /ClickCounters/Edit/5

        public ActionResult Edit(int id)
        {
            ClickCounter clickcounter = context.Clicks.Single(x => x.ClickId == id);
            ViewBag.PossibleUsers = context.Users;
            return View(clickcounter);
        }

        //
        // POST: /ClickCounters/Edit/5

        [HttpPost]
        public ActionResult Edit(ClickCounter clickcounter)
        {
            if (ModelState.IsValid)
            {
                context.Entry(clickcounter).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossibleUsers = context.Users;
            return View(clickcounter);
        }

        //
        // GET: /ClickCounters/Delete/5

        public ActionResult Delete(int id)
        {
            ClickCounter clickcounter = context.Clicks.Single(x => x.ClickId == id);
            return View(clickcounter);
        }

        //
        // POST: /ClickCounters/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClickCounter clickcounter = context.Clicks.Single(x => x.ClickId == id);
            context.Clicks.Remove(clickcounter);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}