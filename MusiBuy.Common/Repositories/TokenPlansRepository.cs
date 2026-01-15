
using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Newtonsoft.Json.Linq;
namespace MusiBuy.Common.Repositories
{
    public class TokenPlansRepository : ITokenPlans
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;

        public TokenPlansRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Get Token Plan data for grid
        /// <summary>
        /// Get TokenPlan data for grid
        /// </summary>
        /// <param name="TokenPlanId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns TokenPlan list</returns>
        public IQueryable<TokenPlanViewModel> GetTokenPlanList(string searchValue)
        {
            var result = (from u in _Context.TokenPlans
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          u.TokenType.EnumValue.ToLower().Contains(searchValue.ToLower()) ||
                          u.Description.ToLower().Contains(searchValue.ToLower()))
                          orderby u.TokenQuantity
                          select new TokenPlanViewModel
                          {
                              Id = u.Id,
                              TokenTypeName = u.TokenType.EnumValue,
                              TokenQuantity = u.TokenQuantity,
                              Price = u.Price,
                              Description = u.Description
                          });

            return result;
        }
        #endregion

        #region Check TokenPlan Exists 
        /// <summary>
        /// Check TokenPlan Exists
        /// </summary>
        /// <param name="TokenPlanID"></param>
        /// <param name="TokenPlanName"></param>
        /// <returns>If TokenPlan already exists it returns true other wise false</returns>
        public bool IsTokenPlanExists(int TokenPlanID, int TokenTypeId)
        {
            return (from x in _Context.TokenPlans where (x.TokenTypeId == TokenTypeId) && (TokenPlanID == 0 || x.Id != TokenPlanID) select x.Id).Any();
        }
        #endregion


        #region Get TokenPlan Details by TokenPlanID
        /// <summary>
        /// Get single TokenPlan by TokenPlanID
        /// </summary>
        /// <returns>It returns TokenPlan details by id </returns>
        public TokenPlanViewModel GetTokenPlanDetailsByID(int TokenPlanID)
        {
            TokenPlanViewModel? validateTokenPlan = (from u in _Context.TokenPlans
                                                     where u.Id == TokenPlanID
                                                     select new TokenPlanViewModel
                                                     {
                                                         Id = u.Id,
                                                         TokenTypeId = u.TokenTypeId,
                                                         TokenTypeName = u.TokenType.EnumValue,
                                                         TokenQuantity = u.TokenQuantity,
                                                         Price = u.Price,
                                                         Description = u.Description,
                                                     }).FirstOrDefault();
            return validateTokenPlan;
        }
        #endregion


        #region Delete multiple TokenPlans
        /// <summary>
        /// Delete multiple TokenPlans
        /// </summary>
        /// <returns>void</returns>
        public bool DeleteTokenPlans(int[] ids)
        {
            _Context.TokenPlans.RemoveRange(_Context.TokenPlans.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save TokenPlan
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TokenPlanViewModel"></param>
        /// <returns></returns>
        public bool Save(TokenPlanViewModel TokenPlanViewModel)
        {
            var TokenPlanData = new TokenPlan();
            if (TokenPlanViewModel.Id > 0)
            {
                TokenPlanData = _Context.TokenPlans.FirstOrDefault(x => x.Id == TokenPlanViewModel.Id) ?? new TokenPlan();
            }
            TokenPlanData.TokenTypeId = TokenPlanViewModel.TokenTypeId;
            TokenPlanData.TokenQuantity = TokenPlanViewModel.TokenQuantity;
            TokenPlanData.Price = TokenPlanViewModel.Price;
            TokenPlanData.Description = TokenPlanViewModel.Description;
            if (TokenPlanData.Id == 0)
            {
                TokenPlanData.CreatedBy = TokenPlanViewModel.CreatedBy;
                TokenPlanData.CreatedOn = DateTime.Now;
                _Context.TokenPlans.Add(TokenPlanData);
            }
            else
            {
                TokenPlanData.UpdatedBy = TokenPlanViewModel.UpdatedBy;
                TokenPlanData.UpdatedOn = DateTime.Now;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get TokenPlan Count for Dashboard
        /// <summary>
        /// Get TokenPlan data for grid
        /// </summary>
        /// <param name="TokenPlanId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns TokenPlan list</returns>
        public int GetTokenPlanCount()
        {
            return _Context.TokenPlans.Count();
        }
        #endregion

        #region Get TokenPlan List
        /// <summary>
        /// Get TokenPlan List
        /// </summary>
        /// <returns>Returns list of All TokenPlan for dropdown</returns>
        public List<DropDownBindViewModel> GetTokenPlanDropDownList(bool IsActive = true)
        {
            List<DropDownBindViewModel> objPostList = _Context.TokenPlans.Select(e => new DropDownBindViewModel { value = e.Id, name = e.TokenType.EnumValue }).ToList();
            return objPostList;
        }
        #endregion
    }
}
