using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusiBuy.Common.Repositories
{
    public class ContentManagementRepository : IContentManagement
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        MusiBuyDB_Connection _Context;
        public ContentManagementRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Save
        /// <summary>
        /// Save Content
        /// </summary>
        /// <param name="validateCMS"></param>
        /// <returns>If content is saved successfully it returns true other wise false.</returns>
        public bool Save(ContentManagementsViewModel validateCMS)
        {
            Content objTblCMS = new Content();
            if (validateCMS.Id > 0)
            {
                objTblCMS = _Context.Contents.Where(r => r.Id == validateCMS.Id).FirstOrDefault() ?? new Content();
            }
            objTblCMS.PageId = validateCMS.PageId;
            objTblCMS.Title = !string.IsNullOrWhiteSpace(validateCMS.Title) ? validateCMS.Title.Trim() : string.Empty;
            objTblCMS.Content1 = !string.IsNullOrWhiteSpace(validateCMS.Content) ? validateCMS.Content.Trim() : string.Empty;
            objTblCMS.IsActive = validateCMS.IsActive;
            if (objTblCMS.Id == 0)
            {
                objTblCMS.CreatedBy = validateCMS.CreatedBy;
                objTblCMS.CreatedOn = DateTime.UtcNow;
                _Context.Contents.Add(objTblCMS);
            }
            else
            {
                objTblCMS.UpdatedBy = validateCMS.UpdatedBy;
                objTblCMS.UpdatedOn = DateTime.UtcNow;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion

        #region Get Content Managements Content Details ByID
        /// <summary>
        /// Get content details by id
        /// </summary>
        /// <param name="PageId"></param>
        /// <returns></returns>
        public ContentManagementsViewModel GetContentManagementsContentByID(int PageId)
        {
            return (from u in _Context.Contents
                    where u.PageId == PageId
                    select new ContentManagementsViewModel
                    {
                        Id = u.Id,
                        Title = u.Title,
                        PageId = u.PageId,
                        Page = u.PageId > 0 ? u.Page.EnumValue : "",
                        Content = u.Content1,
                        IsActive = u.IsActive ? u.IsActive : false,
                        Active = (u.IsActive == true) ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                    }).FirstOrDefault() ?? new ContentManagementsViewModel();
        }
        #endregion
    }
}
