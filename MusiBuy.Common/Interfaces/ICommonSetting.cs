using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ICommonSetting
    {
        CommonSettingViewModel GetCommonSetting();
        bool UpdateCommonSettings(CommonSettingViewModel commonSettingViewModel, int userId);
        bool AddCommonSettings(CommonSettingViewModel commonSettingViewModel, int userId);
        List<DropDownBindViewModel> GetCountryCodeDropDownList();


    }
}
