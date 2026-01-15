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
    public class MusicProducersRepository  :IMusicProducers
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public MusicProducersRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get MusicProducers for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<MusicProducersViewModel> GetMusicProducersList(int userId, string searchValue)
        {
            var result = (from u in _Context.MusicProducers
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.ProducerType.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.SortOrder
                          select new MusicProducersViewModel
                          {
                              Id = u.Id,
                              ProducerType= u.ProducerType,
                              PrimaryExpertise = u.PrimaryExpertise,
                              GenresSpecialized = u.GenresSpecialized,
                              KeyContributions = u.KeyContributions,
                              SortOrder = u.SortOrder,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsCurrentAdminUser = userId == u.Id ? true : false
                          });

            return result;
        }
        #endregion

        #region Check MusicProducers Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PodcastGenresName"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        public bool IsMusicProducersExists(int Id, string ProducerType)
        {
            return (from x in _Context.MusicProducers where (string.IsNullOrWhiteSpace(ProducerType) || x.ProducerType == ProducerType) && (Id == 0 || x.Id != Id) select x.Id).Any();
        }
        #endregion

        #region Save PodcastGenres
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MusicProducersViewModel"></param>
        /// <returns></returns>
        public bool Save(MusicProducersViewModel objmodel)
        {
            var data = new MusicProducer();
            if (objmodel.Id > 0)
            {
                data = _Context.MusicProducers.FirstOrDefault(x => x.Id == objmodel.Id) ?? new MusicProducer();
            }
            data.ProducerType = (objmodel.ProducerType ?? string.Empty).Trim();
            data.PrimaryExpertise = (objmodel.PrimaryExpertise ?? string.Empty).Trim();
            data.GenresSpecialized = (objmodel.GenresSpecialized ?? string.Empty).Trim();
            data.KeyContributions = (objmodel.KeyContributions ?? string.Empty).Trim();
            data.SortOrder = objmodel.SortOrder;
            data.IsActive = objmodel.IsActive;

            if (data.Id == 0)
            {
                data.CreatedOn = DateTime.Now;
                data.CreatedBy = objmodel.CreatedBy;
                _Context.MusicProducers.Add(data);
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
        public MusicProducersViewModel GetMusicProducersByID(int userID)
        {
            MusicProducersViewModel? validateUser = (from u in _Context.MusicProducers
                                                    select new MusicProducersViewModel
                                                    {
                                                        Id = u.Id,
                                                        ProducerType = u.ProducerType,
                                                        PrimaryExpertise = u.PrimaryExpertise,
                                                        GenresSpecialized = u.GenresSpecialized,
                                                        KeyContributions = u.KeyContributions,
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
        public bool DeleteMusicProducers(int[] ids)
        {
            _Context.MusicProducers.RemoveRange(_Context.MusicProducers.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion     
    }
}
