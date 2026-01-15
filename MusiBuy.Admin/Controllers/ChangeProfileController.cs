using MusiBuy.Common.Common;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Admin.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class ChangeProfileController : Controller
    {
        #region Members Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IChangeProfile _changeProfileRepository;
        private readonly ICommonSetting _common;

        public ChangeProfileController(IChangeProfile changeProfile, ICommonSetting common)
        {
            this._changeProfileRepository = changeProfile;
            _common = common;
        }
        #endregion

        #region Index
        /// <summary>
        ///  Change profile get method
        /// </summary>
        /// <returns>Returns change profile view</returns>
        public ActionResult Index()
        {
            ChangeProfileViewModel changeprofile = _changeProfileRepository.GetUserByID(CurrentAdminSession.UserID);
            //changeprofile.SelectCountry = new System.Web.Mvc.SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
            changeprofile.SelectCountry = _common.GetCountryCodeDropDownList().Select(x => new SelectListItem
                                        {
                                            Value = x.value.ToString(),
                                            Text = x.name
                                        }).ToList();
            return View(changeprofile);
        }

        /// <summary>
        /// Change profile post method for updating user profile data
        /// </summary>
        /// <param name="changeProfileViewModel"></param>
        /// <returns>If user profile data updated succesfully then returns success message  other wise returns change profile view with error message</returns>
        [HttpPost]
        public ActionResult Index(ChangeProfileViewModel changeProfileViewModel)
        {
            ModelState.Remove("SelectCountry");
            if (ModelState.IsValid)
            {
                if (_changeProfileRepository.IsEmailExists((changeProfileViewModel.Email ?? string.Empty).Trim(), CurrentAdminSession.UserID))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email "));
                    return View(changeProfileViewModel);
                }
                bool isUpdated = _changeProfileRepository.UpdateProfile(changeProfileViewModel);
                if (isUpdated)
                {
                    CurrentAdminUser User = CurrentAdminSession.User;
                    User.FirstName = changeProfileViewModel.FirstName ?? string.Empty;
                    User.LastName = changeProfileViewModel.Lastname ?? string.Empty;
                    User.Email = changeProfileViewModel.Email ?? string.Empty;
                    CurrentAdminSession.User = User;
                    return RedirectToAction("Home", "Home", new { Msg = "profile" });
                }
                return View(changeProfileViewModel);

            }
            else
            {
                return View(changeProfileViewModel);
            }
        }
        #endregion
    }
}