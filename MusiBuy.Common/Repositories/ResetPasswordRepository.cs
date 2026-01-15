using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Interfaces.Marketing;

namespace MusiBuy.Common.Repositories
{
    public class ResetPasswordRepository : IResetPassword
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public ResetPasswordRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Update User Password
        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userID"></param>
        /// <returns>If password is updated successfully it returns true other wise false.</returns>
        public bool ResetUserPassword(string password, int userID)
        {
            AdminUser? user = _Context.AdminUsers.FirstOrDefault(model => model.Id == userID);
            if (user != null && user.Id>0)
            {
                user.Password = Encryption.Encrypt(password);
               if( _Context.SaveChanges()>0)
                    return true;
               else return false;
            }
            return false;
        }
        #endregion


        #region Update User Password
        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userID"></param>
        /// <returns>If password is updated successfully it returns true other wise false.</returns>
        public bool ResetFrontUserPassword(string password, int userID)
        {
            //Client? user = _Context.Clients.FirstOrDefault(model => model.Id == userID);
            //if (user != null && user.Id > 0)
            //{
            //    user.Password = Encryption.Encrypt(password);
            //    if (_Context.SaveChanges() > 0)
            //        return true;
            //    else return false;
            //}
            return false;
        }
        #endregion
    }
}
