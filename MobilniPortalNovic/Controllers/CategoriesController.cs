using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MobilniPortalNovicLib.Models;

namespace Web.Controllers
{
    public class CategoriesController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();

        //
        // GET: /Categories/

        public ActionResult Index(string format = "html")
        {
            var categories = context.Categories.ToList();
            if (format == "json")
            {
                var jsonObject = categories.Where(x => x.ParentCategoryId == null).Select(x => new
                {
                    Name = x.Name,
                    Id = x.CategoryId,
                    Children = categories.Where(y=>y.ParentCategoryId==x.CategoryId).Select(y=>
                        new{
                            Name=y.Name,
                            Id = x.CategoryId,
                            Children=new List<Category>()
                        })
                });
                return Json(jsonObject, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(categories.ToList());
            }
        }

        //
        // GET: /Categories/Details/5

        public ViewResult Details(int id)
        {
            Category category = context.Categories.Single(x => x.CategoryId == id);
            return View(category);
        }

        //
        // GET: /Categories/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Categories/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //
        // GET: /Categories/Edit/5

        public ActionResult Edit(int id)
        {
            Category category = context.Categories.Single(x => x.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Categories/Delete/5

        public ActionResult Delete(int id)
        {
            Category category = context.Categories.Single(x => x.CategoryId == id);
            return View(category);
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = context.Categories.Single(x => x.CategoryId == id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            foreach (var c in context.Categories.ToList())
            {
                context.Categories.Remove(c);
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}