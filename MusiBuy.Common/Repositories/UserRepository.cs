
using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using static System.Net.WebRequestMethods;
namespace MusiBuy.Common.Repositories
{
    public class UserRepository : IUser
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public UserRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get user data for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<UserViewModel> GetUserList(int userId, string searchValue)
        {
            var result = (from u in _Context.AdminUsers
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                          u.LastName.ToLower().Contains(searchValue.ToLower()) ||
                          u.Username.ToLower().Contains(searchValue.ToLower()) ||
                          u.Role.RoleName.ToLower().Contains(searchValue.ToLower()) ||
                          (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.Username
                          select new UserViewModel
                          {
                              Id = u.Id,
                              Username = u.Username,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              RoleName = u.Role.RoleName,
                              IsAdminUser = u.IsAdminUser,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsCurrentAdminUser = userId == u.Id ? true : false
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
        public bool IsUserExists(int userID, string userName)
        {
            return (from x in _Context.AdminUsers where (string.IsNullOrWhiteSpace(userName) || x.Username == userName) && (userID == 0 || x.Id != userID) select x.Id).Any();
        }
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
            return (from x in _Context.AdminUsers where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) select x.Id).Any();
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public UserViewModel GetUserDetailsByID(int userID)
        {
            UserViewModel? validateUser = (from u in _Context.AdminUsers
                                           join r in _Context.Roles on u.RoleId equals r.Id
                                           where u.Id == userID
                                           select new UserViewModel
                                           {
                                               Id = u.Id,
                                               Username = u.Username,
                                               Password = u.Password,
                                               FirstName = u.FirstName,
                                               LastName = u.LastName,
                                               Mobile = u.Mobile,
                                               Email = u.Email,
                                               IsActive = u.IsActive,
                                               RoleId = u.RoleId,
                                               RoleName = r.RoleName,
                                               CountryId = u.CountryId,
                                               IsAdminUser = u.IsAdminUser,
                                               Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText
                                           }).FirstOrDefault();
            return validateUser;
        }
        #endregion

        #region Get User Details by Email
        /// <summary>
        /// Get single user by Email
        /// </summary>
        /// <returns>It returns user detail by email</returns>
        public UserViewModel GetUserDetailsEmail(string email, string userName, string mobileNo)
        {
            UserViewModel? validateUser = (from u in _Context.AdminUsers
                                           join r in _Context.Roles on u.RoleId equals r.Id
                                           where u.Email == email && u.Username == userName && u.Mobile == mobileNo
                                           select new UserViewModel
                                           {
                                               Id = u.Id,
                                               Username = u.Username,
                                               Password = u.Password,
                                               FirstName = u.FirstName,
                                               LastName = u.LastName,
                                               Mobile = u.Mobile,
                                               Email = u.Email,
                                               IsActive = u.IsActive,
                                               RoleId = u.RoleId,
                                               RoleName = u.Role != null ? u.Role.RoleName : string.Empty,
                                           }).FirstOrDefault();
            return validateUser ?? new UserViewModel();
        }

        #endregion

        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteUsers(int[] ids)
        {
            _Context.AdminUsers.RemoveRange(_Context.AdminUsers.Where(u => ids.Contains(u.Id)).AsEnumerable());
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
        public bool Save(UserViewModel userViewModel)
        {
            var userData = new AdminUser();
            if (userViewModel.Id > 0)
            {
                userData = _Context.AdminUsers.FirstOrDefault(x => x.Id == userViewModel.Id) ?? new AdminUser();
            }
            userData.FirstName = (userViewModel.FirstName ?? string.Empty).Trim();
            userData.LastName = (userViewModel.LastName ?? string.Empty).Trim();
            userData.Mobile = !(string.IsNullOrEmpty(userViewModel.Mobile)) ? (userViewModel.Mobile ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            userData.Email = (userViewModel.Email ?? string.Empty).Trim();
            userData.RoleId = userViewModel.RoleId;
            userData.Username = (userViewModel.Username ?? string.Empty).Trim();
            userData.Password = Encryption.Encrypt(userViewModel.Password ?? string.Empty);
            userData.IsActive = userViewModel.IsActive;
            userData.CountryId = userViewModel.CountryId;
            userData.IsAdminUser = userViewModel.IsAdminUser;

            if (userData.Id == 0)
            {
                userData.CreatedOn = DateTime.Now;
                _Context.AdminUsers.Add(userData);
            }
            else
            {
                userData.UpdatedOn = DateTime.Now;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get user Count for Dashboard
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public int GetUserCount()
        {
            return _Context.FrontUsers.Count(p => p.IsActive == true);
        }
        #endregion

        #region Get User List
        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns>Returns list of All User for dropdown</returns>
        public List<DropDownBindViewModel> GetUserDropDownList(bool IsActive = true)
        {
            List<DropDownBindViewModel> objPostList = _Context.AdminUsers.Where(a => a.IsActive == IsActive).Select(e => new DropDownBindViewModel { value = e.Id, name = e.FirstName + " " + e.LastName }).ToList();
            return objPostList;
        }
        #endregion


    }
}
