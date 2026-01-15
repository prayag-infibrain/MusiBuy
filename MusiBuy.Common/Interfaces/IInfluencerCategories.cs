using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IInfluencerCategories
    {
        IQueryable<InfluencerCategoriesViewModel> GetInfluencerCategoriesList(int userId, string searchValue);
        bool IsInfluencerCategoriesExists(int userID, string userName);
        bool Save(InfluencerCategoriesViewModel objmodel);
        InfluencerCategoriesViewModel GetInfluencerCategoriesByID(int userID);
        bool DeleteUsers(int[] ids);
    }
}
