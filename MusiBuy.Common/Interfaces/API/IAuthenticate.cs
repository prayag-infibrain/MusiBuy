using MusiBuy.Common.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IAuthenticate
    {
        TokenResult AuthenticateByEmail(string EmailId, string password);
    }
}
