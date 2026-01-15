using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.ResponseModels
{
    public class TokenResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
