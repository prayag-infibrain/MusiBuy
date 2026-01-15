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
    public class ProducerExpertiseRepository : IProducerExpertise
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public ProducerExpertiseRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get ProducerExpertise for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<ProducerExpertiseViewModel> GetProducerExpertiseList(int userId, string searchValue)
        {
            var result = (from u in _Context.ProducerExpertises
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.ProducerExpertiseName.ToLower().Contains(searchValue.ToLower()) ||
                          (!string.IsNullOrWhiteSpace(searchValue) ? (u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText).Contains(searchValue) : true))
                          orderby u.SortOrder
                          select new ProducerExpertiseViewModel
                          {
                              Id = u.Id,
                              ProducerExpertiseName = u.ProducerExpertiseName,
                              Description = u.Description,
                              SortOrder = u.SortOrder,
                              IsActive = u.IsActive,
                              Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                              IsCurrentAdminUser = userId == u.Id ? true : false
                          });

            return result;
        }
        #endregion

        #region Check IsProducerExpertiseExists Exists 
        /// <summary>
        /// Check User Exists
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProducerExpertiseName"></param>
        /// <returns>If user already exists it returns true other wise false</returns>
        public bool IsProducerExpertiseExists(int Id, string ProducerExpertiseName)
        {
            return (from x in _Context.ProducerExpertises where (string.IsNullOrWhiteSpace(ProducerExpertiseName) || x.ProducerExpertiseName == ProducerExpertiseName) && (Id == 0 || x.Id != Id) select x.Id).Any();
        }
        #endregion

        #region Save InfluencerCategories
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PodcastGenresViewModel"></param>
        /// <returns></returns>
        public bool Save(ProducerExpertiseViewModel objmodel)
        {
            var data = new ProducerExpertise();
            if (objmodel.Id > 0)
            {
                data = _Context.ProducerExpertises.FirstOrDefault(x => x.Id == objmodel.Id) ?? new ProducerExpertise();
            }
            data.ProducerExpertiseName = (objmodel.ProducerExpertiseName ?? string.Empty).Trim();
            data.Description = (objmodel.Description ?? string.Empty).Trim();
            data.SortOrder = objmodel.SortOrder;
            data.IsActive = objmodel.IsActive;

            if (data.Id == 0)
            {
                data.CreatedOn = DateTime.Now;
                data.CreatedBy = objmodel.CreatedBy;
                _Context.ProducerExpertises.Add(data);
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

        #region Get ProducerExpertise by Id
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public ProducerExpertiseViewModel GeProducerExpertiseByID(int ID)
        {
            ProducerExpertiseViewModel? validateUser = (from u in _Context.ProducerExpertises
                                                           select new ProducerExpertiseViewModel
                                                           {
                                                               Id = u.Id,
                                                               ProducerExpertiseName = u.ProducerExpertiseName,
                                                               Description = u.Description,
                                                               SortOrder = u.SortOrder,
                                                               IsActive = u.IsActive,
                                                               Active = u.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                                                           }).FirstOrDefault();
            return validateUser;
        }
        #endregion

        #region Delete multiple ProducerExpertise
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteProducerExpertise(int[] ids)
        {
            _Context.ProducerExpertises.RemoveRange(_Context.ProducerExpertises.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion     
    }
}
