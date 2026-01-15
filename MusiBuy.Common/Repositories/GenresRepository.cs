using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Newtonsoft.Json.Linq;
namespace MusiBuy.Common.Repositories
{
    public class GenresRepository : IGenres
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public GenresRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get Token Plan data for grid
        /// <summary>
        /// Get Genres data for grid
        /// </summary>
        /// <param name="GenresId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns Genres list</returns>
        public IQueryable<GenresViewModel> GetGenresList(string searchValue)
        {
            var result = (from u in _Context.Genres
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.GenreName.ToLower().Contains(searchValue.ToLower()) ||
                          u.Description.ToLower().Contains(searchValue.ToLower()))
                          orderby u.CreatedOn descending
                          select new GenresViewModel
                          {
                              Id = u.Id,
                              GenreName = u.GenreName,
                              Description = u.Description,
                              CountryName = u.Country.Name,
                              RegionName = u.Region.Name,
                              IsActive  = u.IsActive
                          });

            return result;
        }
        #endregion

        #region Check Genres Exists 
        /// <summary>
        /// Check Genres Exists
        /// </summary>
        /// <param name="GenresID"></param>
        /// <param name="GenresName"></param>
        /// <returns>If Genres already exists it returns true other wise false</returns>
        public bool IsGenresExists(int GenresID, string GenresName)
        {
            return (from x in _Context.Genres where (x.GenreName == GenresName) && (GenresID == 0 || x.Id != GenresID) select x.Id).Any();
        }
        #endregion


        #region Get Genres Details by GenresID
        /// <summary>
        /// Get single Genres by GenresID
        /// </summary>
        /// <returns>It returns Genres details by id </returns>
        public GenresViewModel GetGenresDetailsByID(int GenresID)
        {
            GenresViewModel? validateGenres = (from u in _Context.Genres
                                                     where u.Id == GenresID
                                                     select new GenresViewModel
                                                     {
                                                         Id = u.Id,
                                                         GenreName = u.GenreName,
                                                         Description = u.Description,
                                                         CountryId = u.CountryId,
                                                         CountryName = u.Country.Name,
                                                         RegionId = u.RegionId,
                                                         RegionName = u.Region.Name,
                                                         IsActive = u.IsActive
                                                     }).FirstOrDefault();
            return validateGenres;
        }
        #endregion


        #region Delete multiple Genres
        /// <summary>
        /// Delete multiple Genres
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteGenress(int[] ids)
        {
            _Context.Genres.RemoveRange(_Context.Genres.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save Genres
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GenresViewModel"></param>
        /// <returns></returns>
        public bool Save(GenresViewModel GenresViewModel)
        {
            var GenresData = new Genre();
            if (GenresViewModel.Id > 0)
            {
                GenresData = _Context.Genres.FirstOrDefault(x => x.Id == GenresViewModel.Id) ?? new Genre();
            }
            GenresData.GenreName = GenresViewModel.GenreName;
            GenresData.Description = GenresViewModel.Description;
            GenresData.CountryId = GenresViewModel.CountryId;
            GenresData.RegionId = GenresViewModel.RegionId;
            if (GenresData.Id == 0)
            {
                GenresData.IsActive = true;
                GenresData.CreatedBy = GenresViewModel.CreatedBy;
                GenresData.CreatedOn = DateTime.Now;
                _Context.Genres.Add(GenresData);
            }
            else
            {
                GenresData.UpdatedBy = GenresViewModel.UpdatedBy;
                GenresData.UpdatedOn = DateTime.Now;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get Genres Count for Dashboard
        /// <summary>
        /// Get Genres data for grid
        /// </summary>
        /// <param name="GenresId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns Genres list</returns>
        public int GetGenresCount()
        {
            return _Context.Genres.Count();
        }
        #endregion

        #region Get Genres List
        /// <summary>
        /// Get Genres List
        /// </summary>
        /// <returns>Returns list of All Genres for dropdown</returns>
        public List<DropDownBindViewModel> GetGenresDropDownList(bool IsActive = true)
        {
            List<DropDownBindViewModel> objPostList = _Context.Genres.Where(e => e.IsActive == true).Select(e => new DropDownBindViewModel { value = e.Id, name = e.GenreName}).ToList();
            return objPostList;
        }
        #endregion
    }
}
