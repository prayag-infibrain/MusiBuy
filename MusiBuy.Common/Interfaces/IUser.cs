using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IUser
    {
        IQueryable<UserViewModel> GetUserList(int userId, string searchValue);
        bool IsUserExists(int userID, string userName);
        bool IsEmailExists(int userID, string email);
        bool Save(UserViewModel userViewModel);
        UserViewModel GetUserDetailsByID(int userID);
        UserViewModel GetUserDetailsEmail(string email, string userName, string mobileNo);
        bool DeleteUsers(int[] ids);
        int GetUserCount();
        List<DropDownBindViewModel> GetUserDropDownList(bool IsActive = true);
    }
}
