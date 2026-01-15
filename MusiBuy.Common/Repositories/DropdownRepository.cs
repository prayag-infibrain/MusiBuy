using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Repositories
{
    public class DropdownRepository : IDropdown
    {
        #region Member Declaration
        public MusiBuyDB_Connection _context;
        public DropdownRepository(MusiBuyDB_Connection context)
        {
            this._context = context;
        }
        #endregion


        #region Get Customer/Party List
        public List<DropDownBindViewModel> GetCountryDropDownList()
        {
            List<DropDownBindViewModel> objUserTypeList = _context.Countries.Where(x => x.IsActive == true).Select(e => new DropDownBindViewModel { value = e.Id, name = e.Name, key = e.Region.Name }).ToList();
            return objUserTypeList;
        }
        #endregion

        #region Get Region List
        public List<DropDownBindViewModel> GetRegionDropDownList()
        {
            List<DropDownBindViewModel> objUserTypeList = _context.Regions.Select(e => new DropDownBindViewModel { value = e.Id, name = e.Name }).ToList();
            return objUserTypeList;
        }
        #endregion


        #region API Dropdown
        public List<RoleViewModel> GetRoleList()
        {
            return _context.Enums.Where(a => a.IsActive == true && a.EnumTypeId == (int)EnumTypes.CreatorType).Select(a => new RoleViewModel
            {
                Id = a.Id,
                RoleName = a.EnumValue,
                Description = a.Description,
                IsActive = a.IsActive,
            }).ToList();
        }

        public List<EnumViewModel> GetEventTypeList()
        {
            return _context.Enums.Where(a => a.IsActive == true && a.EnumTypeId == (int)EnumTypes.EventType).Select(a => new EnumViewModel
            {
                Id = a.Id,
                EnumValue = a.EnumValue,
                Description = a.Description,
                IsActive = a.IsActive,
            }).ToList();
        }

        public List<EnumViewModel> GetEnumListByType(int EnumTypeId)
        {
            return _context.Enums.Where(a => a.IsActive == true && a.EnumTypeId == EnumTypeId).Select(a => new EnumViewModel
            {
                Id = a.Id,
                EnumValue = a.EnumValue,
                Description = a.Description,
                IsActive = a.IsActive,
            }).ToList();
        }

        public List<Country> GetCountry()
        {
            return _context.Countries.Where(a => a.IsActive == true).Select(a => new Country
            {
                Id = a.Id,
                Name = a.Name,
                Code= a.Code,
                CountryShortCode = a.CountryShortCode,
                IsActive = a.IsActive,

            }).ToList();
        }

        public List<GenresViewModel> GetGenre()
        {
            return _context.Genres.Where(a => a.IsActive == true).Select(a => new GenresViewModel
            {
                Id = a.Id,
                GenreName = a.GenreName,
                Description = a.Description,
                CountryId = a.CountryId,
                CountryName = a.Country.Name,
                IsActive = a.IsActive,
            }).ToList();
        }
        #endregion
    }
}
