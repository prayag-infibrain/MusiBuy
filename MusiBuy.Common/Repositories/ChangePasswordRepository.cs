using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Repositories
{

    public class ChangePasswordRepository : IChangePassword
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public ChangePasswordRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Update User Password
        /// <summary>
        /// Method to change password
        /// </summary>
        /// <param name="objChangePasswordViewModel"></param>
        /// <returns>If password updated successfully it returns true other wise returns false</returns>
        public bool UpdatePassword(ChangePasswordViewModel objChangePasswordViewModel)
        {
            AdminUser? objTblUser = _Context.AdminUsers.FirstOrDefault(model=> model.Id == objChangePasswordViewModel.UserId);
            if (objTblUser != null)
            {
                objTblUser.Password = objChangePasswordViewModel.Password;
                _Context.SaveChanges();
                return true;
            }
            return false;

        }
        #endregion

        #region Get Old Password
        /// <summary>
        /// Method to get old password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>It returns the oldpassword of the admin user if exist other wise it returns empty string.</returns>
        public string GetOldPassword(int userId)
        {
            return _Context.AdminUsers != null
           ? _Context.AdminUsers.Find(userId)?.Password ?? string.Empty
           : string.Empty;
        }
        #endregion

        #region Get Old Password
        /// <summary>
        /// Method to get old password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>It returns the oldpassword of the admin user if exist other wise it returns empty string.</returns>
        public string GetOldPasswordFrontuser(int userId)
        {
           // return _Context.Clients != null
           //? _Context.Clients.Find(userId)?.Password ?? string.Empty
           //: string.Empty;

            return null;
        }
        #endregion

        #region Update User Password
        /// <summary>
        /// Method to change password
        /// </summary>
        /// <param name="objChangePasswordViewModel"></param>
        /// <returns>If password updated successfully it returns true other wise returns false</returns>
        public bool UpdatePasswordFrontUser(ChangePasswordViewModel objChangePasswordViewModel)
        {
            //Client? objTblUser = _Context.Clients.FirstOrDefault(model => model.Id == objChangePasswordViewModel.UserId);
            //if (objTblUser != null)
            //{
            //    objTblUser.Password = objChangePasswordViewModel.Password;
            //    _Context.SaveChanges();
            //    return true;
            //}
            return false;

        }
        #endregion

    }
}
