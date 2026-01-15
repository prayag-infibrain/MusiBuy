using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class FrontUserRepository : IFrontUser
{
    private readonly IConfiguration _configuration;
    private readonly MusiBuyDB_Connection _Context;

    public FrontUserRepository(IConfiguration configuration, MusiBuyDB_Connection context)
    {
        _configuration = configuration;
        _Context = context;
    }

    #region API (Login, Registor And Forgot Password)

    #region Login User And Get User Details by Email If Login is Valis
    public (FrontUserViewModel User, string Message) ValidateUser(string EmailId, string Password)
    {
        string encryptedPassword = Encryption.Encrypt(Password);
        var User = _Context.FrontUsers.Include(a => a.Role).FirstOrDefault(a => a.Email == EmailId);
        if (User == null)
            return (null, "Account does not exist");

        if (User.Password != encryptedPassword)
            return (null, "Incorrect password");

        FrontUserViewModel? validateUser = new FrontUserViewModel
        {
            Id = User.Id,
            Username = User.Username,
            FirstName = User.FirstName,
            LastName = User.LastName,
            Mobile = User.Mobile,
            Email = User.Email,
            UserTypeId = User.UserTypeId,
            RoleId = User.RoleId ?? 0,
            RoleName = User.Role == null ? string.Empty : User.Role.EnumValue,
            IsActive = User.IsActive,
        };
        return (validateUser, "Login successful");
    }
    #endregion

    #region Check User For Forgot Passsworrd
    public (FrontUserViewModel User, string Message) ValidateUserForForgotPassword(string EmailId)
    {
        var User = _Context.FrontUsers.FirstOrDefault(a => a.Email == EmailId);
        if (User == null)
            return (null, "Invalid Email Id");

        if (!User.IsActive)
            return (null, "Account is inactive");

        FrontUserViewModel? validateUser = new FrontUserViewModel
        {
            Id = User.Id,
            Username = User.Username,
            FirstName = User.FirstName,
            LastName = User.LastName,
            Mobile = User.Mobile,
            Email = User.Email,
            IsActive = User.IsActive,
        };
        return (validateUser, "Sucess");
    }
    #endregion

    #region Update OTP For User By Id
    public bool UpdateUserOPT(int UserId, int OTP)
    {
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Id == UserId);
        if (UserData == null)
            return false;

        UserData.Otp = OTP;
        UserData.OtpCreatedOn = DateTime.Now;

        if (_Context.SaveChanges() > 0)
            return true;
        else
            return false;
    }
    #endregion

    #region Verify OTP For User By EmailId
    public (bool Status, string Message) VeryfyOTPByEmail(string Email, int OTP)
    {
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Email == Email);

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
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Email == Email);

        if (UserData == null)
            return (false, "Incorrect password");

        UserData.Password = Encryption.Encrypt(Passrowd ?? string.Empty);
        UserData.UpdatedOn = DateTime.Now;

        if (_Context.SaveChanges() > 0)
            return (true, "Password Change successfully");
        else
            return (true, "Error While Change Password");
    }
    #endregion

    #region Change Password For User By UserId
    public (bool Status, string Message) ChangePasswordByUserId(int UserId, string OldPassword, string NewPassrowd)
    {
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Id == UserId);

        if (UserData == null)
            return (false, "Invalid User");

        if (UserData.Password != Encryption.Encrypt(OldPassword))
            return (false, "Incorrect old password.");

        UserData.Password = Encryption.Encrypt(NewPassrowd ?? string.Empty);
        UserData.UpdatedOn = DateTime.Now;

        if (_Context.SaveChanges() > 0)
            return (true, "Password Change successfully");
        else
            return (true, "Error While Change Password");
    }
    #endregion

    #region Active Account By Link By User
    public bool ActiveUserById(int UserId)
    {
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Id == UserId);

        if (UserData == null)
            return false;

        UserData.IsActive = true;
        UserData.UpdatedOn = DateTime.Now;

        if (_Context.SaveChanges() > 0)
            return true;
        else
            return true;
    }
    #endregion

    #region Check User Exists 
    public bool IsUserExists(int userID, string userName)
    {
        return (from x in _Context.FrontUsers where (string.IsNullOrWhiteSpace(userName) || x.Username == userName) && (userID == 0 || x.Id != userID) select x.Id).Any();
    }
    #endregion

    #region Check Email Exists
    public bool IsEmailExists(int id, string email)
    {
        return (from x in _Context.FrontUsers where (string.IsNullOrWhiteSpace(email) || x.Email == email) && (id == 0 || x.Id != id) && (x.IsActive == true) select x.Id).Any();
    }
    #endregion

    #region Get Detail By Id
    public FrontUserViewModel GetFrontUserDetailByEmail(string Email)
    {
        string fileReadPath = _configuration.GetSection("FilePath:FileReadPath").Value;
        string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');

        var User = _Context.FrontUsers.Include(a => a.Role).FirstOrDefault(a => a.Email == Email);
        if (User == null)
            return null;

        string fileName = User.Image;

        FrontUserViewModel? FrontUser = new FrontUserViewModel
        {
            Id = User.Id,
            FirstName = User.FirstName,
            LastName = User.LastName,
            Mobile = User.Mobile,
            Email = User.Email,
            Bio = User.Bio,
            StrImage = fileName != null ? $"{fileReadPath}{relativePath}/{fileName}" : null,
            ImageName = User.Image,
            UserTypeId = User.UserTypeId,
            RoleId = User.RoleId,
            RoleName = User.UserTypeId == 2 ? (User.Role != null ? User.Role.EnumValue : string.Empty) : "User",
            IsActive = User.IsActive,
            Password = Encryption.Decrypt(User.Password ?? string.Empty),
            Active = User.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
            ////Followers = "0",
            ////Followings = "0",
            ////TokenErned = "0",
        };
        return FrontUser;
    }
    #endregion

    #region Delete user Account by UserId And User TypeId
    public (bool Status, string Message) DeleteAccountByUserId(int UserId, int UserTypeId)
    {
        FrontUser UserData = new FrontUser();
        UserData = _Context.FrontUsers.FirstOrDefault(a => a.Id == UserId && a.UserTypeId == UserTypeId);

        if (UserData == null)
            return (false, "Account does not exist");

        UserData.IsActive = false;
        UserData.UpdatedOn = DateTime.Now;

        if (_Context.SaveChanges() > 0)
            return (true, "Account Delete successfully");
        else
            return (true, "Error While Deleting Account");
    }
    #endregion
    #endregion


    #region Save User
    public bool Save(FrontUserViewModel frontUser)
    {
        var userData = new FrontUser();
        string Image = "";
        if (frontUser.Id > 0)
        {
            userData = _Context.FrontUsers.FirstOrDefault(x => x.Id == frontUser.Id) ?? new FrontUser();
            userData.Password = Encryption.Decrypt(userData.Password);
        }
        userData.FirstName = (frontUser.FirstName ?? string.Empty).Trim();
        userData.LastName = (frontUser.LastName ?? string.Empty).Trim();
        userData.Mobile = !(string.IsNullOrEmpty(frontUser.Mobile)) ? (frontUser.Mobile ?? string.Empty).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : string.Empty;
        userData.Email = (frontUser.Email ?? string.Empty).Trim();
        userData.Username = frontUser.Username;
        userData.Bio = frontUser.Bio;
        userData.Password = Encryption.Encrypt(frontUser.Password ?? userData.Password ?? string.Empty);
        userData.IsActive = frontUser.IsActive;
        userData.UserTypeId = frontUser.UserTypeId;
        userData.RoleId = frontUser.RoleId == 0 ? null : frontUser.RoleId;
        userData.CountryId = frontUser.CountryId;
        userData.Image = frontUser.StrImage;


        if (userData.Id == 0)
        {
            userData.CreatedBy = 1;
            userData.CreatedOn = DateTime.Now;
            _Context.FrontUsers.Add(userData);
        }
        else
            userData.UpdatedOn = DateTime.Now;

        bool isSave = false;
        if (_Context.SaveChanges() > 0)
        {
            _Context.UserPrefrences.RemoveRange(_Context.UserPrefrences.Where(u => u.FrontUserId == userData.Id).AsEnumerable());
            isSave = true;
            if (frontUser.UserPrefrence.Count > 0)
            {
                _Context.SaveChanges();
                foreach (int item in frontUser.UserPrefrence)
                {
                    var UserPrefrenceData = new UserPrefrence();
                    UserPrefrenceData.FrontUserId = userData.Id;
                    UserPrefrenceData.PrefrenceId = item;
                    UserPrefrenceData.CreatedBy = userData.CreatedBy;
                    UserPrefrenceData.CreatedOn = DateTime.Now;
                    _Context.UserPrefrences.Add(UserPrefrenceData);
                }
                if (_Context.SaveChanges() > 0)
                    isSave = true;
                else
                    isSave = false;
            }
        }

        if (isSave)
            return true;
        else
            return false;
    }
    #endregion


    #region Web Mthod's
    public IQueryable<FrontUserViewModel> GetFrontUserList(int userId, string searchValue)
    {

        var result = _Context.FrontUsers.Include(a => a.Role).Where(x =>
        string.IsNullOrWhiteSpace(searchValue) ||
        x.FirstName.ToLower().Contains(searchValue.ToLower()) ||
        x.LastName.ToLower().Contains(searchValue.ToLower()) ||
        x.Email.ToLower().Contains(searchValue.ToLower()) ||
        x.Mobile.ToLower().Contains(searchValue.ToLower()) ||
        (!string.IsNullOrWhiteSpace(searchValue) ? (x.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
            .OrderByDescending(x => x.CreatedOn)
            .Select(u => new FrontUserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Mobile = u.Mobile,
                RoleName = u.Role == null ? "" : u.Role.EnumValue,
                Bio = u.Bio,
                IsActive = u.IsActive,
                Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
            });


        //var result = (from u in _Context.FrontUsers
        //              join r in _Context.Roles on u.RoleId equals r.Id
        //              where (string.IsNullOrWhiteSpace(searchValue) ||
        //              u.FirstName.ToLower().Contains(searchValue.ToLower()) ||
        //              u.LastName.ToLower().Contains(searchValue.ToLower()) ||
        //              r.RoleName.ToLower().Contains(searchValue.ToLower()) ||
        //              u.Email.ToLower().Contains(searchValue.ToLower()) ||
        //              u.Mobile.ToLower().Contains(searchValue.ToLower()) ||
        //              (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
        //              orderby u.FirstName
        //              select new FrontUserViewModel
        //              {
        //                  Id = u.Id,
        //                  FirstName = u.FirstName,
        //                  LastName = u.LastName,
        //                  Email = u.Email,
        //                  Mobile = u.Mobile,
        //                  RoleName = r.RoleName,
        //                  Bio = u.Bio,
        //                  IsActive = u.IsActive,
        //                  Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
        //              });

        return result;
    }

    #region Get Detail By Id
    public FrontUserViewModel GetFrontUserDetailById(int Id)
    {
        var FollowingList = _Context.FollowersManagements.ToList();
        string fileReadPath = _configuration.GetSection("FilePath:FileReadPath").Value;
        string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');

        var User = _Context.FrontUsers.Include(a => a.Role).FirstOrDefault(a => a.Id == Id);
        if (User == null)
            return null;

        string fileName = User.Image;

        FrontUserViewModel? FrontUser = new FrontUserViewModel
        {
            Id = User.Id,
            FirstName = User.FirstName,
            LastName = User.LastName,
            CountryId = User.CountryId ?? 0,
            Mobile = User.Mobile,
            Email = User.Email,
            Bio = User.Bio,
            StrImage = fileName != null ? $"{fileReadPath}{relativePath}/{fileName}" : null,
            ImageName = User.Image,
            UserTypeId = User.UserTypeId,
            RoleId = User.RoleId,
            RoleName = User.UserTypeId == 2 ? (User.Role != null ? User.Role.EnumValue : string.Empty) : "User",
            IsActive = User.IsActive,
            Password = Encryption.Decrypt(User.Password ?? string.Empty),
            Active = User.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
            UserPrefrence = _Context.UserPrefrences.Where(a => a.FrontUserId == User.Id).Select(a => a.PrefrenceId).ToList(),
            Followers = FollowingList.Where(a => a.FollowIngId == Id).Count(),
            Followings = FollowingList.Where(a => a.UserId == Id).Count(),
            TokenErned = 0,
        };
        return FrontUser;
    }
    #endregion

    #region Delete multiple Users
    public bool DeleteCreatores(int[] ids)
    {
        _Context.FrontUsers.RemoveRange(_Context.FrontUsers.Where(u => ids.Contains(u.Id)).AsEnumerable());
        if (_Context.SaveChanges() > 0)
            return true;
        else return false;
    }
    #endregion


    #region Get User Profile Picture
    public string? GetUserProfilePicture(int id)
    {
        return _Context.FrontUsers.FirstOrDefault(d => d.Id == id)?.Image;

    }
    #endregion

    #region Remove User Profile Picture
    public void RemoveUserProfilePicture(int id)
    {
        FrontUser? FrontUser = _Context.FrontUsers.FirstOrDefault(d => d.Id == id);
        if (FrontUser != null)
        {
            FrontUser.Image = null;
            _Context.SaveChanges();
        }
    }
    #endregion


    #endregion

}


