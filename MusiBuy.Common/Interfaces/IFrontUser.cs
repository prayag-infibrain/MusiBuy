using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IFrontUser
    {
        #region API (Login, Registor And Forgot Password)
        (FrontUserViewModel User, string Message) ValidateUser(string EmailId, string Password);
        (FrontUserViewModel User, string Message) ValidateUserForForgotPassword(string EmailId);
        bool UpdateUserOPT(int UserId, int OTP);
        (bool Status, string Message) VeryfyOTPByEmail(string Email, int OTP);
        (bool Status, string Message) UpdatePasswordByEmail(string Email, string Passrowd);
        (bool Status, string Message) ChangePasswordByUserId(int UserId, string OldPassword, string NewPassrowd);
        bool IsUserExists(int userID, string userName);
        bool IsEmailExists(int id, string email);
        FrontUserViewModel GetFrontUserDetailByEmail(string Email);
        bool ActiveUserById(int UserId);
        (bool Status, string Message) DeleteAccountByUserId(int UserId, int UserTypeId);
        #endregion


        bool Save(FrontUserViewModel frontUser);


        #region Web Mthod's
        IQueryable<FrontUserViewModel> GetFrontUserList(int userId, string searchValue);
        FrontUserViewModel GetFrontUserDetailById(int Id);
        bool DeleteCreatores(int[] ids);
        string? GetUserProfilePicture(int id);
        void RemoveUserProfilePicture(int id);
        #endregion
    }
}
