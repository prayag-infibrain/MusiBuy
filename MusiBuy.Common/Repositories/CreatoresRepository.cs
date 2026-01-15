
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
namespace MusiBuy.Common.Repositories
{
    public class CreatoresRepository : ICreatores
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;

        public CreatoresRepository(MusiBuyDB_Connection context, IConfiguration configuration )
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
        public IQueryable<CreatoresViewModel> GetCreatoresList(int userId, string searchValue)
        {
            var result = (from u in _Context.Creatores
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                          u.LastName.ToLower().Contains(searchValue.ToLower()) ||
                          u.Email.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.FirstName
                          select new CreatoresViewModel
                          {
                              Id = u.Id,
                              FirstName = u.FirstName,
                              LastName = u.LastName,
                              Email = u.Email,
                              Phone = u.Phone,
                              Bio = u.Bio,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsRecordUsed = _Context.PostManagements.Any(p => p.CreatorId == u.Id) || _Context.EventManagements.Any(e => e.CreatorId == u.Id)
                          });

            return result;
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
            return (from x in _Context.Creatores where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) select x.Id).Any();
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public CreatoresViewModel GetCreatoresDetailsByID(int CreatorID)
        {
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.CreatorImages.Replace("\\", "/").Trim('/');

            Creatore? creatoreEntity = _Context.Creatores.FirstOrDefault(p => p.Id == CreatorID);
            string fileName = creatoreEntity.ProfilePicture;

            CreatoresViewModel? validateCreatore = (from u in _Context.Creatores
                                                    where u.Id == CreatorID
                                                    select new CreatoresViewModel
                                                    {
                                                        Id = u.Id,
                                                        FirstName = u.FirstName,
                                                        LastName = u.LastName,
                                                        Phone = u.Phone,
                                                        Email = u.Email,
                                                        Bio = u.Bio,
                                                        StrProfilePicture = fileName != null ? $"{fileReadPath}{relativePath}/{fileName}" : null,
                                                        IsActive = u.IsActive,
                                                        RoleId = u.RoleId,
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
        public CreatoresViewModel GetCreatoresDetailsEmail(string email, string userName, string mobileNo)
        {
            CreatoresViewModel? validateCreatore = (from u in _Context.Creatores
                                                   join r in _Context.Roles on u.RoleId equals r.Id
                                                   //where u.Email == email && u.Username == userName && u.Mobile == mobileNo
                                                   select new CreatoresViewModel
                                                   {
                                                       Id = u.Id,
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName,
                                                       Phone = u.Phone,
                                                       Email = u.Email,
                                                       IsActive = u.IsActive,
                                                       RoleId = u.RoleId,
                                                       RoleName = r.RoleName,
                                                   }).FirstOrDefault();
            return validateCreatore ?? new CreatoresViewModel();
        }

        #endregion

        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteCreatores(int[] ids)
        {
            _Context.Creatores.RemoveRange(_Context.Creatores.Where(u => ids.Contains(u.Id)).AsEnumerable());
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
        public bool Save(CreatoresViewModel CreatoresViewModel)
        {
            Creatore creatoreData = new Creatore();
            if (CreatoresViewModel.Id > 0)
            {
                creatoreData = _Context.Creatores.Where(a=>a.Id == CreatoresViewModel.Id).FirstOrDefault() ?? new Creatore();
            }
            creatoreData.FirstName = (CreatoresViewModel.FirstName ?? string.Empty).Trim();
            creatoreData.LastName = (CreatoresViewModel.LastName ?? string.Empty).Trim();
            creatoreData.Phone = !(string.IsNullOrEmpty(CreatoresViewModel.Phone)) ? (CreatoresViewModel.Phone ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
            creatoreData.Email = (CreatoresViewModel.Email ?? string.Empty).Trim();
            creatoreData.RoleId = CreatoresViewModel.RoleId;
            creatoreData.Bio = (CreatoresViewModel.Bio ?? string.Empty).Trim();
            creatoreData.IsActive = CreatoresViewModel.IsActive;
            creatoreData.Password = Encryption.Encrypt(CreatoresViewModel.Password ?? "Admin@123");//Pending From Web (Then Set Default Password Admin@123)
            creatoreData.ProfilePicture = CreatoresViewModel.StrProfilePicture;

            if (creatoreData.Id == 0)
            {
                creatoreData.CreatedOn = DateTime.Now;
                creatoreData.CreatedBy = CreatoresViewModel.CreatedBy;
                _Context.Creatores.Add(creatoreData);
            }
            else
            {
                creatoreData.UpdatedBy = CreatoresViewModel.UpdatedBy;
                creatoreData.UpdatedOn = DateTime.Now;
                _Context.Creatores.Update(creatoreData);
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion


        #region Get_Creatore_Profile_Picture
        public string? GetCreatoreProfilePicture(int id)
        {
            return _Context.Creatores.FirstOrDefault(d => d.Id == id)?.ProfilePicture;

        }
        #endregion

        #region Remove_Creatore_Profile_Picture
        public void RemoveCreatoreProfilePicture(int id)
        {
            Creatore? creatore = _Context.Creatores.FirstOrDefault(d => d.Id == id);
            if (creatore != null)
            {
                creatore.ProfilePicture = null;
                _Context.SaveChanges();
            }
        }
        #endregion

        #region Get Creatore Count for Dashboard
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public int GetCreatoreCount()
        {
            return _Context.FrontUsers.Count(p => p.IsActive == true && p.UserTypeId == (int)APIUserTypeEnum.Creator);
        }
        #endregion

        #region Get Creatore List
        /// <summary>
        /// Get Creatore List
        /// </summary>
        /// <returns>Returns list of role for dropdown</returns>
        public List<DropDownBindViewModel> GetCreatoreDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null)
        {
            List<DropDownBindViewModel> objRoleList = _Context.FrontUsers.Where(e => (isActive.HasValue ? e.IsActive : true) && e.UserTypeId == (int)APIUserTypeEnum.Creator).Select(e => new DropDownBindViewModel { value = e.Id, name = e.FirstName +" "+ e.LastName }).ToList();
            return objRoleList;
        }
        #endregion




        #region  API Method For Login And Registor
        #region Login Creator And Get Creator Details by Email If Login is Valis
        public (CreatoresViewModel User, string Message) ValidateCreator(string EmailId, string Password)
        {
            string encryptedPassword = Encryption.Encrypt(Password);
            var User = _Context.Creatores.Include(a => a.Role).FirstOrDefault(a => a.Email == EmailId);
            if (User == null)
                return (null, "Invalid Email Id");

            if (!User.IsActive)
                return (null, "Account is inactive");

            if (User.Password != encryptedPassword)
                return (null, "Incorrect password");


            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.CreatorImages.Replace("\\", "/").Trim('/');


            CreatoresViewModel? validateUser = new CreatoresViewModel
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Phone = User.Phone,
                Email = User.Email,
                IsActive = User.IsActive,
                RoleId = User.RoleId,
                RoleName = User.Role.EnumValue,
                StrProfilePicture = User.ProfilePicture != null ? $"{fileReadPath}{relativePath}/{User.ProfilePicture}" : null,
            };
            return (validateUser, "Sucess");
        }
        #endregion

        #region Check Creator For Forgot Passsworrd
        public (CreatoresViewModel User, string Message) ValidateCreatorForForgotPassword(string EmailId)
        {
            var User = _Context.Creatores.FirstOrDefault(a => a.Email == EmailId);
            if (User == null)
                return (null, "Invalid Email Id");

            if (!User.IsActive)
                return (null, "Account is inactive");

            CreatoresViewModel? validateUser = new CreatoresViewModel
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Phone = User.Phone,
                Email = User.Email,
                IsActive = User.IsActive,
            };
            return (validateUser, "Sucess");
        }
        #endregion

        #region Update OTP For Creator By Id
        public bool UpdateCreatorOPT(int UserId, int OTP)
        {
            Creatore CreatorData = new Creatore();
            CreatorData = _Context.Creatores.FirstOrDefault(a => a.Id == UserId);
            if (CreatorData == null)
                return false;

            CreatorData.Otp = OTP;
            CreatorData.OtpCreatedOn = DateTime.Now;

            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Verify OTP For Creator By EmailId
        public (bool Status, string Message) VeryfyOTPByEmail(string Email, int OTP)
        {
            Creatore UserData = new Creatore();
            UserData = _Context.Creatores.FirstOrDefault(a => a.Email == Email);

            if (UserData == null)
                return (false, "Invalid User");

            if (UserData.Otp == null || UserData.OtpCreatedOn == null)
                return (false, "OTP not generated");

            if (UserData.Otp != OTP)
                return (false, "Invalid OTP");

            var otpAge = DateTime.UtcNow - UserData.OtpCreatedOn.Value;
            if (otpAge.TotalMinutes > 1)
                return (false, "OTP has expired");

            return (true, "OTP verified successfully");
        }
        #endregion

        #region Update Password For User By EmailId
        public (bool Status, string Message) UpdatePasswordByEmail(string Email, string Passrowd)
        {
            Creatore UserData = new Creatore();
            UserData = _Context.Creatores.FirstOrDefault(a => a.Email == Email);

            if (UserData == null)
                return (false, "Invalid Data");

            UserData.Password = Encryption.Encrypt(Passrowd ?? string.Empty);
            UserData.UpdatedOn = DateTime.Now;

            if (_Context.SaveChanges() > 0)
                return (true, "Password Change successfully");
            else
                return (true, "Error While Change Password");
        }
        #endregion

        #endregion
    }
}
