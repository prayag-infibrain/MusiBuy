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

namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class TokenPlanController : Controller
    {
        private readonly ITokenPlans _paymentTransactions;
        private readonly IConfiguration _config;
        private readonly IEnum _enum;
        private static int _totalCount = 0;

        public TokenPlanController(ITokenPlans tokenplanRepository, IConfiguration config, IEnum enumrepo)
        {
            this._paymentTransactions = tokenplanRepository;
            this._config = config;
            _enum = enumrepo;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.StripePublishableKey = _config["Stripe:PublishableKey"];
            ViewBag.ModuleName = "Token Plan";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetUserGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetUserGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _paymentTransactions.GetTokenPlanList(searchValue);

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

        #region Create
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public IActionResult Create()
        {
            TokenPlanViewModel objValidateToken = new TokenPlanViewModel();
            BindDropDown(objValidateToken);
            return View(objValidateToken);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(TokenPlanViewModel objValidateToken)
        {
            if (ModelState.IsValid)
            {
                //if (_paymentTransactions.IsUserExists(0, (objValidateToken.Username ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                //    objValidateToken.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                //    return View(objValidateToken);
                //}

                bool isSaved = _paymentTransactions.Save(objValidateToken);
                if (isSaved)
                    return RedirectToAction("Index", "TokenPlan", new { msg = "added" });
                else
                    return RedirectToAction("Index", "TokenPlan", new { msg = "not added" });
            }
            else
            {
                BindDropDown(objValidateToken);
                return View(objValidateToken);
            }
        }
        #endregion

        #region Edit
        /// <summary>
        /// User edit get method to get fetch user data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user edit view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(int id)
        {
            TokenPlanViewModel objValidateToken = _paymentTransactions.GetTokenPlanDetailsByID(id);
            if (objValidateToken == null)
                return RedirectToAction("Index", "TokenPlan", new { msg = "drop" });
            BindDropDown(objValidateToken);
            return View(objValidateToken);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateToken"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(TokenPlanViewModel objValidateToken)
        {
            if (ModelState.IsValid)
            {
                var UserDetail = _paymentTransactions.GetTokenPlanDetailsByID(objValidateToken.Id);
                if (UserDetail == null)
                {
                    return RedirectToAction("Index", "TokenPlan", new { msg = "drop" });
                }
                //if (_paymentTransactions.IsUserExists(objValidateToken.Id, (objValidateToken.Username ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                //    objValidateToken.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                //    return View(objValidateToken);
                //}
                bool isUpdated = _paymentTransactions.Save(objValidateToken);
                if (isUpdated)
                    return RedirectToAction("Index", "TokenPlan", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "TokenPlan", new { msg = "not updated" });

            }
            else
            {
                BindDropDown(objValidateToken);
                return View(objValidateToken);
            }
        }
        #endregion

        #region Detail
        /// <summary>
        /// User detail get method to get enum data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user detail view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        public ActionResult Detail(int id)
        {
            if (id > 0)
            {
                TokenPlanViewModel objValidateToken = _paymentTransactions.GetTokenPlanDetailsByID(id);
                if (objValidateToken == null)
                    return RedirectToAction("Index", "TokenPlan", new { msg = "drop" });
                else
                    return View(objValidateToken);
            }
            else
                return RedirectToAction("Index", "TokenPlan", new { msg = "error" });
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete user post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If user data deleted succesfully then returns success message other wise returns error message on user list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    bool isDeleted = _paymentTransactions.DeleteTokenPlans(chkDelete);
                    if (isDeleted)
                        return RedirectToAction("Index", "TokenPlan", new { msg = "deleted" });
                    return RedirectToAction("Index", "TokenPlan", new { msg = "not deleted" });
                }
                else
                    return RedirectToAction("Index", "TokenPlan", new { msg = "noselect" });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "TokenPlan", new { msg = "inuse" });
            }

        }
        #endregion


        public void BindDropDown(TokenPlanViewModel objValidateToken)
        {
            objValidateToken.SelectTokenType = new SelectList(_enum.GetEnumList((int)EnumTypes.TokenType), "value", "name");
        }
    }
}