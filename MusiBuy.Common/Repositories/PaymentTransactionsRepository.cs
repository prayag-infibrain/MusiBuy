
using System.Buffers;
using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Newtonsoft.Json.Linq;
namespace MusiBuy.Common.Repositories
{
    public class PaymentTransactionsRepository : IPaymentTransactionsRepository
    {

        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        public PaymentTransactionsRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion


        #region Save Payment History
        public bool SavePaymentHistory(PaymentHistory payment)
        {
            _Context.PaymentHistories.Add(payment);
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Save Stripe Webhook Log 
        public bool SaveStripeWebhookLog(StripeWebhookLog StripeWebhookLog)
        {
            _Context.StripeWebhookLogs.Add(StripeWebhookLog);
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get User Token 
        public void UpdateOrInsertUserTokens(int userId, int tokenType, int quantity)
        {
            var token = _Context.UserTokens
                .FirstOrDefault(x => x.UserId == userId && x.TokenType == tokenType);

            if (token != null)
            {
                token.Quantity += quantity;
                token.UpdatedAt = DateTime.Now;
            }
            else
            {
                _Context.UserTokens.Add(new UserToken
                {
                    UserId = userId,
                    TokenType = tokenType,
                    Quantity = quantity,
                    UpdatedAt = DateTime.Now
                });
            }
            _Context.SaveChanges();
        }
        #endregion


        #region Get User Token 
        public IQueryable<TransactionsHistory> GetUserTransactionsHistory(int userId, string searchValue)
        {
            var result = (from u in _Context.PaymentHistories
                          join p in _Context.TokenPlans on u.PlanId equals p.Id
                          join tkn in _Context.Enums on p.TokenTypeId equals tkn.Id
                          join user in _Context.AdminUsers on u.UserId equals user.Id
                          where (string.IsNullOrWhiteSpace(searchValue) ||
                          user.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                          user.LastName.ToLower().Contains(searchValue.ToLower()) ||
                          tkn.EnumValue.ToLower().Contains(searchValue.ToLower()) ||
                          p.Description.ToLower().Contains(searchValue.ToLower()) ||
                          u.Status.ToLower().Contains(searchValue.ToLower()) ||
                          u.Quantity.ToString().ToLower().Contains(searchValue.ToLower()))
                          orderby user.FirstName
                          select new TransactionsHistory
                          {
                              UserId = u.UserId,
                              Username = user.FirstName + " " + user.LastName,
                              PlanId = u.PlanId,
                              TokenTypeName = tkn.EnumValue,
                              planDescription = p.Description,
                              Quantity = u.Quantity,
                              Amount = u.Amount,
                              Status = u.Status,
                              PaidDate = u.PaidAt,
                              PaidTime = TimeOnly.FromDateTime(u.PaidAt),
                              CardNumber = "4242 4242 4242 4242", // not available in tables
                              ReferenceId = u.StripeSessionId
                          });

            return result;

            //var result = (from u in _Context.PaymentHistories
            //              join p in _Context.TokenPlans on u.PlanId equals p.Id
            //              join tkn in _Context.Enums on p.TokenTypeId equals tkn.Id
            //              select new TransactionsHistory
            //              {
            //                  UserId = u.UserId,
            //                  PlanId = u.PlanId,
            //                  TokenTypeName = tkn.EnumValue,
            //                  planDescription = p.Description,
            //                  Quantity = u.Quantity,
            //                  Amount = u.Amount,
            //                  Status = u.Status,
            //                  PaidDate = u.PaidAt.Date,
            //                  PaidTime = TimeOnly.FromDateTime(u.PaidAt),
            //                  CardNumber = "4242 4242 4242 4242", // not available in tables
            //                  ReferenceId = u.StripeSessionId
            //              }).ToList();
            //return result;
        }
        #endregion
    }
}
