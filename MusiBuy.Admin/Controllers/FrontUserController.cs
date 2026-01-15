using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using System.Collections;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class FrontUserController : Controller
    {
        private readonly IFrontUser _frontuserRepository;
        private readonly IRole _roleRepository;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public FrontUserController(IFrontUser frontuser, IRole role, IConfiguration config)
        {
            this._frontuserRepository = frontuser;
            this._roleRepository = role;
            this._config = config;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Front User";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetFrontUserGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetFrontUserGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _frontuserRepository.GetFrontUserList(CurrentAdminSession.User.UserID, searchValue);

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
            FrontUserViewModel objValidateUser = new FrontUserViewModel();
            BindDropdown(objValidateUser);
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(FrontUserViewModel objValidateFrontUser)
        {
            if (ModelState.IsValid)
            {

                if (_frontuserRepository.IsEmailExists(0, (objValidateFrontUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    BindDropdown(objValidateFrontUser);
                    return View(objValidateFrontUser);
                }


                #region Upload Image
                string rootPath = _config.GetSection("FilePath:FilePath").Value;
                string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.FrontUserImages);

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                if (objValidateFrontUser.Image != null)
                {
                    string extension = Path.GetExtension(objValidateFrontUser.Image.FileName);
                    string fileName = $"{Guid.NewGuid()}{extension}";
                    string fullPath = Path.Combine(targetPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        objValidateFrontUser.Image.CopyTo(stream);
                    }

                    objValidateFrontUser.StrImage = fileName;
                }
                #endregion

                objValidateFrontUser.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _frontuserRepository.Save(objValidateFrontUser);
                if (isSaved)
                    return RedirectToAction("Index", "FrontUser", new { msg = "added" });
                else
                    return RedirectToAction("Index", "FrontUser", new { msg = "not added" });
            }
            else
            {
                BindDropdown(objValidateFrontUser);
                return View(objValidateFrontUser);
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
            FrontUserViewModel objValidateFrontUser = _frontuserRepository.GetFrontUserDetailById(id);
            if (objValidateFrontUser == null)
            {
                return RedirectToAction("Index", "FrontUser", new { msg = "drop" });
            }

            BindDropdown(objValidateFrontUser);
            return View(objValidateFrontUser);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateUser"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(FrontUserViewModel objValidateFrontUser)
        {
            if (ModelState.IsValid)
            {
                var UserDetail = _frontuserRepository.GetFrontUserDetailById(objValidateFrontUser.Id);
                if (UserDetail == null)
                {
                    return RedirectToAction("Index", "FrontUser", new { msg = "drop" });
                }               
                if (_frontuserRepository.IsEmailExists(objValidateFrontUser.Id, (objValidateFrontUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    BindDropdown(objValidateFrontUser);
                    return View(objValidateFrontUser);
                }
                #region Updaload Image
                if (objValidateFrontUser.Image != null)
                {
                    string rootPath = _config.GetSection("FilePath:FilePath").Value;
                    string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.FrontUserImages);

                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    if (objValidateFrontUser.Image != null)
                    {
                        string extension = Path.GetExtension(objValidateFrontUser.Image.FileName);
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        string fullPath = Path.Combine(targetPath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            objValidateFrontUser.Image.CopyTo(stream);
                        }

                        objValidateFrontUser.StrImage = fileName;
                    }
                }
                #endregion

                objValidateFrontUser.UpdatedBy = CurrentAdminSession.UserID;


                bool isUpdated = _frontuserRepository.Save(objValidateFrontUser);
                if (isUpdated)
                    return RedirectToAction("Index", "FrontUser", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "FrontUser", new { msg = "not updated" });

            }
            else
            {
                BindDropdown(objValidateFrontUser);
                return View(objValidateFrontUser);
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
                FrontUserViewModel objValidateUser = _frontuserRepository.GetFrontUserDetailById(id);
                if (objValidateUser == null)
                {
                    return RedirectToAction("Index", "FrontUser", new { msg = "drop" });
                }
                else
                {
                    return View(objValidateUser);
                }
            }
            else
            {
                return RedirectToAction("Index", "FrontUser", new { msg = "error" });
            }
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
                    bool isDeleted = _frontuserRepository.DeleteCreatores(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "FrontUser", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "FrontUser", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "FrontUser", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "FrontUser", new { msg = "inuse" });
            }

        }
        #endregion

        //#region Validate Duplidate Username
        ///// <summary>
        /////  Validate Duplicate Username
        ///// </summary>
        ///// <param name="Username"></param>
        ///// <param name="UserID"></param>
        ///// <returns>If User already exists then returns 0 otherwise 1 in json</returns>

        //[HttpGet]
        //public JsonResult ValidateDuplicateUser(string Username, int? Id)
        //{
        //    bool isUsernameExists = _creatoresRepository.IsUserExists(Id.HasValue ? Id.Value : 0, Username.Trim());
        //    return Json(!isUsernameExists);
        //}
        //#endregion

        #region Validate Duplicate Email
        /// <summary>
        /// Duplicate Email – This method is used to check department is already exist or not
        /// </summary>        
        public JsonResult ValidateDuplicateEmail(string email, int? Id)
        {
            bool isUserEmailExists = _frontuserRepository.IsEmailExists(Id.HasValue ? Id.Value : 0, email.Trim());
            return Json(!isUserEmailExists);
        }
        #endregion



        #region Delete Profile Picture
        [HttpGet]
        public IActionResult DeleteImage(int id)
        {
            string? fileName = _frontuserRepository.GetUserProfilePicture(id);
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new { success = false, msg = "No image to delete." });
            }

            string fileReadPath = _config.GetSection("FilePath:FilePath").Value;
            string relativePath = "/wwwroot/" + GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');
            string fullPath = $"{fileReadPath}{relativePath}/{fileName}";

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _frontuserRepository.RemoveUserProfilePicture(id);

                return Json(new { success = true, msg = "Profile Picture deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "Error deleting Profile Picture: " + ex.Message });
            }
        }
        #endregion

        #region Bind Dropdown
        public void BindDropdown(FrontUserViewModel objValidateUser)
        {
            objValidateUser.SelectRole = new SelectList(_roleRepository.GetCreatorRoleList(), "value", "name");
            objValidateUser.SelectUserType = new SelectList(new List<SelectListItem>
                                                {
                                                    new SelectListItem { Text = "User", Value = "1" },
                                                    new SelectListItem { Text = "Creator", Value = "2" }
                                                }, "Value", "Text");
        }
        #endregion

    }
}