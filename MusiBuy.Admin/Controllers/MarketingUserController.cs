using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Admin.Helper;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using MusiBuy.Common.Interfaces.Marketing;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class MarketingUserController : Controller
    {
        private readonly IMarketingUser _userRepository;
        private readonly IConfiguration _config;
        private readonly ICommonSetting _common;
        private static int _totalCount = 0;

        public MarketingUserController(IMarketingUser user, IRole role, IConfiguration config, ICommonSetting common)
        {
            this._userRepository = user;
            this._config = config;
            _common = common;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "MarketingUser";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetUserGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetUserGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _userRepository.GetUserList(searchValue);

            result = result.ApplyFiltering(command.Filters);

            _totalCount = result.Count();

            result = result.ApplySorting(command.Groups, command.Sorts);

            result = result.ApplyPaging(command.Page, command.PageSize);

            if (command.Groups.Any())
            {
                return result.ApplyGrouping(command.Groups);
            }
            return result.ToList();
        }
        #endregion

        #region Create
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public IActionResult Create()
        {
            MarketingUserViewModel objValidateUser = new MarketingUserViewModel();
            objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(MarketingUserViewModel objValidateUser)
        {
            ModelState.Remove("UserTypeValue");
            if (ModelState.IsValid)
            {
                if (_userRepository.IsUserExists(0, (objValidateUser.Username ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                    objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                    return View(objValidateUser);
                }

                if (_userRepository.IsEmailExists(0, (objValidateUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                    return View(objValidateUser);
                }
                bool isSaved = _userRepository.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "MarketingUser", new { msg = "added" });
                else
                    return RedirectToAction("Index", "MarketingUser", new { msg = "not added" });
            }
            else
            {
                objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                return View(objValidateUser);
            }
        }
        #endregion

        #region Edit
        /// <summary>
        /// User edit get method to get fetch user data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user edit view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(int id)
        {
            MarketingUserViewModel objValidateUser = _userRepository.GetUserDetailsByID(id);
            if (objValidateUser == null)
                return RedirectToAction("Index", "MarketingUser", new { msg = "drop" });

            objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
            objValidateUser.Password = Encryption.Decrypt(objValidateUser.Password ?? string.Empty);
            objValidateUser.ConfirmPassword = Encryption.Decrypt(objValidateUser.Password);
            return View(objValidateUser);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateUser"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(MarketingUserViewModel objValidateUser)
        {
            ModelState.Remove("UserTypeValue");
            if (ModelState.IsValid)
            {
                var UserDetail = _userRepository.GetUserDetailsByID(objValidateUser.Id);
                if (UserDetail == null)
                {
                    objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                    return RedirectToAction("Index", "MarketingUser", new { msg = "drop" });
                }
                if (_userRepository.IsUserExists(objValidateUser.Id, (objValidateUser.Username ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                    objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                    return View(objValidateUser);
                }

                if (_userRepository.IsEmailExists(objValidateUser.Id, (objValidateUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                    return View(objValidateUser);
                }

                bool isUpdated = _userRepository.Save(objValidateUser);
                if (isUpdated)
                    return RedirectToAction("Index", "MarketingUser", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "MarketingUser", new { msg = "not updated" });

            }
            else
            {
                objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
                return View(objValidateUser);
            }
        }
        #endregion

        #region Detail
        /// <summary>
        /// User detail get method to get enum data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user detail view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        public ActionResult Detail(int id)
        {
            if (id > 0)
            {
                MarketingUserViewModel objValidateUser = _userRepository.GetUserDetailsByID(id);
                if (objValidateUser == null)
                    return RedirectToAction("Index", "MarketingUser", new { msg = "drop" });
                else
                    return View(objValidateUser);
            }
            else
                return RedirectToAction("Index", "MarketingUser", new { msg = "error" });
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete user post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If user data deleted succesfully then returns success message other wise returns error message on user list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    bool isDeleted = _userRepository.DeleteUsers(chkDelete);
                    if (isDeleted)
                        return RedirectToAction("Index", "MarketingUser", new { msg = "deleted" });
                    return RedirectToAction("Index", "MarketingUser", new { msg = "not deleted" });
                }
                else
                    return RedirectToAction("Index", "MarketingUser", new { msg = "noselect" });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "MarketingUser", new { msg = "inuse" });
            }

        }
        #endregion

        #region Validate Duplidate Username
        /// <summary>
        ///  Validate Duplicate Username
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="UserID"></param>
        /// <returns>If User already exists then returns 0 otherwise 1 in json</returns>

        [HttpGet]
        public JsonResult ValidateDuplicateUser(string Username, int? Id)
        {
            bool isUsernameExists = _userRepository.IsUserExists(Id.HasValue ? Id.Value : 0, Username.Trim());
            return Json(!isUsernameExists);
        }
        #endregion

        #region Validate Duplicate Email
        /// <summary>
        /// Duplicate Email – This method is used to check department is already exist or not
        /// </summary>        
        public JsonResult ValidateDuplicateEmail(string email, int? Id)
        {
            bool isUserEmailExists = _userRepository.IsEmailExists(Id.HasValue ? Id.Value : 0, email.Trim());
            return Json(!isUserEmailExists);
        }
        #endregion
    }
}