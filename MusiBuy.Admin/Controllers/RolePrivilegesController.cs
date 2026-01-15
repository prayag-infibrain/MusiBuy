using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.Models;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Helpers;

namespace Reconxn.Admin.Controllers
{
    [ValidateAdminLogin]
    public class RolePrivilegesController : Controller
    {
        #region Members Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IRolePrivileges _rolePrivilegesRepository;
        private readonly IRole _roleRepository;
        private readonly IMenu _menuRepository;

        public RolePrivilegesController(IRolePrivileges rolePrivileges, IRole role, IMenu menu)
        {
            this._rolePrivilegesRepository = rolePrivileges;
            this._roleRepository = role;
            this._menuRepository = menu;
        }
        #endregion

        #region Index
        /// <summary>
        /// Role Privileges index get method
        /// </summary>
        /// <returns>Returns role privileges index view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            RolePrivilegesViewModel rolePrivilegesViewModel = new RolePrivilegesViewModel();
            ViewBag.ModuleName = "Role privileges ";
            rolePrivilegesViewModel.Roles = new SelectList(_roleRepository.GetRolePrivilegesDropDownList(true), "value", "name");
            return View(rolePrivilegesViewModel);
        }

        /// <summary>
        /// Role Privileges index post method to save Privileges data
        /// </summary>
        /// <param name="Role_ID"></param>
        /// <param name="btnSubmit"></param>
        /// <param name="RolePrivilegesViewModel"></param>
        /// <returns>If role privileges data saved succesfully then returns success message other wise returns role privileges view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult Index(int? roleID, string? btnSubmit, RolePrivilegesViewModel rolePrivilegesViewModel)
        {
            if (Convert.ToInt32(roleID) > 0 && btnSubmit == null)
            {
                var menuData = _menuRepository.GetMenuList();
                var rolePrivilegesData = _rolePrivilegesRepository.GetRolePrivilegesByRoleId(roleID.Value);

                var privilegeList1 = (from m in menuData
                                      join y in rolePrivilegesData on m.Id equals y.MenuItemId into joinVal
                                      from r in joinVal.DefaultIfEmpty()

                                      select new
                                      {
                                          m,
                                          r
                                      }).ToList();

                var privilegeList = (from m in menuData
                                     join y in rolePrivilegesData on m.Id equals y.MenuItemId into joinVal
                                     from r in joinVal.DefaultIfEmpty()
                                     select new RolePrivilegesViewModel
                                     {
                                         MenuItemId = m.Id,
                                         MenuItem = m,
                                         RoleId = r == null ? 0 : r.RoleId,
                                         View = r == null ? false : r.View,
                                         Add = r == null ? false : r.Add,
                                         Edit = r == null ? false : r.Edit,
                                         Delete = r == null ? false : r.Delete,
                                         Detail = r == null ? false : r.Detail,
                                         IsActive = r == null ? false : r.IsActive
                                     }).ToList();

                SessionManager.SetSession("RolePrivileges", privilegeList);
                rolePrivilegesViewModel.Roles = new SelectList(_roleRepository.GetRolePrivilegesDropDownList(true), "value", "name");
                return View(rolePrivilegesViewModel);
            }
            else if (roleID > 0)
            {
                string strHdMenuItemID = Request.Form["hdMenuItemID"];
                string[] strHdMenuItemIDArray = strHdMenuItemID.Split(',');

                string strChkView = Request.Form["chkView"];
                string[] strChkViewArray = null;
                if (strChkView != null)
                    strChkViewArray = strChkView.Split(',');

                string strChkAdd = Request.Form["chkAdd"];
                string[] strChkAddArray = null;
                if (strChkAdd != null)
                    strChkAddArray = strChkAdd.Split(',');

                string strChkEdit = Request.Form["chkEdit"];
                string[] strChkEditArray = null;
                if (strChkEdit != null)
                    strChkEditArray = strChkEdit.Split(',');

                string strChkDelete = Request.Form["chkDelete"];
                string[] strChkDeleteArray = null;
                if (strChkDelete != null)
                    strChkDeleteArray = strChkDelete.Split(',');

                string strChkDetail = Request.Form["chkDetail"];
                string[] strChkDetailArray = null;
                if (strChkDetail != null)
                    strChkDetailArray = strChkDetail.Split(',');

                #region Delete RolePrivileges Using RoleID
                if (roleID.HasValue && roleID.Value > 0)
                {
                    _rolePrivilegesRepository.DeleteRolePrivilegesByRoleID(roleID.Value);
                }
                #endregion

                int MenuItemID = 0;
                List<RolePrivilegesViewModel> rolePrivilegesList = new List<RolePrivilegesViewModel>();
                foreach (string item in strHdMenuItemIDArray)
                {
                    rolePrivilegesViewModel = new RolePrivilegesViewModel();
                    MenuItemID = Convert.ToInt32(item);

                    if (strChkViewArray != null && strChkViewArray.Contains("v" + MenuItemID))
                        rolePrivilegesViewModel.View = true;
                    if (strChkAddArray != null && strChkAddArray.Contains("a" + MenuItemID))
                        rolePrivilegesViewModel.Add = true;
                    if (strChkEditArray != null && strChkEditArray.Contains("e" + MenuItemID))
                        rolePrivilegesViewModel.Edit = true;
                    if (strChkDeleteArray != null && strChkDeleteArray.Contains("d" + MenuItemID))
                        rolePrivilegesViewModel.Delete = true;
                    if (strChkDetailArray != null && strChkDetailArray.Contains("de" + MenuItemID))
                        rolePrivilegesViewModel.Detail = true;

                    if ((rolePrivilegesViewModel.View.HasValue && rolePrivilegesViewModel.View.Value) || 
                        (rolePrivilegesViewModel.Add.HasValue && rolePrivilegesViewModel.Add.Value) || 
                        (rolePrivilegesViewModel.Edit.HasValue && rolePrivilegesViewModel.Edit.Value) ||
                        (rolePrivilegesViewModel.Delete.HasValue && rolePrivilegesViewModel.Delete.Value) ||
                        (rolePrivilegesViewModel.Delete.HasValue && rolePrivilegesViewModel.Delete.Value))
                    {
                        rolePrivilegesViewModel.RoleId = roleID.Value;
                        rolePrivilegesViewModel.MenuItemId = MenuItemID;

                        #region Insert RolePrivileges 
                        rolePrivilegesList.Add(rolePrivilegesViewModel);
                        #endregion
                    }
                }

                if (rolePrivilegesList.Count > 0) {
                    _rolePrivilegesRepository.AddRolePrivileges(rolePrivilegesList);
                }

                SessionManager.SetSession("RolePrivileges",null);
                return RedirectToAction("Index", "RolePrivileges", new { Msg = "added" });
            }
            else
            {
                SessionManager.SetSession("RolePrivileges", null);
                return RedirectToAction("Index", "RolePrivileges");
            }
        }
        #endregion
    }
}