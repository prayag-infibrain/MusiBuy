using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Marketing.CustomBinding;
using MusiBuy.Marketing.Helper;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using MusiBuy.Common.Enumeration;
using System.Buffers;
using MusiBuy.Common.DB;

namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class TransactionsHistoryController : Controller
    {
        private readonly IPaymentTransactionsRepository _paymentTransactions;
        private static int _totalCount = 0;

        public TransactionsHistoryController(IPaymentTransactionsRepository paymentTransactions)
        {
            this._paymentTransactions = paymentTransactions;
        }



        #region Transaction history Grid
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Transactions History";
            TransactionsHistory TransactionsHistory = new TransactionsHistory();
            return View(TransactionsHistory);
        }


        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetUserTransactionGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetUserTransactionGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            //var transactions = _paymentTransactions.GetUserTransactionsHistory(CurrentAdminSession.User.UserID);
            var result = _paymentTransactions.GetUserTransactionsHistory(CurrentUserSession.User.UserID, searchValue);

            result = result.ApplyFiltering(command.Filters);

            _totalCount = result.Count();

            result = result.ApplySorting(command.Groups, command.Sorts);

            result = result.ApplyPaging(command.Page, command.PageSize);

            if (command.Groups.Any())
            {
                return result.ApplyGrouping(command.Groups);
            }
            return result.ToList();
        }
        #endregion
    }
}