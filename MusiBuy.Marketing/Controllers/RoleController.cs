using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;
using MusiBuy.Marketing.Helper;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Common;
using MusiBuy.Common.Models;
using MusiBuy.Marketing.CustomBinding;
using Kendo.Mvc.UI;

namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class RoleController : Controller
    {
        #region Members Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IRole _roleRepository;
        private static int _totalCount = 0;

        public RoleController(IRole role)
        {
            this._roleRepository = role;
        }
        #endregion

        #region Index
        /// <summary>
        /// Action to render role index view        
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns role view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            RoleViewModel roleViewModel = new RoleViewModel();
            ViewBag.ModuleName = "Role ";
            return View(roleViewModel);
        }
        #endregion

        #region Ajax Binding
        /// <summary>
        /// Role kendo grid        
        /// </summary>
        /// <param name="request">Kendo grid data source request object</param>
        /// <returns>List of Role</returns>
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult Role_Read([DataSourceRequest] DataSourceRequest command, string roleValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetRoleGridData(command, roleValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetRoleGridData([DataSourceRequest] DataSourceRequest command, string roleValue)
        {
            var result = _roleRepository.GetRoleList(CurrentUserSession.User.RoleID, roleValue);

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

        #region Create Role
        /// <summary>
        /// Action to render role create view 
        /// </summary>
        /// <returns>Returns role create view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create()
        {
            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel.IsActive = true;
            return View(roleViewModel);
        }

        /// <summary>
        ///  Role create post method for adding role data
        /// </summary>
        /// <param name="RoleViewModel"></param>
        /// <returns>If role data saved succesfully then returns success message other wise returns role view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_roleRepository.IsRoleExists(0, (roleViewModel.RoleName ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Role"));
                    return View(roleViewModel);
                }
                roleViewModel.CreatedBy = CurrentUserSession.UserID;
                roleViewModel.CreatedOn = DateTime.UtcNow;
                bool isSaved=_roleRepository.SaveChanges(roleViewModel);
                if(isSaved)
                return RedirectToAction("Index", "Role", new { msg = "added" });
                else return RedirectToAction("Index", "Role", new { msg = "not added" });
            }
            else
            {
                return View(roleViewModel);
            }
        }
        #endregion

        #region Edit Role
        /// <summary>
        /// Role edit get method to get fetch data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns role edit view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(int id)
        {
            if (id == 1)
            {
                return RedirectToAction("Index", "Role", new { Msg = "admin" });
            }
            else
            {
                RoleViewModel roleViewModel = _roleRepository.GetRoleDetail(id);
                if (roleViewModel == null)
                {
                    return RedirectToAction("Index", "Role", new { msg = "drop" });
                }
                return View(roleViewModel);
            }
        }

        /// <summary>
        /// Role edit post method for updating role data
        /// </summary>
        /// <param name="RoleViewModel"></param>
        /// <returns>If role data updated succesfully then returns success message other wise returns role view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_roleRepository.IsRoleExists(roleViewModel.Id, (roleViewModel.RoleName ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Role"));
                    return View(roleViewModel);
                }
                else
                {
                    roleViewModel.UpdatedBy = CurrentUserSession.UserID;
                    roleViewModel.UpdatedOn = DateTime.UtcNow;
                    bool isUpdated = _roleRepository.SaveChanges(roleViewModel);
                    if (isUpdated)
                    {
                        if (roleViewModel.Id == CurrentUserSession.RoleID)
                        {
                            return RedirectToAction("Index", "Role", new { msg = "currentroleupdate" });
                        }
                        return RedirectToAction("Index", "Role", new { msg = "updated" });
                    }
                    return RedirectToAction("Index", "Role", new { msg = "not updated" });

                }
            }
            else
            {
                return View(roleViewModel);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        ///  Delete role post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If role data deleted succesfully then returns success message other wise returns error message on role list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    bool isDeleted =_roleRepository.DeleteRoles(chkDelete);
                    if (isDeleted) 
                    return RedirectToAction("Index", "Role", new { msg = "deleted" });
                    else return RedirectToAction("Index", "Role", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "Role", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Role", new { msg = "inuse" });
            }
        }
        #endregion

        #region Detail
        /// <summary>
        /// Role detail get method to get role data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns role detail view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        public ActionResult Detail(int id)
        {
            var objRoleViewModel = _roleRepository.GetRoleDetail(id);
            if (objRoleViewModel == null)
            {
                return RedirectToAction("Index", "Role", new { msg = "drop" });
            }
            return View(objRoleViewModel);
        }
        #endregion

        #region Validate Duplicate Role
        /// <summary>
        /// Validate if Role is already exists
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="Role"></param>
        /// <returns>If role exists then returns 0 otherwise 1 in json</returns>
        [HttpGet]
        public JsonResult ValidateDuplicateRole(int? roleID, string role)
        {
            bool isRoleExists = _roleRepository.IsRoleExists(Convert.ToInt32(roleID), role.Trim());
            return Json(!isRoleExists);
        }
        #endregion
    }
}