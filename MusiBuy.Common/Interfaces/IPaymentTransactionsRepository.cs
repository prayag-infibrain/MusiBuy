using MusiBuy.Common.DB;
using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IPaymentTransactionsRepository
    {
        bool SavePaymentHistory(PaymentHistory payment);
        bool SaveStripeWebhookLog(StripeWebhookLog StripeWebhookLog);
        void UpdateOrInsertUserTokens(int userId, int tokenType, int quantity);
        IQueryable<TransactionsHistory> GetUserTransactionsHistory(int userId, string searchValue);
    }
}
