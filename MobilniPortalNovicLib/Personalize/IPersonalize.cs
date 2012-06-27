using System.Collections.Generic;
using System.Linq;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public interface IPersonalize
    {
        IQueryable<NewsFile> GetNews(NewsRequest request);
        void AddMessage(string s);
    }

    public interface Filter
    {
        IQueryable<ClickCounter> Filter(IQueryable<ClickCounter> clicks);
        string GetMessage();
    }

}