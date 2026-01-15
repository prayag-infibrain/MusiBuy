
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories
{
    public class CustomerRepository : ICustomer
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;

        public CustomerRepository(MusiBuyDB_Connection context, IConfiguration configuration)
        {
            this._Context = context;
            _config = configuration;
        }
        #endregion

        #region Get user data for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<CustomerViewModel> GetCustomerList(int userId, string searchValue)
        {
            var result = (from c in _Context.Customers
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          c.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                          c.LastName.ToLower().Contains(searchValue.ToLower()) ||
                          c.Email.ToLower().Contains(searchValue.ToLower()) ||
                          c.Phone.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (c.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby c.FirstName
                          select new CustomerViewModel
                          {
                              Id = c.Id,
                              FirstName = c.FirstName,
                              LastName = c.LastName,
                              Email = c.Email,
                              Phone = "+" + c.Country.Code + " "+ c.Phone,
                              ShareContactDetail = c.ShareContactDetail,
                              IsActive = c.IsActive,
                              Active = c.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                          });

            return result;
        }
        #endregion

        #region Check User Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        /// Pending
        //public bool IsCustomerExists(int userID, string userName)
        //{
        //    return (from x in _Context.Customer where (string.IsNullOrWhiteSpace(userName) || x.Username == userName) && (userID == 0 || x.Id != userID) select x.Id).Any();
        //}
        #endregion

        #region Check Email Exists
        /// <summary>
        /// Check if email exists 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns>If user email already exists it returns true other wise false </returns>
        public bool IsEmailExists(int id, string email)
        {
            return (from x in _Context.Customers where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) select x.Id).Any();
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public CustomerViewModel GetCustomerDetailsByID(int CustomerID)
        {
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.CustomerImages.Replace("\\", "/").Trim('/');

            Customer? customerEntity = _Context.Customers.FirstOrDefault(p => p.Id == CustomerID);
            string fileName = customerEntity.ProfilePicture;

            CustomerViewModel? validateCreatore = (from u in _Context.Customers
                                                   where u.Id == CustomerID
                                                   select new CustomerViewModel
                                                   {
                                                       Id = u.Id,
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName,
                                                       Phone = u.Phone,
                                                       CountryId = u.CountryId,
                                                       Email = u.Email,
                                                       MediaFileName = u.ProfilePicture,
                                                       IsActive = u.IsActive,
                                                       StrProfilePicture = fileName != null ? $"{fileReadPath}{relativePath}/{fileName}" : null,
                                                       ShareContactDetail = u.ShareContactDetail,
                                                       Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText
                                                   }).FirstOrDefault();
            return validateCreatore;
        }
        #endregion

        #region Get User Details by Email
        /// <summary>
        /// Get single user by Email
        /// </summary>
        /// <returns>It returns user detail by email</returns>
        public CustomerViewModel GetCustomerDetailsEmail(string email, string userName, string mobileNo)
        {
            CustomerViewModel? validateCreatore = (from u in _Context.Customers
                                                       //where u.Email == email && u.Username == userName && u.Mobile == mobileNo
                                                   select new CustomerViewModel
                                                   {
                                                       Id = u.Id,
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName,
                                                       Phone = u.Phone,
                                                       Email = u.Email,
                                                       IsActive = u.IsActive,
                                                       ShareContactDetail = u.ShareContactDetail,
                                                   }).FirstOrDefault();
            return validateCreatore ?? new CustomerViewModel();
        }

        #endregion

        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteCustomer(int[] ids)
        {
            _Context.Customers.RemoveRange(_Context.Customers.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        public bool Save(CustomerViewModel CustomerViewModel)
        {
            var customerData = new Customer();
            if (CustomerViewModel.Id > 0)
            {
                customerData = _Context.Customers.FirstOrDefault(x => x.Id == CustomerViewModel.Id) ?? new Customer();
            }
            customerData.FirstName = (CustomerViewModel.FirstName ?? string.Empty).Trim();
            customerData.LastName = (CustomerViewModel.LastName ?? string.Empty).Trim();
            customerData.Phone = !(string.IsNullOrEmpty(CustomerViewModel.Phone)) ? (CustomerViewModel.Phone ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            customerData.Email = (CustomerViewModel.Email ?? string.Empty).Trim();
            customerData.ShareContactDetail = CustomerViewModel.ShareContactDetail;
            customerData.ProfilePicture = CustomerViewModel.MediaFileName;
            customerData.CountryId = CustomerViewModel.CountryId;
            customerData.IsActive = CustomerViewModel.IsActive;

            if (customerData.Id == 0)
            {
                customerData.CreatedOn = DateTime.Now;
                customerData.CreatedBy = CustomerViewModel.CreatedBy;
                _Context.Customers.Add(customerData);
            }
            else
            {
                customerData.UpdatedOn = DateTime.Now;
                customerData.UpdatedBy = CustomerViewModel.UpdatedBy;
                _Context.Customers.Update(customerData);
            }

            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get_Customer_Profile_Picture
        public string? GetCustomerProfilePicture(int id)
        {
            return _Context.Customers.FirstOrDefault(d => d.Id == id)?.ProfilePicture;

        }
        #endregion

        #region Remove_Customer_Profile_Picture
        public void RemoveCustomerProfilePicture(int id)
        {
            Customer? customer = _Context.Customers.FirstOrDefault(d => d.Id == id);
            if (customer != null)
            {
                customer.ProfilePicture = null;
                _Context.SaveChanges();
            }
        }
        #endregion

    }
}
