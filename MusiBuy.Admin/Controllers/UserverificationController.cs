using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Common;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Repositories;

namespace MusiBuy.Admin.Controllers
{
    public class UserverificationController : Controller
    {

        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IResetPassword _resetPasswordRepository;
        private readonly IFrontUser _userRepository;

        public UserverificationController(IResetPassword resetPassword, IFrontUser user)
        {
            this._resetPasswordRepository = resetPassword;
            this._userRepository = user;
        }
        #endregion

        public ActionResult Index()
        {
            Microsoft.Extensions.Primitives.StringValues queryStringVal;
            HttpContext.Request.Query.TryGetValue("UID", out queryStringVal);
            if (!string.IsNullOrWhiteSpace(queryStringVal) && queryStringVal != "")
            {
                string? userDetails = queryStringVal;
                if (!string.IsNullOrWhiteSpace(userDetails) && userDetails != "")
                {
                    int userId;
                    if (int.TryParse(userDetails.Substring(userDetails.LastIndexOf("-") + 1), out userId))
                    {
                        var objValidateUser = _userRepository.GetFrontUserDetailById(userId);
                        if (objValidateUser == null)
                        {
                            ViewBag.Status = "fail";
                            ViewBag.Message = "User does not exist.";
                            return View();
                            //ModelState.AddModelError(string.Empty, Messages.NotExistsUser);
                            //return View(new ResetPasswordViewModel { UserName = string.Empty });
                        }
                        bool Status = _userRepository.ActiveUserById(userId);
                        if (Status)
                        {
                            ViewBag.Status = "success";
                            ViewBag.Message = "Your account has been successfully verified.";
                        }
                        else
                        {
                            ViewBag.Status = "fail";
                            ViewBag.Message = "Account verification failed. Please try again later.";
                        }
                        return View();
                    }
                }
                ViewBag.Status = "fail";
                ViewBag.Message = "Invalid User.";
                return View();
            }
            else
            {

                ViewBag.Status = "fail";
                ViewBag.Message = "Invalid verification link.";
                return View();
            }
        }
    }
}
