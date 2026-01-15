using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.Repositories;
using MusiBuy.Common.ResponseModels;
using System;
using System.Linq;
using System.Reflection;

namespace MusiBuy.API.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Member Declaration
        private readonly IAuthenticate _AuthenticationRepository;
        private readonly IFrontUser _User;
        private readonly IConfiguration _config;
        private readonly ITemplate _templateRepository;
        private readonly ICommonSetting _commonSettingRepository;
        private readonly IContentManagement _content;

        private readonly IDropdown _dropdown;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticate"></param>
        public AuthenticationController(IAuthenticate authenticate, IConfiguration configuration, IFrontUser User, ITemplate templateRepository, ICommonSetting commonSettingRepository, IDropdown dorpdown, IContentManagement content)
        {
            this._AuthenticationRepository = authenticate;
            this._User = User;
            this._config = configuration;
            _templateRepository = templateRepository;
            _commonSettingRepository = commonSettingRepository;
            _dropdown = dorpdown;
            _content = content;
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        [EndpointSummary("Login")]
        [EndpointDescription("Validate Email & password and return user details with token")]
        [EndpointName("Login")]
        public IActionResult Login(LoginRequests loginRequests)
        {
            if (string.IsNullOrWhiteSpace(loginRequests.EmailId) || string.IsNullOrWhiteSpace(loginRequests.Password))
            {
                return Ok(new ApiResponse(false, "EmailId and password are required", null));
            }

            var (User, Msg) = _User.ValidateUser(loginRequests.EmailId, loginRequests.Password);
            if (User == null)
            {
                return Ok(new ApiResponse(false, Msg, null));
            }
            if (User.IsActive == false)
                return Ok(new ApiResponse(false, "Please Verify Your Account", null));
            //Image-Pending
            //if (!string.IsNullOrWhiteSpace(user.Image))
            //{
            //    var readPath = _config.GetValue<string>("FilePath:FileReadPath");
            //    user.Image = readPath + GlobalCode.FrontUserFileImage + user.Image;
            //}
            var Token = _AuthenticationRepository.AuthenticateByEmail(loginRequests.EmailId, loginRequests.Password);
            string encryptedUserId = Encryption.Encrypt(User.Id.ToString());
            var response = new
            {
                UserId = User.Id,
                encryptedUserId = encryptedUserId,
                UserName = User.Username,
                FirstName = User.FirstName,
                LastName = User.LastName,
                MobileNumber = User.Mobile,
                Email = User.Email,
                UserType = User.UserTypeId,
                RoleId = User.RoleId,
                RoleName = User.RoleName,
                IsActive = User.IsActive,
                ImageUrl = "",
                Token = Token,
            };
            return Ok(new ApiResponse(true, Msg, response));
        }
        #endregion

        #region Forgot Password
        [HttpPost("ForgotPassword")]
        [EndpointSummary("ForgotPassword")]
        [EndpointDescription("Validate Email And Send OTP in Email")]
        [EndpointName("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            if (string.IsNullOrWhiteSpace(forgotPassword.EmailId))
            {
                return Ok(new ApiResponse(false, "Enter Email Address", null));
            }

            var (User, Msg) = _User.ValidateUserForForgotPassword(forgotPassword.EmailId);
            int OTP = new Random().Next(100000, 999999);
            if (User == null)
            {
                return Ok(new ApiResponse(false, Msg, null));
            }
            else
            {
                if (!_User.UpdateUserOPT(User.Id, OTP))
                {
                    return Ok(new ApiResponse(false, "Error While Send OTP", null));
                }
            }
            var template = _templateRepository.GetEmailTemplateByName(TemplateEnum.MForgotPassword.ToString());
            if (template.TemplateContent != null)
            {
                var commonSetting = _commonSettingRepository.GetCommonSetting();
                string emailBody = template.TemplateContent;
                emailBody = PopulateEmailBody(User, emailBody, commonSetting.SiteURL ?? string.Empty, OTP);

                bool status = GlobalCode.SendEmail(forgotPassword.EmailId, template.Subject ?? string.Empty, emailBody, commonSetting);
                if (status)
                    return Ok(new ApiResponse(true, "OTP has been sent to your registered email address.", null));
                else
                    return Ok(new ApiResponse(false, "Error While Send Mail", null));
            }
            else
                return Ok(new ApiResponse(false, "Email Template Not Found", null));
        }
        private string PopulateEmailBody(FrontUserViewModel userViewModel, string emailBody, string siteUrl, int OTP)
        {
            emailBody = emailBody.Replace("###SiteUrl###", Convert.ToString(siteUrl));
            emailBody = emailBody.Replace("###FirstName###", Convert.ToString(userViewModel.FirstName));
            emailBody = emailBody.Replace("###LastName###", Convert.ToString(userViewModel.LastName));
            emailBody = emailBody.Replace("###OTP###", Convert.ToString(OTP));
            emailBody = emailBody.Replace("###Year###", DateTime.UtcNow.Year.ToString());
            //siteUrl = siteUrl.Contains("http") ? siteUrl : "http://" + siteUrl;
            //emailBody = emailBody.Replace("###Link###", "<a href=" + siteUrl + "/ResetPassword?UID=" + Guid.NewGuid() + "-" + userViewModel.Id + ">click here</a>");
            //emailBody = emailBody.Replace("###CopyLink###", siteUrl + "/ResetPassword?UID=" + Guid.NewGuid() + "-" + userViewModel.Id);

            return emailBody;
        }
        #endregion

        #region Verify OTP
        [HttpPost("VeryfyOTP")]
        [EndpointSummary("VeryfyOTP")]
        [EndpointDescription("Validate OTP With Email")]
        [EndpointName("VeryfyOTP")]
        public IActionResult VeryfyOTP(OTPVerifaction oTPVerifaction)
        {
            if (string.IsNullOrWhiteSpace(oTPVerifaction.EmailId))
            {
                return Ok(new ApiResponse(false, "Enter Email Address", null));
            }
            if (oTPVerifaction.OTP == 0)
            {
                return Ok(new ApiResponse(false, "Enter OTP", null));
            }
            var (Status, Msg) = _User.VeryfyOTPByEmail(oTPVerifaction.EmailId, oTPVerifaction.OTP);
            return Ok(new ApiResponse(Status, Msg, null));
        }
        #endregion

        #region Update Password
        [HttpPost("UpdatePassword")]
        [EndpointSummary("UpdatePassword")]
        [EndpointDescription("Update Password With Emailid")]
        [EndpointName("UpdatePassword")]
        public IActionResult UpdatePassword(UpdatePassword updatePassword)
        {
            if (string.IsNullOrWhiteSpace(updatePassword.EmailId))
            {
                return Ok(new ApiResponse(false, "Enter Email Address", null));
            }
            if (string.IsNullOrWhiteSpace(updatePassword.password))
            {
                return Ok(new ApiResponse(false, "Enter Password", null));
            }
            var (Status, Msg) = _User.UpdatePasswordByEmail(updatePassword.EmailId, updatePassword.password);
            return Ok(new ApiResponse(Status, Msg, null));
        }
        #endregion

        #region Register User
        [HttpPost("UserRegistration")]
        [EndpointSummary("UserRegistration")]
        [EndpointDescription("User Registration")]
        [EndpointName("UserRegistration")]
        public IActionResult UserRegistration(UserRegistrationModel userViewModel)
        {
            //if (_User.IsUserExists(0, (userViewModel.Username ?? string.Empty).Trim()))
            //    return Ok(new ApiResponse(false, "Username Already Exist", null));


            if (userViewModel.UserTypeId == (int)APIUserTypeEnum.Creator)
            {
                if (userViewModel.RoleId == 0)
                    return Ok(new ApiResponse(false, "Invalid Role", null));
            }
            else if (userViewModel.UserTypeId == (int)APIUserTypeEnum.User)
                userViewModel.RoleId = 0;
            else
                return Ok(new ApiResponse(false, "Invalid User Type Id", null));

            if (_User.IsEmailExists(0, (userViewModel.Email ?? string.Empty).Trim()))
                return Ok(new ApiResponse(false, "Email Address Already Exist", null));



            FrontUserViewModel frontUserViewModel = new FrontUserViewModel();
            frontUserViewModel.UserTypeId = userViewModel.UserTypeId;
            frontUserViewModel.RoleId = userViewModel.RoleId;
            frontUserViewModel.FirstName = userViewModel.FirstName;
            frontUserViewModel.LastName = userViewModel.LastName;
            frontUserViewModel.Email = userViewModel.Email;
            frontUserViewModel.Mobile = userViewModel.Mobile;
            frontUserViewModel.CountryId = userViewModel.CountryId;
            frontUserViewModel.Password = userViewModel.Password;
            frontUserViewModel.UserPrefrence = userViewModel.UserPrefrence;
            frontUserViewModel.IsActive = false;
            frontUserViewModel.Username = "";

            if (_User.Save(frontUserViewModel))
            {
                var userData = _User.GetFrontUserDetailByEmail(userViewModel.Email);
                if (userData == null)
                    return Ok(new ApiResponse(true, "Invalid user", null));
                if (userData != null && !string.IsNullOrWhiteSpace(userData.Email))
                {
                    var template = _templateRepository.GetEmailTemplateByName(TemplateEnum.VerifyUserAccount.ToString());
                    if (template.TemplateContent != null)
                    {
                        var commonSetting = _commonSettingRepository.GetCommonSetting();
                        string emailBody = template.TemplateContent;
                        emailBody = PopulateEmailBody(userData, emailBody, commonSetting.SiteURL ?? string.Empty);

                        bool status = GlobalCode.SendEmail(userViewModel.Email, template.Subject ?? string.Empty, emailBody, commonSetting);
                        if (status)
                            return Ok(new ApiResponse(true, "Mail sent to the registered email account for authentication.", null));
                        else
                            return Ok(new ApiResponse(false, "Invalid Email", null));
                    }
                    else
                        return Ok(new ApiResponse(false, "Email Content Not Found", null));
                }
                else
                    return Ok(new ApiResponse(false, "Invalid Email", null));

            }
            else
            {
                return Ok(new ApiResponse(true, "Error While Registration.", null));
            }


            //if (_User.Save(frontUserViewModel))
            //    return Ok(new ApiResponse(true, "Registration Successful", null));
            //else
            //    return Ok(new ApiResponse(false, "Error While Save Record", null));
        }


        #region Email body Html For Veryfy User Email
        /// <summary>
        /// Get email content by replacing dynamic data from model
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <param name="emailBody"></param>
        /// <param name="siteUrl"></param>
        /// <returns>Returns email content as string by replacing dynamic data from model</returns>
        private string PopulateEmailBody(FrontUserViewModel userViewModel, string emailBody, string siteUrl)
        {
            emailBody = emailBody.Replace("###SiteUrl###", Convert.ToString(siteUrl));
            emailBody = emailBody.Replace("###FirstName###", Convert.ToString(userViewModel.FirstName));
            emailBody = emailBody.Replace("###LastName###", Convert.ToString(userViewModel.LastName));
            emailBody = emailBody.Replace("###Year###", DateTime.UtcNow.Year.ToString());
            siteUrl = siteUrl.Contains("http") ? siteUrl : "http://" + siteUrl;
            emailBody = emailBody.Replace("###Link###", "<a href=" + siteUrl + "/Userverification?UID=" + Guid.NewGuid() + "-" + userViewModel.Id + ">click here</a>");
            emailBody = emailBody.Replace("###CopyLink###", siteUrl + "/Userverification?UID=" + Guid.NewGuid() + "-" + userViewModel.Id);

            return emailBody;
        }
        #endregion
        #endregion

        #region Update Password
        [HttpPost("DeleteUserAccount")]
        [EndpointSummary("DeleteUserAccount")]
        [EndpointDescription("Delete user Account By UserId And UserTypeId")]
        [EndpointName("DeleteUserAccount")]
        public IActionResult DeleteUserAccount(GetByIdAndType getByIdAndType)
        {
            if (getByIdAndType.Id == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User", null));
            }
            if (getByIdAndType.TypeId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User Type", null));
            }
            var (Status, Msg) = _User.DeleteAccountByUserId(getByIdAndType.Id, getByIdAndType.TypeId);
            return Ok(new ApiResponse(Status, Msg, null));
        }
        #endregion
    }
}
