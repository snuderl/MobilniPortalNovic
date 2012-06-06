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

            return Context.NewsFiles.OrderByDescending(x => x.PubDate);
        }
    }
}
