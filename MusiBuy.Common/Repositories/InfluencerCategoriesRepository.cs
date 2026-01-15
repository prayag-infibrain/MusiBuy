using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Repositories
{
    public class InfluencerCategoriesRepository :IInfluencerCategories
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public InfluencerCategoriesRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get InfluencerCategories for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<InfluencerCategoriesViewModel> GetInfluencerCategoriesList(int userId, string searchValue)
        {
            var result = (from u in _Context.InfluencerCategories
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.InfluencerTypes.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.SortOrder
                          select new InfluencerCategoriesViewModel
                          {
                              Id = u.Id,
                              InfluencerTypes = u.InfluencerTypes,
                              Criteria = u.Criteria,
                              EstimatedNumber = u.EstimatedNumber,
                              SortOrder = u.SortOrder,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsCurrentAdminUser = userId == u.Id ? true : false
                          });

            return result;
        }
        #endregion

        #region Check InfluencerCategoriesExists Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="InfluencerTypes"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        public bool IsInfluencerCategoriesExists(int Id, string InfluencerTypes)
        {
            return (from x in _Context.InfluencerCategories where (string.IsNullOrWhiteSpace(InfluencerTypes) || x.InfluencerTypes == InfluencerTypes) && (Id == 0 || x.Id != Id) select x.Id).Any();
        }
        #endregion

        #region Save InfluencerCategories
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PodcastGenresViewModel"></param>
        /// <returns></returns>
        public bool Save(InfluencerCategoriesViewModel objmodel)
        {
            var data = new InfluencerCategory();
            if (objmodel.Id > 0)
            {
                data = _Context.InfluencerCategories.FirstOrDefault(x => x.Id == objmodel.Id) ?? new InfluencerCategory();
            }
            data.InfluencerTypes = (objmodel.InfluencerTypes ?? string.Empty).Trim();
            data.Criteria = (objmodel.Criteria ?? string.Empty).Trim();
            data.EstimatedNumber = (objmodel.EstimatedNumber ?? string.Empty).Trim();
            data.SortOrder = objmodel.SortOrder;
            data.IsActive = objmodel.IsActive;

            if (data.Id == 0)
            {
                data.CreatedOn = DateTime.Now;
                data.CreatedBy = objmodel.CreatedBy;
                _Context.InfluencerCategories.Add(data);
            }
            else
            {
                data.UpdatedOn = DateTime.Now;
                data.UpdatedBy = objmodel.UpdatedBy;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get InfluencerCategories by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public InfluencerCategoriesViewModel GetInfluencerCategoriesByID(int userID)
        {
            InfluencerCategoriesViewModel? validateUser = (from u in _Context.InfluencerCategories
                                           select new InfluencerCategoriesViewModel
                                           {
                                               Id = u.Id,
                                               InfluencerTypes = u.InfluencerTypes,
                                               Criteria = u.Criteria,
                                               EstimatedNumber = u.EstimatedNumber,
                                               SortOrder = u.SortOrder,
                                               IsActive = u.IsActive,
                                               Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                                           }).FirstOrDefault();
            return validateUser;
        }
        #endregion

        #region Delete multiple InfluencerCategories
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteUsers(int[] ids)
        {
            _Context.InfluencerCategories.RemoveRange(_Context.InfluencerCategories.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion     
    }
}
