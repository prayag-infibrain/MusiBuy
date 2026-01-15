using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces.Marketing
{
    public interface IMarketingUser
    {
        IQueryable<MarketingUserViewModel> GetUserList(string searchValue);
        bool IsUserExists(int userID, string userName);
        bool IsEmailExists(int userID, string email);
        bool Save(MarketingUserViewModel MarketingUserViewModel);
        MarketingUserViewModel GetUserDetailsByID(int userID);
        MarketingUserViewModel GetUserDetailsEmail(string email, string userName, string mobileNo);
        bool DeleteUsers(int[] ids);
        int GetUserCount();
        List<DropDownBindViewModel> GetMarketingUserDropDownList();
    }
}
