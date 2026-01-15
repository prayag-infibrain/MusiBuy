using MusiBuy.Common.DB;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ITokenPlans
    {
        IQueryable<TokenPlanViewModel> GetTokenPlanList(string searchValue);
        bool IsTokenPlanExists(int TokenPlanID, int TokenTypeId);
        TokenPlanViewModel GetTokenPlanDetailsByID(int TokenPlanID);
        bool DeleteTokenPlans(int[] ids);
        bool Save(TokenPlanViewModel TokenPlanViewModel);
        int GetTokenPlanCount();
        List<DropDownBindViewModel> GetTokenPlanDropDownList(bool IsActive = true);
    }
}
