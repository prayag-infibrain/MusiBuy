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

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class CreatoresController : Controller
    {
        private readonly ICreatores _creatoresRepository;
        private readonly IRole _roleRepository;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public CreatoresController(ICreatores creatores, IRole role, IConfiguration config)
        {
            this._creatoresRepository = creatores;
            this._roleRepository = role;
            this._config = config;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Creatores";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetCreatoresGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetCreatoresGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _creatoresRepository.GetCreatoresList(CurrentAdminSession.User.UserID, searchValue);

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
            CreatoresViewModel objValidateUser = new CreatoresViewModel();
            objValidateUser.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(CreatoresViewModel objValidateCreatores)
        {
            if (ModelState.IsValid)
            {

                if (_creatoresRepository.IsEmailExists(0, (objValidateCreatores.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    objValidateCreatores.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                    return View(objValidateCreatores);
                }


                #region Upload Image
                string rootPath = _config.GetSection("FilePath:FilePath").Value;
                string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.CreatorImages);

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                if (objValidateCreatores.ProfilePicture != null)
                {
                    string extension = Path.GetExtension(objValidateCreatores.ProfilePicture.FileName);
                    string fileName = $"{Guid.NewGuid()}{extension}";
                    string fullPath = Path.Combine(targetPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        objValidateCreatores.ProfilePicture.CopyTo(stream);
                    }

                    objValidateCreatores.StrProfilePicture = fileName;
                }
                #endregion

                objValidateCreatores.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _creatoresRepository.Save(objValidateCreatores);
                if (isSaved)
                    return RedirectToAction("Index", "Creatores", new { msg = "added" });
                else
                    return RedirectToAction("Index", "Creatores", new { msg = "not added" });
            }
            else
            {
                objValidateCreatores.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                return View(objValidateCreatores);
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
            CreatoresViewModel objValidateCreatores = _creatoresRepository.GetCreatoresDetailsByID(id);
            if (objValidateCreatores == null)
            {
                return RedirectToAction("Index", "Creatores", new { msg = "drop" });
            }

            objValidateCreatores.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
            return View(objValidateCreatores);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateUser"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(CreatoresViewModel objValidateCreatores)
        {
            if (ModelState.IsValid)
            {
                var UserDetail = _creatoresRepository.GetCreatoresDetailsByID(objValidateCreatores.Id);
                if (UserDetail == null)
                {
                    return RedirectToAction("Index", "Creatores", new { msg = "drop" });
                }               
                if (_creatoresRepository.IsEmailExists(objValidateCreatores.Id, (objValidateCreatores.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    objValidateCreatores.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                    return View(objValidateCreatores);
                }
                #region Updaload Image
                if (objValidateCreatores.ProfilePicture != null)
                {
                    string rootPath = _config.GetSection("FilePath:FilePath").Value;
                    string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode. CreatorImages);

                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    if (objValidateCreatores.ProfilePicture != null)
                    {
                        string extension = Path.GetExtension(objValidateCreatores.ProfilePicture.FileName);
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        string fullPath = Path.Combine(targetPath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            objValidateCreatores.ProfilePicture.CopyTo(stream);
                        }

                        objValidateCreatores.StrProfilePicture = fileName;
                    }
                }
                #endregion

                objValidateCreatores.UpdatedBy = CurrentAdminSession.UserID;


                bool isUpdated = _creatoresRepository.Save(objValidateCreatores);
                if (isUpdated)
                    return RedirectToAction("Index", "Creatores", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "Creatores", new { msg = "not updated" });

            }
            else
            {
                objValidateCreatores.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                return View(objValidateCreatores);
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
                CreatoresViewModel objValidateUser = _creatoresRepository.GetCreatoresDetailsByID(id);
                if (objValidateUser == null)
                {
                    return RedirectToAction("Index", "Creatores", new { msg = "drop" });
                }
                else
                {
                    return View(objValidateUser);
                }
            }
            else
            {
                return RedirectToAction("Index", "Creatores", new { msg = "error" });
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
                    bool isDeleted = _creatoresRepository.DeleteCreatores(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "Creatores", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "Creatores", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "Creatores", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Creatores", new { msg = "inuse" });
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
            bool isUserEmailExists = _creatoresRepository.IsEmailExists(Id.HasValue ? Id.Value : 0, email.Trim());
            return Json(!isUserEmailExists);
        }
        #endregion



        #region Delete Profile Picture
        [HttpGet]
        public IActionResult DeleteImage(int id)
        {
            string? fileName = _creatoresRepository.GetCreatoreProfilePicture(id);
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new { success = false, msg = "No image to delete." });
            }

            string fileReadPath = _config.GetSection("FilePath:FilePath").Value;
            string relativePath = "/wwwroot/" + GlobalCode.CreatorImages.Replace("\\", "/").Trim('/');
            string fullPath = $"{fileReadPath}{relativePath}/{fileName}";

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _creatoresRepository.RemoveCreatoreProfilePicture(id);

                return Json(new { success = true, msg = "Profile Picture deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "Error deleting Profile Picture: " + ex.Message });
            }
        }
        #endregion

    }
}