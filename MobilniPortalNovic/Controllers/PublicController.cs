﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilniPortalNovic.ModelView;
using MobilniPortalNovicLib.Models;
using PagedList;
using MobilniPortalNovicLib.Personalize;
using MobilniPortalNovic.Helpers;

namespace MobilniPortalNovic.Controllers
{
    public class PublicController : Controller
    {

        MobilniPortalNovicContext12 context = new MobilniPortalNovicContext12();
        //
        // GET: /Feed/

        [CustomAuthorize]
        public ActionResult Index(int page = 1)
        {
            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            var userName = Session["username"].ToString();
            var request = NewsRequest.Construct(userName, context);
            IQueryable<NewsFile> articles = personalize.GetNews(request).OrderByDescending(x => x.PubDate);
            var items = articles;
            return View("~/Views/Feed/Index.cshtml", new RssHtmlModelView
            {
                includedCategories = items.Select(x => x.Category).GroupBy(x => x.CategoryId).Select(x => x.FirstOrDefault()).ToList(),
                newsFiles = items.Take(30).ToList(),
                FilterMessages = personalize.Messages
            });
        }


        [CustomAuthorize]
        public ActionResult PersonalizedHtmlTable(int page = 1)
        {
            CategoryPersonalizer personalize = new CategoryPersonalizer(context);
            var username = Session["username"].ToString();
            var request = NewsRequest.Construct(username, context);
            IQueryable<NewsFile> articles = personalize.GetNews(request).OrderByDescending(x => x.PubDate);
            var items = articles.ToList();
            return View("~/Views/NewsFiles/Index.cshtml", items.ToPagedList(page, 25));
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
