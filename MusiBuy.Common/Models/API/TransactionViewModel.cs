using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class TransactionViewModel
    {
        public class ApiBankMaster
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string BankName { get; set; }
            public string AccountNumber { get; set; }
            public string IFSCCode { get; set; }
            public string AccountHolder { get; set; }
        }
    }
}
