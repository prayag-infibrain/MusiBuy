
using System.Security.Cryptography.Xml;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
namespace MusiBuy.Common.Repositories
{
    public class ClientRepository : IClient
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public ClientRepository(MusiBuyDB_Connection context)
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
        public IQueryable<ClientViewModel> GetUserList(int userId, string searchValue)
        {
            //var result = (from u in _Context.Clients
            //              where (string.IsNullOrWhiteSpace(searchValue) ||
            //              u.FirstName.ToLower().Contains(searchValue.ToLower()) ||
            //              u.LastName.ToLower().Contains(searchValue.ToLower()) ||
            //              u.Email.ToLower().Contains(searchValue.ToLower()) ||

            //              (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(searchValue.ToLower()) ||
            //              (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
            //              orderby u.FirstName
            //              select new ClientViewModel
            //              {
            //                  Id = u.Id,
            //                  FirstName = u.FirstName,
            //                  Email = u.Email,
            //                  Mobile = u.Mobile,
            //                  LastName = u.LastName,
            //                  IsActive = u.IsActive,
            //                  Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
            //                  IsCurrentAdminUser = userId == u.Id ? true : false
            //              });

            return null;// result;
        }
        #endregion

        #region Check User Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        public bool IsUserExists(int userID, string firstname)
        {
            return false; //(from x in _Context.Clients where (string.IsNullOrWhiteSpace(firstname) || x.FirstName == firstname) && (userID == 0 || x.Id != userID) select x.Id).Any();
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
            return false;//(from x in _Context.Clients where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) select x.Id).Any();
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public ClientViewModel GetUserDetailsByID(int userID)
        {
            //ClientViewModel? validateUser = (from u in _Context.Clients
            //                                 where u.Id == userID
            //                                 select new ClientViewModel
            //                                 {
            //                                     Id = u.Id,
            //                                     Password = u.Password,
            //                                     FirstName = u.FirstName,
            //                                     LastName = u.LastName,
            //                                     Mobile = u.Mobile,
            //                                     Email = u.Email,
            //                                     IsActive = u.IsActive,
            //                                     Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
            //                                     RoleId = u.RoleId,
            //                                     RoleName  = u.Role.RoleName,
            //                                 }).FirstOrDefault();
            return null;  //validateUser;
        }
        #endregion

        #region Get User Details by Email
        /// <summary>
        /// Get single user by Email
        /// </summary>
        /// <returns>It returns user detail by email</returns>
        public ClientViewModel GetUserDetailsEmail(string email)
        {
            //ClientViewModel? validateUser = (from u in _Context.Clients
            //                                 where u.Email == email
            //                                 select new ClientViewModel
            //                                 {
            //                                     Id = u.Id,
            //                                     Password = u.Password,
            //                                     FirstName = u.FirstName,
            //                                     LastName = u.LastName,
            //                                     Mobile = u.Mobile,
            //                                     Email = u.Email,
            //                                     IsActive = u.IsActive,
            //                                 }).FirstOrDefault();
            return /*validateUser ??*/ new ClientViewModel();
        }

        #endregion

        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteUsers(int[] ids)
        {
            //_Context.Clients.RemoveRange(_Context.Clients.Where(u => ids.Contains(u.Id)).AsEnumerable());
            //if (_Context.SaveChanges() > 0)
            //    return true;
            //else
            return false;
        }
        #endregion        

        #region Save User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        public bool Save(ClientViewModel client)
        {
            //var userData = new Client();
            //if (client.Id > 0)
            //{
            //    userData = _Context.Clients.FirstOrDefault(x => x.Id == client.Id) ?? new Client();
            //}
            //userData.FirstName = (client.FirstName ?? string.Empty).Trim();
            //userData.LastName = (client.LastName ?? string.Empty).Trim();
            //userData.Mobile = !(string.IsNullOrEmpty(client.Mobile)) ? (client.Mobile ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            //userData.Email = (client.Email ?? string.Empty).Trim();
            //userData.Password = Encryption.Encrypt(client.Password ?? string.Empty);
            //userData.IsActive = client.IsActive;
            //userData.RoleId = client.RoleId;
            //if (userData.Id == 0)
            //{
            //    userData.CreatedOn = DateTime.Now;
            //    _Context.Clients.Add(userData);
            //}
            //else
            //{
            //    userData.UpdatedOn = DateTime.Now;
            //}
            //if (_Context.SaveChanges() > 0)
            //    return true;
            //else
                return false;
        }
        #endregion

        #region MyRegion

        public List<DropDownBindViewModel> GetFrontRoles ()
        {
            //var lst = (from x in _Context.FrontRoles
            //           select new DropDownBindViewModel { name = x.RoleName,
            //           value = x.Id
            //           }
            //           ).ToList();
            return null;//lst;
        }



        #endregion


    }
}
