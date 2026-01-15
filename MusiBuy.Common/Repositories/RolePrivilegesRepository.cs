using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace MusiBuy.Common.Repositories
{
    public class RolePrivilegesRepository : IRolePrivileges
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        public RolePrivilegesRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Add Role Privileges
        /// <summary>
        /// Add role privileges
        /// </summary>
        /// <returns>bool</returns>
        public void AddRolePrivileges(List<RolePrivilegesViewModel> rolePrivilegesViewModelList)
        {
            List<RolePrivilege> rolePrivileges = rolePrivilegesViewModelList.Select(x => new RolePrivilege
            {
                RoleId = x.RoleId,
                MenuItemId = x.MenuItemId,
                Add = x.Add,
                Edit = x.Edit,
                Delete = x.Delete,
                Detail = x.Detail,
                View = x.View,
                IsActive = true
            }).ToList();

            _Context.RolePrivileges.AddRange(rolePrivileges);
            _Context.SaveChanges();
        }
        #endregion

        #region Delete Role Privileges by Role ID
        /// <summary>
        /// Delete role privileges by role ID
        /// </summary>
        /// <returns>void</returns>
        public void DeleteRolePrivilegesByRoleID(int roleID)
        {
            _Context.RolePrivileges.RemoveRange(_Context.RolePrivileges.Where(c => c.RoleId == roleID).AsEnumerable());
            _Context.SaveChanges();
        }
        #endregion

        #region Get Role Privileges & Menu Items
        /// <summary>
        /// Get role priviledges by role id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<RolePrivilegesViewModel> GetRolePrivilegesByRoleId(int roleId)
        {
            List<RolePrivilegesViewModel> objListRolePrivileges = _Context.RolePrivileges.Where(x => x.RoleId == roleId && x.View.HasValue && x.View.Value && x.MenuItem.IsActive).OrderBy(p => p.MenuItem.SortOrder)
                .Select(x => new RolePrivilegesViewModel
                {
                    RoleId = x.RoleId,
                    MenuItemId = x.MenuItemId,
                    View = x.View.HasValue ? x.View.Value : false,
                    Add = x.Add.HasValue ? x.Add.Value : false,
                    Delete = x.Delete.HasValue ? x.Delete.Value : false,
                    Edit = x.Edit.HasValue ? x.Edit.Value : false,
                    Detail = x.Detail.HasValue ? x.Detail.Value : false,
                    IsActive = x.IsActive,
                    
                    MenuItem = new MenuItemViewModel
                    {
                        Id = x.MenuItem.Id,
                        MenuItemName = x.MenuItem.MenuItemName,
                        MenuItemController = x.MenuItem.MenuItemController,
                        MenuItemView = x.MenuItem.MenuItemView,
                        ParentId = x.MenuItem.ParentId,
                        SortOrder = x.MenuItem.SortOrder,
                        IsActive = x.MenuItem.IsActive,
                        ImageName = x.MenuItem.ImageName,
                      


                    }
                }).OrderBy(x=>x.MenuItem.SortOrder).ToList();
           
            return objListRolePrivileges;
        }
        #endregion
    }
}
