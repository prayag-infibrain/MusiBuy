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
using MusiBuy.Common.Repositories;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class CustomerController : Controller
    {
        private readonly ICustomer _customerRepository;
        private readonly IConfiguration _config;
        private readonly ICommonSetting _common;
        private static int _totalCount = 0;

        public CustomerController(ICustomer customer, IConfiguration config, ICommonSetting common)
        {
            this._customerRepository = customer;
            this._config = config;
            this._common = common;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Customer";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetCustomerGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetCustomerGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _customerRepository.GetCustomerList(CurrentAdminSession.User.UserID, searchValue);

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
            CustomerViewModel objValidateUser = new CustomerViewModel();
            objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(CustomerViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {

                if (_customerRepository.IsEmailExists(0, (objValidateUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    return View(objValidateUser);
                }

                #region Upload Image
                string rootPath = _config.GetSection("FilePath:FilePath").Value;
                string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.CustomerImages);

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                if (objValidateUser.ProfilePicture != null)
                {
                    string extension = Path.GetExtension(objValidateUser.ProfilePicture.FileName);
                    string fileName = $"{Guid.NewGuid()}{extension}";
                    string fullPath = Path.Combine(targetPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        objValidateUser.ProfilePicture.CopyTo(stream);
                    }

                    objValidateUser.StrProfilePicture = fileName;
                    objValidateUser.MediaFileName = fileName;
                }

                #endregion
                objValidateUser.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _customerRepository.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "Customer", new { msg = "added" });
                else
                    return RedirectToAction("Index", "Customer", new { msg = "not added" });
            }
            else
            {
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
            CustomerViewModel objValidateUser = _customerRepository.GetCustomerDetailsByID(id);
            if (objValidateUser == null)
            {
                return RedirectToAction("Index", "Customer", new { msg = "drop" });
            }
            objValidateUser.SelectCountry = new SelectList(_common.GetCountryCodeDropDownList(), "value", "name");
            return View(objValidateUser);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateUser"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(CustomerViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {
                var UserDetail = _customerRepository.GetCustomerDetailsByID(objValidateUser.Id);
                if (UserDetail == null)
                {
                    return RedirectToAction("Index", "Customer", new { msg = "drop" });
                }               
                if (_customerRepository.IsEmailExists(objValidateUser.Id, (objValidateUser.Email ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Email"));
                    return View(objValidateUser);
                }

                #region Updaload Image
                if (objValidateUser.ProfilePicture != null || !string.IsNullOrEmpty(objValidateUser.MediaFileName))
                {
                    string rootPath = _config.GetSection("FilePath:FilePath").Value;
                    string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.CustomerImages);

                    if (!Directory.Exists(targetPath))
                        Directory.CreateDirectory(targetPath);
                        
                    if (objValidateUser.ProfilePicture != null)
                    {
                        string extension = Path.GetExtension(objValidateUser.ProfilePicture.FileName);
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        string fullPath = Path.Combine(targetPath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            objValidateUser.ProfilePicture.CopyTo(stream);
                        }

                        objValidateUser.MediaFileName = fileName;
                    }
                }
                #endregion

                objValidateUser.UpdatedBy = CurrentAdminSession.UserID;


                bool isUpdated = _customerRepository.Save(objValidateUser);
                if (isUpdated)
                    return RedirectToAction("Index", "Customer", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "Customer", new { msg = "not updated" });

            }
            else
            {
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
                CustomerViewModel objValidateUser = _customerRepository.GetCustomerDetailsByID(id);
                if (objValidateUser == null)
                {
                    return RedirectToAction("Index", "Customer", new { msg = "drop" });
                }
                else
                {
                    return View(objValidateUser);
                }
            }
            else
            {
                return RedirectToAction("Index", "Customer", new { msg = "error" });
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
                    bool isDeleted = _customerRepository.DeleteCustomer(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "Customer", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "Customer", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "Customer", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Customer", new { msg = "inuse" });
            }

        }
        #endregion

        #region Validate Duplicate Email
        /// <summary>
        /// Duplicate Email – This method is used to check department is already exist or not
        /// </summary>        
        public JsonResult ValidateDuplicateEmail(string email, int? Id)
        {
            bool isUserEmailExists = _customerRepository.IsEmailExists(Id.HasValue ? Id.Value : 0, email.Trim());
            return Json(!isUserEmailExists);
        }
        #endregion

        #region Delete Profile Picture
        [HttpGet]
        public IActionResult DeleteImage(int id)
        {
            string? fileName = _customerRepository.GetCustomerProfilePicture(id);
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new { success = false, msg = "No image to delete." });
            }

            string fileReadPath = _config.GetSection("FilePath:FilePath").Value;
            string relativePath = "/wwwroot/" + GlobalCode.CustomerImages.Replace("\\", "/").Trim('/');
            string fullPath = $"{fileReadPath}{relativePath}/{fileName}";

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _customerRepository.RemoveCustomerProfilePicture(id);

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