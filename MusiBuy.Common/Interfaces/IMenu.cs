using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IMenu
    {
        List<MenuItemViewModel> GetMenuList(bool? isActive = null);
    }
}
