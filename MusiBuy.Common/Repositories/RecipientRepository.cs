using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.DB;
using MusiBuy.Common.Models;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Repositories
{
    public class RecipientRepository : IRecipient
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public RecipientRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region For Recipient List, Recipient Search and Recipient Duplication check
        /// <summary>
        /// Get list of recipient based on multiple property filter
        /// </summary>
        /// <returns>List of RecipientViewModel</returns>
        public IEnumerable<RecipientViewModel> GetRecipientViewList(RecipientViewModel recipientViewModel)
        {
            IEnumerable<RecipientViewModel> lstRecipientData = (from x in _Context.Recipients
                                                                where (recipientViewModel.FirstName == string.Empty || recipientViewModel.FirstName == null || x.FirstName.ToUpper().Contains(recipientViewModel.FirstName.ToUpper()))
                                                                && (recipientViewModel.LastName == string.Empty || recipientViewModel.LastName == null || x.LastName.ToUpper().Contains(recipientViewModel.LastName.ToUpper()))
                                                                && (recipientViewModel.Email == string.Empty || recipientViewModel.Email == null || x.Email.ToUpper().Contains(recipientViewModel.Email.ToUpper()))
                                                                && (!recipientViewModel.TemplateId.HasValue || x.TemplateId == recipientViewModel.TemplateId)
                                                                && (recipientViewModel.DuplicateEmail == string.Empty || recipientViewModel.DuplicateEmail == null || x.Email.ToUpper() == recipientViewModel.DuplicateEmail.ToUpper())
                                                                && (recipientViewModel.Id == 0 || x.Id != recipientViewModel.Id)
                                                                select new RecipientViewModel
                                                                {
                                                                    Id = x.Id,
                                                                    TemplateId = x.TemplateId,
                                                                    FirstName = x.FirstName,
                                                                    LastName = x.LastName,
                                                                    Email = x.Email,
                                                                }).ToList();
            return lstRecipientData;
        }
        #endregion

        #region Get recipient data for grid
        /// <summary>
        /// Get recipient data for grid
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>It returns recipient data </returns>
        public IQueryable<RecipientViewModel> GetRecipientList(int? templateID)
        {
            var result = (from x in _Context.Recipients
                          where templateID.HasValue && x.TemplateId == templateID
                          orderby x.FirstName
                          select new RecipientViewModel
                          {
                              Id = x.Id,
                              TemplateId = x.TemplateId,
                              FirstName = x.FirstName,
                              LastName = x.LastName,
                             IsActive=x.IsDeleted,
                              Active = x.IsDeleted ? GlobalCode.InActiveText:GlobalCode.ActiveText,
                              Email = x.Email,
                          });

            return result;
        }
        #endregion

        #region Save Recipient
        /// <summary>
        /// save recipient
        /// </summary>
        /// <returns>If receipient saved successfully it returns true other false</returns>
        public bool SaveRecipient(RecipientViewModel recipientViewModel, int templateID, int userId)
        {
            Recipient tbl = new Recipient();
            if (recipientViewModel.Id > 0)
            {
                tbl = _Context.Recipients.Where(r => r.Id == recipientViewModel.Id).FirstOrDefault() ?? new Recipient();
            }
            tbl.FirstName = !string.IsNullOrWhiteSpace(recipientViewModel.FirstName) ? recipientViewModel.FirstName.Trim() : string.Empty;
            tbl.LastName = !string.IsNullOrWhiteSpace(recipientViewModel.LastName)? recipientViewModel.LastName.Trim(): string.Empty;
            tbl.Email = !string.IsNullOrWhiteSpace(recipientViewModel.Email) ? recipientViewModel.Email.Trim() : string.Empty;
            if (tbl.Id == 0)
            {
                tbl.TemplateId = templateID;
                tbl.CreatedBy = userId;
                tbl.CreatedOn = DateTime.UtcNow;
                _Context.Recipients.Add(tbl);
            }
            else
            {
                tbl.TemplateId = recipientViewModel.TemplateId.HasValue ? recipientViewModel.TemplateId.Value : 0;
                tbl.UpdatedBy = userId;
                tbl.UpdatedOn = DateTime.UtcNow;
                tbl.IsDeleted = recipientViewModel.IsActive;
            }
            if (_Context.SaveChanges()>0)
                return true;
            return false;
        }
        #endregion

        #region To Delete Recipient
        /// <summary>
        /// Delete multiple recipient by recipientID
        /// </summary>
        /// <returns>RecipientViewModel</returns>
        public bool DeleteRecipient(int[] recipientID)
        {
            if (recipientID.Length > 0)
            {
                _Context.Recipients.RemoveRange(_Context.Recipients.Where(c => recipientID.Contains(c.Id)).AsEnumerable());
                if (_Context.SaveChanges() > 0)
                    return true;
              
            }
            return false;
        }
        #endregion

        #region To Get Recipient Single Record
        /// <summary>
        /// Get single recipient by recipientID
        /// </summary>
        /// <returns>Recipients</returns>
        public RecipientViewModel GetRecipientById(int recipientID)
        {
            RecipientViewModel? recipientViewModel = _Context.Recipients.Where(x => x.Id == recipientID).Select(x => new RecipientViewModel
            {
                Id = x.Id,
                TemplateId = x.TemplateId,
                TemplateName = x.Template != null ? x.Template.TemplateName : string.Empty,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsActive=x.IsDeleted,
                  Active = x.IsDeleted ? GlobalCode.InActiveText : GlobalCode.ActiveText,
            }).FirstOrDefault();

            return recipientViewModel ?? new RecipientViewModel();
        }
        #endregion

        #region To Get Tempalte ID
        /// <summary>
        /// Get Template ID by recipientID
        /// </summary>
        /// <returns>It returns template id by receipient id</returns>
        public int? GetTemplateIdByRecipientId(int recipientID)
        {
            int templateID = _Context.Recipients.Where(x => x.Id == recipientID).Select(x => x.TemplateId).FirstOrDefault();
            return templateID;
        }
        #endregion

        #region Is Recipient exists
        /// <summary>
        /// Check Recipient exist
        /// </summary>
        /// <returns>bool</returns>
        public bool IsRecepientExists(RecipientViewModel recipientViewModel)
        {
            bool IsExists = (from x in _Context.Recipients
                             where (recipientViewModel.FirstName == string.Empty || recipientViewModel.FirstName == null || x.FirstName.ToUpper().Contains(recipientViewModel.FirstName.ToUpper()))
                             && (recipientViewModel.LastName == string.Empty || recipientViewModel.LastName == null || x.LastName.ToUpper().Contains(recipientViewModel.LastName.ToUpper()))
                             && (recipientViewModel.Email == string.Empty || recipientViewModel.Email == null || x.Email.ToUpper().Contains(recipientViewModel.Email.ToUpper()))
                             && (!recipientViewModel.TemplateId.HasValue || x.TemplateId == recipientViewModel.TemplateId)
                             && (recipientViewModel.DuplicateEmail == string.Empty || recipientViewModel.DuplicateEmail == null || x.Email.ToUpper() == recipientViewModel.DuplicateEmail.ToUpper())
                             && (recipientViewModel.Id == 0 || x.Id != recipientViewModel.Id)
                             select new RecipientViewModel
                             {
                                 Id = x.Id,
                                 TemplateId = x.TemplateId,
                                 FirstName = x.FirstName,
                                 LastName = x.LastName,
                                 Email = x.Email,
                             }).Any();
            return IsExists;
        }
        #endregion
    }
}