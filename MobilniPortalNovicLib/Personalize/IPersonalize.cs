using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public interface IPersonalize
    {
        IQueryable<NewsFile> GetNews(User u);
    }
}
