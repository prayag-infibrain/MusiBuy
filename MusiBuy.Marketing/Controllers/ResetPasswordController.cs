using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Interfaces.Marketing;

namespace MusiBuy.Marketing.Controllers
{
    public class ResetPasswordController : Controller
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IResetPassword _resetPasswordRepository;
        private readonly IMarketingUser _userRepository;

        public ResetPasswordController(IResetPassword resetPassword, IMarketingUser user)
        {
            this._resetPasswordRepository = resetPassword;
            this._userRepository = user;
        }
        #endregion

        #region Index
        /// <summary>
        /// Reset password get method
        /// </summary>
        /// <returns>Returns reset password view</returns>        
        public ActionResult Index()
        {
            Microsoft.Extensions.Primitives.StringValues queryStringVal;
            HttpContext.Request.Query.TryGetValue("UID", out queryStringVal);
            if (!string.IsNullOrWhiteSpace(queryStringVal) && queryStringVal!="")
            {
                string? userDetails = queryStringVal;
                if (!string.IsNullOrWhiteSpace(userDetails) && userDetails!="")
                {
                    int userId;
                    if (int.TryParse(userDetails.Substring(userDetails.LastIndexOf("-") + 1), out userId))
                    {
                        var objValidateUser = _userRepository.GetUserDetailsByID(userId);
                        if (objValidateUser == null)
                        {
                            ModelState.AddModelError(string.Empty, Messages.NotExistsUser);
                            return View(new ResetPasswordViewModel { UserName = string.Empty });
                        }
                        ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel
                        {
                            UserId = userId,
                            UserName = objValidateUser.Username
                        };
                        return View(resetPasswordViewModel);
                    }
                }
                ModelState.AddModelError(string.Empty, string.Format(Messages.InvalidLink, string.Empty));
                return View(new ResetPasswordViewModel { UserName = string.Empty });
            }
            else
            {
                ModelState.AddModelError(string.Empty, string.Format(Messages.InvalidLink, string.Empty));
                return View(new ResetPasswordViewModel { UserName = string.Empty });
            }
        }

        /// <summary>
        /// Reset password post method for reseting password
        /// </summary>
        /// <param name="resetPasswordViewModel"></param>
        /// <returns>If password reset successfully then returns success message other wise returns reset password view with error message</returns>
        [HttpPost]
        public ActionResult Index(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(resetPasswordViewModel.Password))
            {
               
                if (resetPasswordViewModel.OldPassword == resetPasswordViewModel.Password)
                {
                    ModelState.AddModelError(string.Empty, Messages.NewPasswordCanNotBeSame);
                    return View(resetPasswordViewModel);
                }

                MarketingUserViewModel objValidateUser = _userRepository.GetUserDetailsByID(resetPasswordViewModel.UserId);

                if (objValidateUser == null)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.InvalidLink, "Email"));
                    return View(resetPasswordViewModel);
                }
                else
                {
                    if (objValidateUser.Password == Encryption.Encrypt(resetPasswordViewModel.Password))
                    {
                        ModelState.AddModelError(string.Empty, Messages.NewPasswordCanNotBeSame);
                        return View(resetPasswordViewModel);
                    }
                }
                
                bool isReset=_resetPasswordRepository.ResetUserPassword(resetPasswordViewModel.Password, resetPasswordViewModel.UserId);
                if (isReset) 
                return RedirectToAction("Index", "Home", new { Msg = "reset" });
                else return RedirectToAction("Index", "Home", new { Msg = "reset unsuccessfully" });
            }
            else
            {
                return View(resetPasswordViewModel);
            }
        }
        #endregion        
    }
}