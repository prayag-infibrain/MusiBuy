using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IRole
    {
        IQueryable<RoleViewModel> GetRoleList(int? roleId, string roleValue);
        bool IsRoleExists(int roleID, string roleName);
        bool SaveChanges(RoleViewModel objRoleViewModel);
        RoleViewModel GetRoleDetail(int roleID);
        bool DeleteRoles(int[] ids);
        List<DropDownBindViewModel> GetRoleDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null);
        List<DropDownBindViewModel> GetRolePrivilegesDropDownList(bool? isActive = null, bool? isAdminRoleExclude = null);
        List<DropDownBindViewModel> GetCreatorRoleList();
    }
}
