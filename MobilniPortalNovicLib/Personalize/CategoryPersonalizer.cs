using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class CategoryPersonalizer : IPersonalize
    {
        public MobilniPortalNovicContext12 Context { get; set; }
        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
        }
        public IQueryable<NewsFile> GetNews(User u)
        {
            var i = Context.Clicks.Where(x => x.UserId == u.UserId).GroupBy(x => x.CategoryId).Select(x => new { id = x.Key, count = x.Count() }).
             OrderByDescending(x => x.count).First();

            return Context.NewsFiles.Where(x => x.CategoryId == i.id).OrderByDescending(x => x.PubDate);
        }
    }
}
