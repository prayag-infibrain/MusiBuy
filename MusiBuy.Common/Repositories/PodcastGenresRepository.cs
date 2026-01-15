using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Repositories
{
    public class PodcastGenresRepository :IPodcastGenres
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public PodcastGenresRepository(MusiBuyDB_Connection context)
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
        public IQueryable<PodcastGenresViewModel> GetPodcastGenresList(int userId, string searchValue)
        {
            var result = (from u in _Context.PodcastGenres
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.PodcastGenresName.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.SortOrder
                          select new PodcastGenresViewModel
                          {
                              Id = u.Id,
                              PodcastGenresName = u.PodcastGenresName,
                              Description = u.Description,
                              ProvidersNumber = u.ProvidersNumber,
                              SortOrder = u.SortOrder,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsCurrentAdminUser = userId == u.Id ? true : false
                          });

            return result;
        }
        #endregion

        #region Check PodcastGenres Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PodcastGenresName"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        public bool IsPodcastGenresExit(int Id, string PodcastGenresName)
        {
            return (from x in _Context.PodcastGenres where (string.IsNullOrWhiteSpace(PodcastGenresName) || x.PodcastGenresName == PodcastGenresName) && (Id == 0 || x.Id != Id) select x.Id).Any();
        }
        #endregion

        #region Save PodcastGenres
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PodcastGenresViewModel"></param>
        /// <returns></returns>
        public bool Save(PodcastGenresViewModel objmodel)
        {
            var data = new PodcastGenre();
            if (objmodel.Id > 0)
            {
                data = _Context.PodcastGenres.FirstOrDefault(x => x.Id == objmodel.Id) ?? new PodcastGenre();
            }
            data.PodcastGenresName = (objmodel.PodcastGenresName ?? string.Empty).Trim();
            data.Description = (objmodel.Description ?? string.Empty).Trim();
            data.ProvidersNumber = (objmodel.ProvidersNumber ?? string.Empty).Trim();
            data.SortOrder = objmodel.SortOrder;
            data.IsActive = objmodel.IsActive;

            if (data.Id == 0)
            {
                data.CreatedOn = DateTime.Now;
                data.CreatedBy = objmodel.CreatedBy;
                _Context.PodcastGenres.Add(data);
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

        #region Get PodcastGenres by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public PodcastGenresViewModel GetPodcastGenresByID(int userID)
        {
            PodcastGenresViewModel? validateUser = (from u in _Context.PodcastGenres
                                                           select new PodcastGenresViewModel
                                                           {
                                                               Id = u.Id,
                                                               PodcastGenresName = u.PodcastGenresName,
                                                               Description = u.Description,
                                                               ProvidersNumber = u.ProvidersNumber,
                                                               SortOrder = u.SortOrder,
                                                               IsActive = u.IsActive,
                                                               Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                                                           }).FirstOrDefault();
            return validateUser;
        }
        #endregion

        #region Delete multiple PodcastGenres
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeletePodcastGenres(int[] ids)
        {
            _Context.PodcastGenres.RemoveRange(_Context.PodcastGenres.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion     
    }
}
