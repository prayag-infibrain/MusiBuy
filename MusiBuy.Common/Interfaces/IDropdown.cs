using MusiBuy.Common.DB;
using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IDropdown
    {
        List<DropDownBindViewModel> GetCountryDropDownList();
        List<DropDownBindViewModel> GetRegionDropDownList();

        #region API Dropdown List
        List<RoleViewModel> GetRoleList();
        List<EnumViewModel> GetEventTypeList();
        List<EnumViewModel> GetEnumListByType(int EnumTypeId);
        List<Country> GetCountry();
        List<GenresViewModel> GetGenre();
        #endregion
    }
}
