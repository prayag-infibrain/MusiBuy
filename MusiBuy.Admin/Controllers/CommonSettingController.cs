using Microsoft.AspNetCore.Mvc;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Common;
using MusiBuy.Common.Models;


namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class CommonSettingController : Controller
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly ICommonSetting _commonsettingRepository;
        private readonly IConfiguration _config;

        public CommonSettingController(ICommonSetting commonsettingRepository, IConfiguration config)
        {
            this._commonsettingRepository = commonsettingRepository;
            this._config = config;            
        }
        #endregion

        #region Edit
        /// <summary>
        /// Common setting get method
        /// </summary>
        /// <returns>Returns common setting view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        [HttpGet]
        public ActionResult Edit()
        {
            CommonSettingViewModel commonSettingViewModel = _commonsettingRepository.GetCommonSetting();
            ViewBag.ModuleName = "Common Settings";
            if (commonSettingViewModel.Id >0)
            {                
                return View(commonSettingViewModel);
            }
            else
                return View(new CommonSettingViewModel());
        }

        /// <summary>
        /// Common setting post method for adding/updating email server configuration & Site URL data
        /// </summary>
        /// <param name="commonSettingViewModel"></param>
        /// <returns>If common setting data added/updated succesfully then returns success message other wise returns common setting view with error message</returns>
        [HttpPost]
        public ActionResult Edit(CommonSettingViewModel commonSettingViewModel)
        {
            string? action = Request.Form["btnSubmit"];
            if (action == "Update")
            {               
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(c => c.Errors.Select(v => v.ErrorMessage)))
                    {
                        ModelState.AddModelError("", error);
                    }

                    return Edit(commonSettingViewModel);
                }

                CommonSettingViewModel tbl = _commonsettingRepository.GetCommonSetting();
                if (tbl != null)
                {
                    bool isUpdated= _commonsettingRepository.UpdateCommonSettings(commonSettingViewModel, CurrentAdminSession.User.UserID);
                    if (isUpdated) {
                        return RedirectToAction("Edit", "CommonSetting", new { Msg = "updated" });
                    }
                }
                else
                {
                    bool isAdded=_commonsettingRepository.AddCommonSettings(commonSettingViewModel, CurrentAdminSession.User.UserID);
                    if (isAdded)
                    {
                        return RedirectToAction("Edit", "CommonSetting", new { Msg = "added" });
                    }
                }
            }
            return RedirectToAction("Edit", "CommonSetting", new { Msg = "not updated" });
        }
        #endregion
    }
}