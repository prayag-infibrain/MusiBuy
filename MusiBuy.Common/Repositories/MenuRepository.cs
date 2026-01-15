using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusiBuy.Common.Repositories
{
    public class MenuRepository : IMenu
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;

        public MenuRepository(MusiBuyDB_Connection context)
        {
            _Context = context;
        }
        #endregion

        #region Get Menu List
        /// <summary>
        /// Get menu list
        /// </summary>
        /// <returns>List<MenuItemViewModel></returns>
        public List<MenuItemViewModel> GetMenuList(bool? isActive = null)
        {
            List<MenuItemViewModel> menuList = (from x in _Context.MenuItems
                                                where x.IsActive
                                                select new MenuItemViewModel
                                                {
                                                    Id = x.Id,
                                                    MenuItemName = x.MenuItemName,
                                                    MenuItemController = x.MenuItemController,
                                                    MenuItemView = x.MenuItemView,
                                                    ParentId = x.ParentId,
                                                    ImageName = x.ImageName,
                                                    SortOrder = x.SortOrder,
                                                    IsActive = x.IsActive
                                                }).ToList();
            return menuList;
        }
        #endregion
    }
}
