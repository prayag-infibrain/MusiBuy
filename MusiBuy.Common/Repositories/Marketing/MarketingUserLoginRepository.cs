using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces.Marketing;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories.Marketing
{
    public class MarketingUserLoginRepository : IMarketingUsersLogin
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _context;
        public MarketingUserLoginRepository(MusiBuyDB_Connection context)
        {
            _context = context;
        }
        #endregion

        #region Validate Login Details
        /// <summary>
        /// Validate login details
        /// </summary>
        /// <param name="objLoginViewModel"></param>
        /// <returns>If login credentials are valid it returns user details other wise returns view model </returns>
        public MarketingUserViewModel ValidateLogin(LoginViewModel objLoginViewModel)
        {
            string encryptedPassword = Encryption.Encrypt(objLoginViewModel.Password);
            MarketingUserViewModel? objUserViewModel = _context.MarketingUsers.Where(e => e.Username == objLoginViewModel.UserName && e.Password == encryptedPassword && e.IsActive).Select(e => new MarketingUserViewModel
            {
                Id = e.Id,
                Username = e.Username,
                FirstName =e.FirstName,
                LastName =e.LastName,
                Mobile = e.Mobile,
                Email = e.Email,
                UserTypeId = e.UserTypeId,
                IsActive = e.IsActive
            }).FirstOrDefault();
    
            return objUserViewModel ?? new MarketingUserViewModel();
        }

        #endregion


        #region  Validate FrontLigin
        //public UserViewModel ValidateFrontLogin(FrontLoginViewModel objLoginViewModel)
        //{
        //    string encryptedPassword = Encryption.Encrypt(objLoginViewModel.Password);
        //    //UserViewModel? objUserViewModel = _context.Clients.Where(e => e.Email == objLoginViewModel.Email && e.Password == encryptedPassword && e.IsActive).Select(e => new UserViewModel
        //    //{
        //    //    Id = e.Id,
                
        //    //    FirstName = e.FirstName,
        //    //    LastName = e.LastName,
        //    //    Mobile = e.Mobile,
        //    //    Email = e.Email,
        //    //    RoleId = e.RoleId ??0,               
        //    //    IsActive = e.IsActive
        //    //}).FirstOrDefault();

        //    return /*objUserViewModel ??*/ new UserViewModel();
        //}
        #endregion
    }
}
