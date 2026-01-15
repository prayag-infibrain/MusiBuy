using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces.Marketing;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories.Marketing
{
    public class MarketingUserRepository : IMarketingUser
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public MarketingUserRepository(MusiBuyDB_Connection context)
        {
            _Context = context;
        }
        #endregion

        #region Get user data for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<MarketingUserViewModel> GetUserList(string searchValue)
        {
            var result = from u in _Context.MarketingUsers
                          where string.IsNullOrWhiteSpace(searchValue) ||
                          u.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                          u.LastName.ToLower().Contains(searchValue.ToLower()) ||
                          u.Username.ToLower().Contains(searchValue.ToLower()) ||
                          (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true)
                          orderby u.Username
                          select new MarketingUserViewModel
                          {
                              Id = u.Id,
                              Username = u.Username,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              UserTypeId = u.UserTypeId,
                              UserTypeValue = u.UserTypeId == (int)MarketingUserTypeEnum.Advertiser ? MarketingUserTypeEnum.Advertiser.ToString() : MarketingUserTypeEnum.Publishers.ToString(),
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                          };

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
            return (from x in _Context.MarketingUsers where (string.IsNullOrWhiteSpace(userName) || x.Username == userName) && (userID == 0 || x.Id != userID) select x.Id).Any();
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
            return (from x in _Context.MarketingUsers where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) select x.Id).Any();
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public MarketingUserViewModel GetUserDetailsByID(int userID)
        {
            MarketingUserViewModel? validateUser = (from u in _Context.MarketingUsers
                                           where u.Id == userID
                                           select new MarketingUserViewModel
                                           {
                                               Id = u.Id,
                                               Username = u.Username,
                                               Password = u.Password,
                                               FirstName = u.FirstName,
                                               LastName = u.LastName,
                                               Mobile = u.Mobile,
                                               Email = u.Email,
                                               IsActive = u.IsActive,
                                               UserTypeId = u.UserTypeId,
                                               UserTypeValue = u.UserTypeId == (int)MarketingUserTypeEnum.Advertiser ? MarketingUserTypeEnum.Advertiser.ToString() : MarketingUserTypeEnum.Publishers.ToString(),
                                               CountryId = u.CountryId,
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
        public MarketingUserViewModel GetUserDetailsEmail(string email, string userName, string mobileNo)
        {
            MarketingUserViewModel? validateUser = (from u in _Context.MarketingUsers
                                           where u.Email == email && u.Username == userName && u.Mobile == mobileNo
                                           select new MarketingUserViewModel
                                           {
                                               Id = u.Id,
                                               Username = u.Username,
                                               Password = u.Password,
                                               FirstName = u.FirstName,
                                               LastName = u.LastName,
                                               Mobile = u.Mobile,
                                               Email = u.Email,
                                               UserTypeId = u.UserTypeId,
                                               IsActive = u.IsActive,
                                           }).FirstOrDefault();
            return validateUser ?? new MarketingUserViewModel();
        }

        #endregion

        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteUsers(int[] ids)
        {
            _Context.MarketingUsers.RemoveRange(_Context.MarketingUsers.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MarketingUserViewModel"></param>
        /// <returns></returns>
        public bool Save(MarketingUserViewModel MarketingUserViewModel)
        {
            var userData = new MarketingUser();
            if (MarketingUserViewModel.Id > 0)
            {
                userData = _Context.MarketingUsers.FirstOrDefault(x => x.Id == MarketingUserViewModel.Id) ?? new MarketingUser();
            }
            userData.UserTypeId = MarketingUserViewModel.UserTypeId;
            userData.FirstName = (MarketingUserViewModel.FirstName ?? string.Empty).Trim();
            userData.LastName = (MarketingUserViewModel.LastName ?? string.Empty).Trim();
            userData.Email = (MarketingUserViewModel.Email ?? string.Empty).Trim();
            userData.Mobile = !string.IsNullOrEmpty(MarketingUserViewModel.Mobile) ? (MarketingUserViewModel.Mobile ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            userData.Username = (MarketingUserViewModel.Username ?? string.Empty).Trim();
            userData.Password = Encryption.Encrypt(MarketingUserViewModel.Password ?? string.Empty);
            userData.CountryId = MarketingUserViewModel.CountryId;
            userData.IsActive = MarketingUserViewModel.IsActive;

            if (userData.Id == 0)
            {
                userData.CreatedOn = DateTime.Now;
                _Context.MarketingUsers.Add(userData);
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
            return _Context.MarketingUsers.Count(p => p.IsActive == true);
        }
        #endregion

        #region Get User List
        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns>Returns list of All User for dropdown</returns>
        public List<DropDownBindViewModel> GetMarketingUserDropDownList()
        {
            List<DropDownBindViewModel> objPostList = _Context.MarketingUsers.Where(a => a.IsActive == true).Select(e => new DropDownBindViewModel { value = e.Id, name = e.FirstName + " " + e.LastName }).ToList();
            return objPostList;
        }
        #endregion

    }
}
