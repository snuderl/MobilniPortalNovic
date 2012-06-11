using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Helpers;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public class CategoryPersonalizer : IPersonalize
    {
        public MobilniPortalNovicContext12 Context { get; set; }
        public float CategoryTreshold { get; set; }

        public CategoryPersonalizer(MobilniPortalNovicContext12 context)
        {
            this.Context = context;
            CategoryTreshold = 70;
        }

        /// <summary>
        /// Get news for given user
        /// </summary>
        /// <param name="u"></param>
        /// <param name="categoryTreshold">Percent of total clicks to use in personalization</param>
        /// <returns></returns>
        public IQueryable<NewsFile> GetNews(User u)
        {
            var clicks = Context.Clicks.Include("NewsFile").Where(x => x.UserId == u.UserId).ToList();
            var categories = new CategoryWraper(Context.Categories.ToList());
            var count = CategoryHelpers.NumberOfCliksPerCategory(clicks.Select(x => x.NewsFile));

            var goodCategories = new HashSet<int>();
            float total = 0;
            var enumerator = count.OrderByDescending(x => x.Value).GetEnumerator();
            while(total<CategoryTreshold){
                total += enumerator.Current.Value;
                goodCategories.Add(enumerator.Current.Key);
                enumerator.MoveNext();
            }

            return Context.NewsFiles.Where(x => goodCategories.Contains(x.CategoryId)).OrderByDescending(x => x.PubDate);
        }
    }
}