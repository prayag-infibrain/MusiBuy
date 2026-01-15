using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
//using MusiBuy.Common.Enumeration;
using System.Drawing;
using System.Runtime.InteropServices.Marshalling;

namespace MusiBuy.Common.Repositories
{
    public class CommonSettingRepository : ICommonSetting
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public CommonSettingRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get Common Setting
        public CommonSettingViewModel GetCommonSetting()
        {
            return _Context.CommonSettings.Select(x => new CommonSettingViewModel
            {
                Email = x.Email,
                IsSSL = x.IsSsl,
                Password = x.Password,
                Port = Convert.ToString(x.Port),
                Id = x.Id,
                SiteURL = x.SiteUrl,
                MarktingSiteURL = x.MarktingSiteUrl,
                SMTPServer = x.Smtpserver,               
               
            }).FirstOrDefault() ?? new CommonSettingViewModel();
        }
        #endregion


        #region Edit Common Settings Data
        /// <summary>
        /// Update common setting
        /// </summary>
        /// <returns>If common settings updated successfully it returns true other wise it returns false.</returns>
        public bool UpdateCommonSettings(CommonSettingViewModel commonSettingViewModel, int userId)
        {
            CommonSetting commonSettings = _Context.CommonSettings.FirstOrDefault() ?? new CommonSetting();
            commonSettings.Smtpserver = !string.IsNullOrWhiteSpace(commonSettingViewModel.SMTPServer) ? commonSettingViewModel.SMTPServer.Trim() : string.Empty;
            commonSettings.Email = !string.IsNullOrWhiteSpace(commonSettingViewModel.Email) ? commonSettingViewModel.Email.Trim() : string.Empty; 
            commonSettings.Password = !string.IsNullOrWhiteSpace(commonSettingViewModel.Password) ? commonSettingViewModel.Password.Trim() : string.Empty;
            commonSettings.Port = Convert.ToInt32(commonSettingViewModel.Port);
            commonSettings.SiteUrl = !string.IsNullOrWhiteSpace(commonSettingViewModel.SiteURL) ?  commonSettingViewModel.SiteURL.Trim() : string.Empty;
            commonSettings.MarktingSiteUrl = !string.IsNullOrWhiteSpace(commonSettingViewModel.MarktingSiteURL) ?  commonSettingViewModel.MarktingSiteURL.Trim() : string.Empty;
            commonSettings.IsSsl = commonSettingViewModel.IsSSL;
            commonSettings.UpdatedBy = userId;
            commonSettings.UpdatedOn = DateTime.Now;
            
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion

        #region Add Common Settings Data
        /// <summary>
        /// Add Common Settings
        /// </summary>
        /// <param name="commonSettingViewModel"></param>
        /// <param name="userId"></param>
        /// <returns>If common settings added successfully it returns true other wise it returns false.</returns>
        public bool AddCommonSettings(CommonSettingViewModel commonSettingViewModel, int userId)
        {
            CommonSetting commonSettings = new CommonSetting();
            commonSettings.Smtpserver = !string.IsNullOrWhiteSpace(commonSettingViewModel.SMTPServer) ? commonSettingViewModel.SMTPServer.Trim() : string.Empty;
            commonSettings.Email = !string.IsNullOrWhiteSpace(commonSettingViewModel.Email) ? commonSettingViewModel.Email.Trim() : string.Empty;
            commonSettings.Password = !string.IsNullOrWhiteSpace(commonSettingViewModel.Password) ? commonSettingViewModel.Password.Trim() : string.Empty;
            commonSettings.Port = Convert.ToInt32(commonSettingViewModel.Port);
            commonSettings.SiteUrl = !string.IsNullOrWhiteSpace(commonSettingViewModel.SiteURL) ? commonSettingViewModel.SiteURL.Trim() : string.Empty;
            commonSettings.MarktingSiteUrl = !string.IsNullOrWhiteSpace(commonSettingViewModel.MarktingSiteURL) ? commonSettingViewModel.MarktingSiteURL.Trim() : string.Empty;
            commonSettings.IsSsl = commonSettingViewModel.IsSSL;
            commonSettings.CreatedBy = userId;
            commonSettings.CreatedOn = DateTime.UtcNow;
            _Context.CommonSettings.Add(commonSettings);
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion


        #region Get Country Code List
        /// <summary>
        /// Get Role List
        /// </summary>
        /// <returns>Returns list of role for dropdown</returns>
        public List<DropDownBindViewModel> GetCountryCodeDropDownList()
        {
            List<DropDownBindViewModel> objRoleList = _Context.Countries.Where(e => e.IsActive == true).Select(e => new DropDownBindViewModel { value = e.Id, name = "+" + e.Code + " (" +  e.Name + ")" }).OrderBy(a => a.name).ToList();
            return objRoleList;
        }
        #endregion
    }
}