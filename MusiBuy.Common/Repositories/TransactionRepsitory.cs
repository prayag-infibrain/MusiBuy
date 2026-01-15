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
    public class TransactionRepsitory : ITransaction
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        public TransactionRepsitory(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion



        #region Get Token Plan List
        public List<TokenPlanViewModel> GetTokenPlanList()
        {
            var result = _Context.TokenPlans.OrderByDescending(x => x.CreatedOn)
                .Select(u => new TokenPlanViewModel
                {
                    Id = u.Id,
                    TokenTypeName = u.TokenType.EnumValue,
                    TokenQuantity = u.TokenQuantity,
                    Price = u.Price,
                    Description = u.Description,
                }).ToList();

            return result;
        }
        #endregion
    }
}
