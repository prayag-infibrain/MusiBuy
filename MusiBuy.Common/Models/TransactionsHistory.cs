using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class TransactionsHistory
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int PlanId { get; set; }
        public string TokenTypeName { get; set; }
        public string planDescription { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaidDate { get; set; }
        public TimeOnly PaidTime { get; set; }
        public string CardNumber { get; set; }
        public string ReferenceId { get; set; }
    }
}
