using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusiBuy.Common.Repositories
{
    public class RoleRepository : IRole
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        public RoleRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get Role View Data
        /// <summary>
        /// Get role list
        /// </summary>
        /// <returns>IQeryable of RoleViewModel</returns>
        public IQueryable<RoleViewModel> GetRoleList(int? roleId, string roleValue)
        {
            var result = (from p in _Context.Roles
                          orderby p.RoleName
                          where (string.IsNullOrWhiteSpace(roleValue) || p.RoleName.ToLower().Contains(roleValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(roleValue) ? (p.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(roleValue) : true))
                          select new RoleViewModel
                          {
                              Id = p.Id,
                              RoleName = p.RoleName,
                              IsCurrentUserRole = roleId == p.Id ? true : false,
                              IsActive = p.IsActive,
                              Active = p.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsAdminRole = (p.Id == GlobalCode.AdminRoleID) || (p.Id == GlobalCode.TherapistRole),
                              IsDeletable = ((int)UserTypeEnum.Admin == p.Id || (int)UserTypeEnum.Therapist == p.Id)? false : true
                          });

            return result;
        }
        #endregion

        #region Is Role Exists
        /// <summary>
        /// Check if roles exists
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="roleName"></param>
        /// <returns>If role already exists it returns true other wise false.</returns>
        public bool IsRoleExists(int roleID, string roleName)
        {
            bool result = (from r in _Context.Roles where (roleID == 0 || r.Id != roleID) && r.RoleName == roleName select r.Id).Any();
            return result;
        }
        #endregion

        #region Save Roles
        /// <summary>
        /// Save Role
        /// </summary>
        /// <returns>If role is saved is successfully it returns true other wise false</returns>
        public bool SaveChanges(RoleViewModel objRoleViewModel)
        {
            Role objTblRoles = new Role();
            if (objRoleViewModel.Id > 0)
            {
                objTblRoles = _Context.Roles.Where(r => r.Id == objRoleViewModel.Id).FirstOrDefault() ?? new Role();
            }
            objTblRoles.RoleName = objRoleViewModel.RoleName.Trim();
            objTblRoles.Description = !string.IsNullOrWhiteSpace(objRoleViewModel.Description) ? objRoleViewModel.Description.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ").Trim() : objRoleViewModel.Description;
            objTblRoles.IsActive = objRoleViewModel.IsActive;
            if (objTblRoles.Id == 0)
            {
                objTblRoles.CreatedBy = objRoleViewModel.CreatedBy;
                objTblRoles.CreatedOn = objRoleViewModel.CreatedOn;
                _Context.Roles.Add(objTblRoles);
            }
            else
            {
                objTblRoles.UpdatedBy = objRoleViewModel.UpdatedBy;
                objTblRoles.UpdatedOn = objRoleViewModel.UpdatedOn;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get Role Details By RoleID
        /// <summary>
        /// Get role list by roleID
        /// </summary>
        /// <returns>It returns role detail by role id</returns>
        public RoleViewModel GetRoleDetail(int roleID)
        {
            RoleViewModel? role = (from p in _Context.Roles
                                  where p.Id == roleID
                                  select new RoleViewModel
                                  {
                                      Id = p.Id,
                                      RoleName = p.RoleName,
                                      Description = p.Description ?? string.Empty,
                                      IsActive = p.IsActive,
                                      Active = p.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText
                                  }).FirstOrDefault();
            return role ?? new RoleViewModel();
        }
        #endregion

        #region Delete Roles
        /// <summary>
        /// Delete multiple role by int        
        /// </summary>
        /// <returns>If role is deleted successfully it returns true other wise false</returns>
        public bool DeleteRoles(int[] ids)
        {
            _Context.Roles.RemoveRange(_Context.Roles.Where(r => ids.Contains(r.Id)).AsEnumerable());
            if(_Context.SaveChanges()>0)
                return true;
            else return false;
        }
        #endregion

        #region Get Role List
        /// <summary>
        /// Get Role List
        /// </summary>
        /// <returns>Returns list of role for dropdown</returns>
        public List<DropDownBindViewModel> GetRoleDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null)
        {
            List<DropDownBindViewModel> objRoleList = _Context.Roles.Where(e => (isActive.HasValue ? e.IsActive : true)).Select(e => new DropDownBindViewModel { value = e.Id, name = e.RoleName }).OrderBy(a => a.name).ToList();
            return objRoleList;
        }
        #endregion

        #region API Dropdown
        public List<DropDownBindViewModel> GetCreatorRoleList()
        {
            List<DropDownBindViewModel> objRoleList = _Context.Enums.Where(a => a.IsActive == true && a.EnumTypeId == (int)EnumTypes.CreatorType).Select(e => new DropDownBindViewModel { value = e.Id, name = e.EnumValue }).OrderBy(a => a.name).ToList();
            return objRoleList;
        }
        #endregion

        #region Get Role Priviledges List
        /// <summary>
        /// Get Role Priviledges List
        /// </summary>
        /// <returns>It returns role priviledges dropdown list</returns>
        public List<DropDownBindViewModel> GetRolePrivilegesDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null)
        {
            List<DropDownBindViewModel> objRoleList = _Context.Roles.Where(e => (isActive == null || e.IsActive == true) && e.Id != 1).Select(e => new DropDownBindViewModel { value = e.Id, name = e.RoleName }).ToList();
            return objRoleList;
        }
        #endregion
    }
}
