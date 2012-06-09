using System.Data;
using System.Linq;
using System.Web.Mvc;
using MobilniPortalNovicLib.Models;

namespace Web.Controllers
{
    public class NewsSiteController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /NewsSite/

        public ViewResult Index()
        {
            return View(context.NewsSites.ToList());
        }

        //
        // GET: /NewsSite/Details/5

        public ViewResult Details(int id)
        {
            NewsSite newssite = context.NewsSites.Single(x => x.SiteId == id);
            return View(newssite);
        }

        //
        // GET: /NewsSite/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /NewsSite/Create

        [HttpPost]
        public ActionResult Create(NewsSite newssite)
        {
            if (ModelState.IsValid)
            {
                context.NewsSites.Add(newssite);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newssite);
        }

        //
        // GET: /NewsSite/Edit/5

        public ActionResult Edit(int id)
        {
            NewsSite newssite = context.NewsSites.Single(x => x.SiteId == id);
            return View(newssite);
        }

        //
        // POST: /NewsSite/Edit/5

        [HttpPost]
        public ActionResult Edit(NewsSite newssite)
        {
            if (ModelState.IsValid)
            {
                context.Entry(newssite).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newssite);
        }

        //
        // GET: /NewsSite/Delete/5

        public ActionResult Delete(int id)
        {
            NewsSite newssite = context.NewsSites.Single(x => x.SiteId == id);
            return View(newssite);
        }

        //
        // POST: /NewsSite/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsSite newssite = context.NewsSites.Single(x => x.SiteId == id);
            context.NewsSites.Remove(newssite);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}