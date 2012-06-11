using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MobilniPortalNovic.ModelView;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Controllers
{
    public class UserController : Controller
    {
        private MobilniPortalNovicContext12 db = new MobilniPortalNovicContext12();

        //
        // GET: /User/

        public ViewResult Index()
        {
            return View(db.Users.ToList());
        }

        public Dictionary<String, int> getFeedStats(IEnumerable<ClickCounter> a)
        {
            var dict = new Dictionary<String, int>();
            foreach (var i in a)
            {
                var category = i.NewsFile.Feed.FeedName;
                if (dict.ContainsKey(category))
                {
                    dict[category] = dict[category] + 1;
                }
                else
                {
                    dict.Add(category, 1);
                }
            }
            return dict;
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(int id)
        {
            User user = db.Users.Find(id);
            var c = db.Clicks.Include(x => x.NewsFile).Where(x => x.UserId == id).ToList();
            var categories = db.Categories.ToList();

            var categoryStats = CategoryHelpers.NumberOfCliksPerCategory(c.Select(x => x.NewsFile)).ToList()
                .ToDictionary(x => categories.Where(y => x.Key == y.CategoryId).First().Name, x=>x.Value);

            return View(new UserDetailsModel
            {
                clicks = c,
                Id = id,
                Username = user.Username,
                categoryStats = categoryStats,
                feedStats = getFeedStats(c)
            });
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}