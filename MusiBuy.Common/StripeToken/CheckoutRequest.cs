using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.StripeToken
{
    public class CheckoutRequest
    {
        public int PlanId { get; set; }
        public int UserId { get; set; }
    }
}
