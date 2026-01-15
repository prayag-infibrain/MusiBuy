using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IBank
    {
        bool Save(BankMasterViewModel BankViewModel);
        List<BankMasterViewModel> GetBankListByUserId(int Id);
        bool DeletebankByBankId(GetByUserId getByUser);
        bool SelectByBankId(GetByUserId getByUser);
    }
}
