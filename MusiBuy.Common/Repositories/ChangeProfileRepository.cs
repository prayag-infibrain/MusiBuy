using System;
using System.Linq;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Repositories
{
    public class ChangeProfileRepository : IChangeProfile
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;

        public ChangeProfileRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get user by ID
        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <returns>ChangeProfileViewModel</returns>
        public ChangeProfileViewModel GetUserByID(int userID)
        {
            var userData = (from u in _Context.AdminUsers
                            where u.Id == userID
                            select new ChangeProfileViewModel
                            {
                                Username = u.Username,
                                UserId = u.Id,
                                FirstName = u.FirstName,
                                Lastname = u.LastName,
                                Email = u.Email,
                                CountryId = u.CountryId,
                                Phone = u.Mobile,
                            }).FirstOrDefault();
            return userData ?? new ChangeProfileViewModel();
        }
        #endregion

        #region Check if email exists
        /// <summary>
        /// Check if email exists
        /// </summary>
        /// <returns>bool</returns>
        public bool IsEmailExists(string email, int userId)
        {
            return _Context.AdminUsers.Where(u => u.Email == email && u.Id != userId).Any();
        }
        #endregion

        #region Update user profile
        /// <summary>
        /// Update user profile
        /// </summary>
        /// <returns>If profile is updated successfully it returns true other wise returns false</returns>
        public bool UpdateProfile(ChangeProfileViewModel changeProfileViewModel)
        {
            AdminUser? tbl = _Context.AdminUsers.Where(e => e.Id == changeProfileViewModel.UserId).FirstOrDefault();
            if(tbl == null)
            {
                tbl = new AdminUser();
            }            
            tbl.FirstName =!string.IsNullOrWhiteSpace(changeProfileViewModel.FirstName) ? changeProfileViewModel.FirstName.Trim() : string.Empty;
            tbl.LastName = !string.IsNullOrWhiteSpace(changeProfileViewModel.Lastname) ? changeProfileViewModel.Lastname.Trim():string.Empty;
            tbl.Email = !string.IsNullOrWhiteSpace(changeProfileViewModel.Email) ? changeProfileViewModel.Email.Trim() : string.Empty;
            tbl.Mobile = !string.IsNullOrWhiteSpace(changeProfileViewModel.Phone) ? changeProfileViewModel.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            tbl.CountryId = changeProfileViewModel.CountryId;
            tbl.UpdatedBy = changeProfileViewModel.UserId;
            tbl.UpdatedOn = DateTime.Now;
            _Context.SaveChanges();
            return true;
        }
        #endregion

        #region Get user by ID
        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <returns>ChangeProfileViewModel</returns>
        public ChangeProfileViewModel GetFrontUserByID(int userID)
        {
            //var userData = (from u in _Context.Clients
            //                where u.Id == userID
            //                select new ChangeProfileViewModel
            //                {
                               
            //                    UserId = u.Id,
            //                    FirstName = u.FirstName,
            //                    Lastname = u.LastName,
            //                    Email = u.Email,
            //                    Phone = u.Mobile,
            //                }).FirstOrDefault();
            return /*userData ??*/ new ChangeProfileViewModel();
        }
        #endregion

        #region Check if email exists
        /// <summary>
        /// Check if email exists
        /// </summary>
        /// <returns>bool</returns>
        public bool IsEmailFrontUserExists(string email, int userId)
        {
            return false; //_Context.Clients.Where(u => u.Email == email && u.Id != userId).Any();
        }
        #endregion


        #region Update user profile
        /// <summary>
        /// Update user profile
        /// </summary>
        /// <returns>If profile is updated successfully it returns true other wise returns false</returns>
        public bool UpdateProfileFrontUser(ChangeProfileViewModel changeProfileViewModel)
        {
            //Client? tbl = _Context.Clients.Where(e => e.Id == changeProfileViewModel.UserId).FirstOrDefault();
            //if (tbl == null)
            //{
            //    tbl = new Client();
            //}
            //tbl.FirstName = !string.IsNullOrWhiteSpace(changeProfileViewModel.FirstName) ? changeProfileViewModel.FirstName.Trim() : string.Empty;
            //tbl.LastName = !string.IsNullOrWhiteSpace(changeProfileViewModel.Lastname) ? changeProfileViewModel.Lastname.Trim() : string.Empty;
            //tbl.Email = !string.IsNullOrWhiteSpace(changeProfileViewModel.Email) ? changeProfileViewModel.Email.Trim() : string.Empty;
            //tbl.Mobile = !string.IsNullOrWhiteSpace(changeProfileViewModel.Phone) ? changeProfileViewModel.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            //tbl.UpdatedBy = changeProfileViewModel.UserId;
            //tbl.UpdatedOn = DateTime.Now;
            //_Context.SaveChanges();
            return true;
        }
        #endregion

    }
}
