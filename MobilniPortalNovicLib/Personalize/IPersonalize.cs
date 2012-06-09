using System.Linq;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    public interface IPersonalize
    {
        IQueryable<NewsFile> GetNews(User u);
    }
}