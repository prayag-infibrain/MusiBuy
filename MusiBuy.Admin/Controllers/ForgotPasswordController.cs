using MusiBuy.Common.Common;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace MusiBuy.Admin.Controllers
{
    public class ForgotPasswordController : Controller
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IUser _userRepository;
        private readonly ITemplate _templateRepository;
        private readonly ICommonSetting _commonSettingRepository;
        private readonly IConfiguration _config;

        public ForgotPasswordController(IUser user, ITemplate template, ICommonSetting commonSetting, IConfiguration config)
        {
            this._userRepository = user;
            this._templateRepository = template;
            this._commonSettingRepository = commonSetting;
            this._config = config;
        }
        #endregion

        #region Index
        /// <summary>
        /// Forgot password get method
        /// </summary>
        /// <returns>Returns forgot password view</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Forgot password post method for sending email with reset password link if user is valid 
        /// </summary>
        /// <param name="forgotPasswordViewModel"></param>
        /// <returns>If email sent successfully then returns success message other wise returns forgot password view with error message </returns>
        [HttpPost]
        public async Task<ActionResult> Index(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            if (ModelState.IsValid)
            {
                var mobile = forgotPasswordViewModel.Mobile.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                var userData = _userRepository.GetUserDetailsEmail(forgotPasswordViewModel.Email, forgotPasswordViewModel.UserName, mobile);
                if (userData != null && !string.IsNullOrWhiteSpace(userData.Email))
                {
                    var template = _templateRepository.GetEmailTemplateByName(TemplateEnum.ForgotPassword.ToString());
                    if (template.TemplateContent != null)
                    {
                        var commonSetting = _commonSettingRepository.GetCommonSetting();
                        string emailBody = template.TemplateContent;
                        emailBody = PopulateEmailBody( userData, emailBody, commonSetting.SiteURL ?? string.Empty);
                        
                        bool status = GlobalCode.SendEmail(forgotPasswordViewModel.Email, template.Subject ?? string.Empty, emailBody, commonSetting);
                        if (status)
                            return RedirectToAction("Index", "Home", new { Msg = "forgot" });
                        else
                            ModelState.AddModelError(string.Empty, string.Format(Messages.Error));
                            return View(forgotPasswordViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, string.Format(Messages.Error));
                        return View(forgotPasswordViewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.NotExistsUser));
                    return View(forgotPasswordViewModel);
                }

            }
            return View(forgotPasswordViewModel);
        }
        #endregion

        #region Email body Html
        /// <summary>
        /// Get email content by replacing dynamic data from model
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <param name="emailBody"></param>
        /// <param name="siteUrl"></param>
        /// <returns>Returns email content as string by replacing dynamic data from model</returns>
        private string PopulateEmailBody(UserViewModel userViewModel, string emailBody, string siteUrl)
        {
            emailBody = emailBody.Replace("###SiteUrl###", Convert.ToString(siteUrl));
            emailBody = emailBody.Replace("###FirstName###", Convert.ToString(userViewModel.FirstName));
            emailBody = emailBody.Replace("###LastName###", Convert.ToString(userViewModel.LastName));
            emailBody = emailBody.Replace("###Year###", DateTime.UtcNow.Year.ToString());
            siteUrl = siteUrl.Contains("http") ? siteUrl : "http://" + siteUrl;
            emailBody = emailBody.Replace("###Link###", "<a href=" + siteUrl + "/ResetPassword?UID=" + Guid.NewGuid() + "-" + userViewModel.Id + ">click here</a>");
            emailBody = emailBody.Replace("###CopyLink###", siteUrl + "/ResetPassword?UID=" + Guid.NewGuid() + "-" + userViewModel.Id);

            return emailBody;
        }
        #endregion
    }
}