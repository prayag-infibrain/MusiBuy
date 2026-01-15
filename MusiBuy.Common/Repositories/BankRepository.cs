using Microsoft.EntityFrameworkCore;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Repositories
{
    public class BankRepository : IBank
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        public BankRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion

        #region Save Bank
        public bool Save(BankMasterViewModel BankViewModel)
        {
            var BankData = new BankMaster();
            if (BankViewModel.Id > 0)
            {
                BankData = _Context.BankMasters.FirstOrDefault(x => x.Id == BankViewModel.Id) ?? new BankMaster();
            }
            BankData.UserId = BankViewModel.UserId;
            BankData.BankName = BankViewModel.BankName;
            BankData.AccountNumber = BankViewModel.AccountNumber;
            BankData.Ifsccode = BankViewModel.IFSCCode;
            BankData.AccountHolder = BankViewModel.AccountHolder;
            BankData.IsSelected = BankViewModel.IsSelected;
            BankData.IsActive = BankViewModel.IsActive;

            if (BankViewModel.Id == 0)
            {
                var OldData = _Context.BankMasters.Where(x => x.UserId == BankViewModel.UserId).ToList();
                foreach (var item in OldData)
                {
                    item.IsSelected = false;
                    _Context.SaveChanges();
                }
                BankData.CreatedBy = BankViewModel.CreatedBy;
                BankData.CreatedOn = DateTime.Now;
                _Context.BankMasters.Add(BankData);
            }
            else
            {
                BankData.UpdatedBy = BankViewModel.UpdatedBy;
                BankData.UpdatedOn = DateTime.Now;
            }
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Get Bank List By UserId
        public List<BankMasterViewModel> GetBankListByUserId(int Id)
        {
            var result = _Context.BankMasters.OrderByDescending(x => x.CreatedOn).Where(a => a.UserId == Id && a.IsActive == true)
                .Select(u => new BankMasterViewModel
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    BankName = u.BankName,
                    AccountNumber = u.AccountNumber,
                    IFSCCode = u.Ifsccode,
                    AccountHolder = u.AccountHolder,
                    IsActive = u.IsActive,
                    IsSelected = u.IsSelected,
                }).ToList();

            return result;
        }
        #endregion

        #region Delete Bank By UserId and Bank Id
        public bool DeletebankByBankId(GetByUserId getByUser)
        {
            var result = _Context.BankMasters.FirstOrDefault(x => x.Id == getByUser.Id && x.UserId == getByUser.UserId);
            result.IsActive = false;
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Select Bank By UserId and Bank Id
        public bool SelectByBankId(GetByUserId getByUser)
        {
            var result = _Context.BankMasters.FirstOrDefault(x => x.Id == getByUser.Id && x.UserId == getByUser.UserId);
            var OldData = _Context.BankMasters.Where(x => x.UserId == getByUser.UserId).ToList();
            foreach (var item in OldData)
            {
                item.IsSelected = false;
                _Context.SaveChanges();
            }
            result.IsSelected = true;
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
