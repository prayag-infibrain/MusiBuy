using Microsoft.Extensions.Configuration;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using Stripe.Checkout;
namespace MusiBuy.Common.Repositories
{
    public class StripeTokenRepository : IStripeToken
    {
        #region Member Declaration
        private readonly IConfiguration _config;
        private readonly ITokenPlans _Planrepo;
        public StripeTokenRepository(IConfiguration config, ITokenPlans planrepo)
        {
            _config = config;
            _Planrepo = planrepo;
        }
        #endregion

        public string CreateTokenCheckoutSession(int planId, int UserId)
        {
            var domain = _config["Stripe:domain"];  //"https://f5wqrm9f-44359.inc1.devtunnels.ms";
            var plan = _Planrepo.GetTokenPlanDetailsByID(planId);  // Get plan from DB

            if (plan == null)
                throw new Exception("Invalid Plan");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
    {
        new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "inr",
                UnitAmount = (long)(plan.Price * 100),
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = $"{plan.TokenQuantity} {plan.TokenTypeName}"
                },
            },
            Quantity = 1,
        }
    },
                Mode = "payment",
                SuccessUrl = $"{domain}/TokenPlan/Index",
                CancelUrl = $"{domain}/payment/cancel?planId={planId}",

                // ✅ Set metadata on PaymentIntent instead of session
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
        {
            { "PlanId", plan.Id.ToString() },
            { "TokenType", plan.TokenTypeId.ToString() },
            { "Quantity", plan.TokenQuantity.ToString() },
            { "UserId", UserId.ToString() }
        }
                }
            };


            var service = new SessionService();
            var session = service.Create(options);
            return session.Url;
        }
    }
}
