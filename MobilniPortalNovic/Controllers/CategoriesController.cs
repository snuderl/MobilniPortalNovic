using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using MobilniPortalNovic.ModelView;
using MobilniPortalNovicLib.Models;

namespace Web.Controllers
{
    public class JsonCategory
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<JsonCategory> Children { get; set; }
    }

    public class CategoriesController : Controller
    {
        private MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();
        //
        // GET: /Categories/

        public ActionResult Index(string format = "html")
        {
            var categories = context.Categories.Include("ParentCategory").ToList();
            if (format == "json")
            {
                var jsonObject = categories.Where(x => x.ParentCategoryId == null).Select(x => new JsonCategory
                {
                    Name = x.Name,
                    Id = x.CategoryId,
                    Children = categories.Where(y => y.ParentCategoryId == x.CategoryId).Select(y =>
                    {
                        var name = y.Name;
                        if (name == "Novice")
                        {
                            name = x.Name + " novice";
                        }
                        return new JsonCategory
                        {
                            Name = name,
                            Id = x.CategoryId,
                            Children = new List<JsonCategory>()
                        };
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
            Category category = context.Categories.Include("ParentCategory").Include("Children").Single(x => x.CategoryId == id);
            return View(new CategoryModelView
            {
                Category = category,
                NewsCount=context.NewsFiles.Where(x=>category.CategoryId==x.CategoryId).Count()
            });
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