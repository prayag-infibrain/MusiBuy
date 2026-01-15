using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ICustomer
    {
        IQueryable<CustomerViewModel> GetCustomerList(int userId, string searchValue);
        //bool IsCreatoresExists(int userID, string userName);
        bool IsEmailExists(int userID, string email);
        bool Save(CustomerViewModel userViewModel);
        CustomerViewModel GetCustomerDetailsByID(int userID);
        CustomerViewModel GetCustomerDetailsEmail(string email, string userName, string mobileNo);
        bool DeleteCustomer(int[] ids);
        string? GetCustomerProfilePicture(int id);
        void RemoveCustomerProfilePicture(int id);
    }
}
