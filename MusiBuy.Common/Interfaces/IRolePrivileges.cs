using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IRolePrivileges
    {
        void AddRolePrivileges(List<RolePrivilegesViewModel> rolePrivilegesList);
        void DeleteRolePrivilegesByRoleID(int roleID);
        List<RolePrivilegesViewModel> GetRolePrivilegesByRoleId(int roleId);
    }
}
