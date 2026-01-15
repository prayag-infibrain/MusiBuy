using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MusiBuy.Common.Repositories
{
    public class TemplateRepository : ITemplate
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        public readonly ICommonSetting _commonSetting;

        public TemplateRepository(MusiBuyDB_Connection context, ICommonSetting commonSetting)
        {
            this._Context = context;
            _commonSetting = commonSetting;
        }
        #endregion

        #region Get user data for grid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public IQueryable<TemplateViewModel> GetTemplateList(string templateName)
        {
            var result = (from p in _Context.EmailTemplates
                          where (!string.IsNullOrWhiteSpace(templateName) ? p.TemplateName.Contains(templateName) : true == true) ||
                          (!string.IsNullOrWhiteSpace(templateName) ? (p.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(templateName) : true)
                          select new TemplateViewModel
                          {
                              Id = p.Id,
                              TemplateName =p.TemplateName, 
                              IsActive = p.IsActive,
                              Active = p.IsActive == false ? GlobalCode.InActiveText: GlobalCode.ActiveText
                          });

            return result;
        }
        #endregion

        #region Get Email Template By Template Name
        /// <summary>
        /// Get Email Template By Template Name
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns>It returns template details by template name.</returns>
        public TemplateViewModel GetEmailTemplateByName(string templateName)
            {
            TemplateViewModel obj = new TemplateViewModel();
            try
            {
                var Templates = (from p in _Context.EmailTemplates
                                 where p.TemplateName.ToUpper() == templateName.ToUpper()
                                 && p.IsActive == true
                                 select p).FirstOrDefault();
                List<RecipientViewModel> lstRecipient = _Context.Recipients.Where(x => x.TemplateId == Templates.Id && !x.IsDeleted).Select(x => new RecipientViewModel { FirstName = x.FirstName, LastName = x.LastName, Email = x.Email, Id = x.Id }).ToList();
                if (Templates != null)
                {
                    var recipients = string.Join(",", _Context.Recipients.Where(p => p.TemplateId == Templates.Id && !p.IsDeleted).Select(p => p.Email).ToArray());

                    obj.Id = Templates.Id;
                    obj.TemplateName = Templates.TemplateName;
                    obj.Subject = Templates.Subject;
                    obj.TemplateContent = Templates.TemplateContent;
                    obj.Recipients = recipients;
                    obj.LstRecipients = lstRecipient;
                }
            }
            catch (Exception)
            {
                return new TemplateViewModel();
            }
            return obj;
        }
        #endregion

     
        #region For Template Detail
        /// <summary>
        /// Get template by template ID
        /// </summary>
        /// <returns>TemplateViewModel</returns>
        public TemplateViewModel GetTemplateByID(int templateID)
        {
            TemplateViewModel? templateViewModel = _Context.EmailTemplates.Where(p => p.Id == templateID).Select(p => new TemplateViewModel
            {
                Id = p.Id,
                TemplateName = p.TemplateName,
                Subject = p.Subject,
                IsActive = p.IsActive,
                TemplateContent = p.TemplateContent,
                Active = p.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText
            }).FirstOrDefault();

            return templateViewModel ?? new TemplateViewModel();
        }
        #endregion

        #region To Sentence Case
        /// <summary>
        /// Convert to sentence case
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns>It returns template name in sentence case </returns>
        public string ToSentenceCase(string templateName)
        {
            string spaced = Regex.Replace(templateName, "(\\B[A-Z])", " $1");
            spaced = spaced.ToLower();
            return char.ToUpper(spaced[0]) + spaced.Substring(1).ToLower();
        }
        #endregion

        #region Save Template Data
        /// <summary>
        /// Save template by template ID
        /// </summary>
        /// <returns>If email template is saved succesfully it returns true other wise it returns false.</returns>
        public bool SaveTemplateData(TemplateViewModel templateViewModel)
        {
            EmailTemplate tbl = new EmailTemplate();
            if (templateViewModel.Id.HasValue && templateViewModel.Id > 0)
            {
                tbl = _Context.EmailTemplates.Where(p => p.Id == templateViewModel.Id).FirstOrDefault() ?? new EmailTemplate();
            }
            tbl.TemplateName = templateViewModel.TemplateName.Trim() ?? string.Empty;
            tbl.Subject = templateViewModel.Subject.Trim() ?? string.Empty;
            tbl.TemplateContent = templateViewModel.TemplateContent.Trim() ?? string.Empty;

            if (templateViewModel.Id == 0)
            {
                tbl.IsActive = true;
                tbl.CreatedBy = templateViewModel.CreatedBy;
                tbl.CreatedOn = DateTime.UtcNow;
                _Context.EmailTemplates.Add(tbl);
            }
            else
            {
                tbl.IsActive = templateViewModel.IsActive;
                tbl.UpdatedBy = templateViewModel.UpdatedBy;
                tbl.UpdatedOn = DateTime.UtcNow;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;

        }
        #endregion

        #region Delete Master Keyword Data
        /// <summary>
        /// Delete template by template ID
        /// </summary>
        /// <returns>TemplateViewModel</returns>
        public bool DeleteTemplate(int[] ids)
        {
            if (ids.Length > 0)
            {
                _Context.EmailTemplates.RemoveRange(_Context.EmailTemplates.Where(k => ids.Contains(k.Id)).AsEnumerable());
                if(_Context.SaveChanges()>0)
                    return true;
            }
            return false;
        }
        #endregion

        #region To Get Tempelate Name
        /// <summary>
        /// Get Template name by templateID
        /// </summary>
        /// <returns>It returns template name by template id</returns>
        public string GetTemplateNameById(int templateID)
        {
            string? tempName = _Context.EmailTemplates.Where(p => p.Id == templateID).Select(x => x.TemplateName).FirstOrDefault();
            return tempName ?? string.Empty;
        }
        #endregion

    }
}
