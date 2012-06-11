using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilniPortalNovicLib.Helpers
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns number of rows at random from iqueryable
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static public IQueryable<T> getNumberOfRandomRows<T>(this IQueryable<T> query, int count)
        {
            return query.OrderBy(x => Guid.NewGuid()).Take(count);
        }
    }
}
