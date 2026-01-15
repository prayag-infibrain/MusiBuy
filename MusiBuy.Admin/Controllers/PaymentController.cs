using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Repositories;
using MusiBuy.Common.StripeToken;
using Stripe;
using Stripe.Checkout;



namespace MusiBuy.Admin.Controllers
{
    [ApiController]
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IStripeToken _stripeTokenService;
        private readonly IPaymentTransactionsRepository _transactionsrepo;
        private const string CheckoutSessionCompleted = "checkout.session.completed";


        public PaymentController(IConfiguration config, IStripeToken stripeTokenService, IPaymentTransactionsRepository transactionsrepo)
        {
            _config = config;
            _stripeTokenService = stripeTokenService;
            _transactionsrepo = transactionsrepo;
        }

        [HttpPost("CreateCheckoutSession")]
        public IActionResult CreateCheckoutSession([FromBody] CheckoutRequest request)
        {
            try
            {
                // Call your StripeTokenService method here
                string sessionId = _stripeTokenService.CreateTokenCheckoutSession(request.PlanId, request.UserId);
                return Json(new { id = sessionId });
            }
            catch (Exception ex)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
                System.IO.File.AppendAllText(path, ex.ToString());

                return StatusCode(500, ex.Message);
                //return Json(new { ex = ex.Message });
                //return BadRequest(ex.Message);
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var json = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var secret = _config["Stripe:WebhookSecret"]; //"whsec_YA3nKFhFa38V2tIESBHYeEjLrRI8Ehl9";
            var stripeSignature = Request.Headers["Stripe-Signature"];

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, secret);
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Stripe verification error: {ex.Message}");
                return BadRequest("Signature verification failed");
            }

            // ✅ Save webhook log
            var log = new StripeWebhookLog
            {
                EventId = stripeEvent.Id,
                EventType = stripeEvent.Type,
                Payload = json,
                CreatedAt = DateTime.Now
            };
            _transactionsrepo.SaveStripeWebhookLog(log);

            //if (stripeEvent.Type ==  CheckoutSessionCompleted)
            //{
                //var session = stripeEvent.Data.Object as Session;
            var session = stripeEvent.Data.Object as PaymentIntent;

            var metadata = session.Metadata;
            if (metadata != null || metadata.Count > 0)
            {
                var planId = Convert.ToInt32(metadata["PlanId"]);
                var tokenType = Convert.ToInt32(metadata["TokenType"]);
                var quantity = Convert.ToInt32(metadata["Quantity"]);
                var amount = (decimal)(session.Amount / 100.0m);
                var userId = Convert.ToInt32(metadata["UserId"]);
                var sessionid = session.Id;

                var payment = new PaymentHistory
                {
                    PlanId = planId,
                    UserId = userId,
                    StripeSessionId = sessionid,
                    TokenType = tokenType,
                    Quantity = quantity,
                    Amount = amount,
                    Status = "Success",
                    PaidAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                _transactionsrepo.SavePaymentHistory(payment);

                _transactionsrepo.UpdateOrInsertUserTokens(userId, tokenType, quantity);
            }
            //}

            return Ok();
        }





        [HttpGet("success")]
        public IActionResult Success() => View();

        [HttpGet("cancel")]
        public IActionResult Cancel() => View();

    }
}
