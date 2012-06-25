using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovic.Controllers
{
    public class PublicController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Public/
        public ActionResult Login(User u)
        {
            if (u != null)
            {
                var context = new MobilniPortalNovicContext12();
                if (context.Users.Where(x => x.Password == u.Password && x.Username == u.Username).FirstOrDefault() != null)
                {
                    Session["username"] = u.Username;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Error", "Username of parssword is invalid");
                return View(u);
            }
            return View(new User());
        }


    }
}
