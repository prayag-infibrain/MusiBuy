using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IClient
    {
        IQueryable<ClientViewModel> GetUserList(int userId, string searchValue);
        bool IsUserExists(int userID, string firstname);
        bool IsEmailExists(int userID, string email);
        bool Save(ClientViewModel userViewModel);
        ClientViewModel GetUserDetailsByID(int userID);
        public ClientViewModel GetUserDetailsEmail(string email);
        bool DeleteUsers(int[] ids);
        public List<DropDownBindViewModel> GetFrontRoles();
    }
}
