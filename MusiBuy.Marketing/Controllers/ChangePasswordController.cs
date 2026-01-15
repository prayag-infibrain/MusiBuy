using Microsoft.AspNetCore.Mvc;
using MusiBuy.Marketing.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Models;
using MusiBuy.Common.Interfaces.Marketing;

namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class ChangePasswordController : Controller
    {
        #region Members Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IMarketingChangePassword _changePassword;

        public ChangePasswordController(IMarketingChangePassword changePassword)
        {
            this._changePassword = changePassword;
        }
        #endregion
        
        #region Index
        /// <summary>
        /// Action to render view for change password
        /// </summary>
        /// <returns>Returns change password view</returns>
        public ActionResult Index()
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
            changePasswordViewModel.UserId = CurrentUserSession.User.UserID;
            changePasswordViewModel.UserName = CurrentUserSession.User.UserName;
            ViewBag.ModuleName = "Change Password";
            return View(changePasswordViewModel);
        }

        /// <summary>
        /// Change password post method for updating password
        /// </summary>
        /// <param name="changePasswordViewModel"></param>
        /// <returns>If password updated successfully it redirects to login page with success message other wise return change password view with error message</returns>
        [HttpPost]
        public ActionResult Index(ChangePasswordViewModel changePasswordViewModel)
        {
            changePasswordViewModel.UserId = CurrentUserSession.User.UserID;
            changePasswordViewModel.UserName = CurrentUserSession.User.UserName;
            if (ModelState.IsValid)
            {
                if (changePasswordViewModel.Password != changePasswordViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, Messages.PasswordDoNotMatch);
                    return View(changePasswordViewModel);
                }
                var userPassword = _changePassword.GetOldPassword(changePasswordViewModel.UserId);
                if (userPassword != null || userPassword !="")
                {
                    if (userPassword == Encryption.Encrypt(changePasswordViewModel.Password))
                    {
                        ModelState.AddModelError(string.Empty, Messages.NewPasswordCanNotBeSame);
                        return View(changePasswordViewModel);
                    }

                    if (userPassword!= Encryption.Encrypt(changePasswordViewModel.OldPassword))
                    {
                        ModelState.AddModelError(string.Empty, Messages.OldPasswordDoNotMatch);
                        return View(changePasswordViewModel);
                    }
                }
                changePasswordViewModel.Password = Encryption.Encrypt(changePasswordViewModel.Password);
                bool isUpdated=_changePassword.UpdatePassword(changePasswordViewModel);
                if(isUpdated)
                {
                    SessionManager.ClearSession();
                    return RedirectToAction("Index", "Home", new { Msg = "changed" });
                }
                SessionManager.ClearSession();
                return RedirectToAction("Index", "Home", new { Msg = "not changed" });
            }
            else
            {
                return View(changePasswordViewModel);
            }
        }
        #endregion        
    }
}