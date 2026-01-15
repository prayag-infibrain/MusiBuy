using System.Collections;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using MusiBuy.Marketing.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Marketing.CustomGridBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class EnumController : Controller
    {
        #region Members Declaration
        private static int _Count = 0;
        private readonly IEnum _enumRepository;
        private readonly IConfiguration _config;

        public EnumController(IEnum enumRepository, IConfiguration config)
        {
            this._enumRepository = enumRepository;
            this._config = config;
        }
        #endregion

        #region Index

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult Index()
        {
            ViewBag.ModuleName = "Enum";
            return View();
        }
        #endregion

        #region Grid Event
        /// <summary>
        /// Enum kendo grid
        /// </summary>
        /// <param name="request">Kendo grid data source request object</param>
        /// <returns>List of enum</returns>
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string enumValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetEnumGridData(request, enumValue),
                Total = _Count
            };
            return Json(result);
        }
        #endregion

        #region Get Enum Grid Data
        /// <summary>
        /// Get enum grid data
        /// </summary>
        /// <param name="command">Kendo grid data source request object</param>
        /// <returns>List of enum</returns>
        public IEnumerable GetEnumGridData([DataSourceRequest] DataSourceRequest command, string enumValue)
        {
            var result = _enumRepository.GetEnumsList(enumValue);

            result = result.ApplyFiltering(command.Filters);

            _Count = result.Count();

            result = result.ApplySorting(command.Groups, command.Sorts);

            result = result.ApplyPaging(command.Page, command.PageSize);

            if (command.Groups.Any())
            {
                return result.ApplyGrouping(command.Groups);
            }

            foreach (var single in result)
            {
                if (single.CreatedOn != null)
                {
                    single.CreatedOn = single.CreatedOn.Value.ToLocalTime();
                }
            }
            return result.ToList();
        }
        #endregion

        #region Create

        /// <returns>Returns enum create view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create()
        {
            EnumViewModel objEnumViewModel = new EnumViewModel();
            objEnumViewModel = BindAllDropDowns(objEnumViewModel);
            objEnumViewModel.IsActive = true;
            objEnumViewModel.IsEditable = true;
            objEnumViewModel.IsDeleteable = true;
            return View(objEnumViewModel);
        }
        #endregion

        #region Create Enum Post Method
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(EnumViewModel objEnumViewModel)
        {
            try
            {
                if (objEnumViewModel.EnumTypeId == 0 || objEnumViewModel.EnumValue == null)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.NotEnteredRequiredField));
                    objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                    return View(objEnumViewModel);
                }
                long checkDuplicateEnumCount = 0;

                if (objEnumViewModel.ParentTypeId > 0 && objEnumViewModel.ParentId != null)
                    checkDuplicateEnumCount = _enumRepository.CheckEnumDuplicationByParantId(objEnumViewModel.EnumTypeId, objEnumViewModel.EnumValue.Trim(), objEnumViewModel.ParentId.Value, 0);
                else
                    checkDuplicateEnumCount = _enumRepository.CheckEnumDuplication(objEnumViewModel.EnumTypeId, objEnumViewModel.EnumValue.Trim(), 0);

                if (checkDuplicateEnumCount > 0)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Enum type-value combination"));
                    objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                    return View(objEnumViewModel);
                }

                objEnumViewModel.CreatedBy = CurrentUserSession.UserID;
                _enumRepository.SaveChanges(objEnumViewModel);
                return RedirectToAction("Index", "Enum", new { Msg = "added" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                return View(objEnumViewModel);
            }
        }
        #endregion

        #region Edit
        /// <summary>
        /// Enum edit get method to get fetch data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns enum edit view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                EnumViewModel objEnumViewModel = _enumRepository.GetEnumDetailsByEnumId(id.Value);
                if (objEnumViewModel == null)
                {
                    return RedirectToAction("Index", "Enum", new { msg = "drop" });
                }
                objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                return View(objEnumViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Enum");
            }
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(EnumViewModel objEnumViewModel)
        {
            try
            {
                if (objEnumViewModel.EnumTypeId == 0 || objEnumViewModel.EnumValue == null)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.NotEnteredRequiredField));
                    objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                    return View(objEnumViewModel);
                }
                long checkDuplicateEnumCount = 0;

                if (objEnumViewModel.ParentTypeId > 0 && objEnumViewModel.ParentId != null)
                    checkDuplicateEnumCount = _enumRepository.CheckEnumDuplicationByParantId(objEnumViewModel.EnumTypeId, objEnumViewModel.EnumValue.Trim(), objEnumViewModel.ParentId.Value, objEnumViewModel.Id);
                else
                    checkDuplicateEnumCount = _enumRepository.CheckEnumDuplication(objEnumViewModel.EnumTypeId, objEnumViewModel.EnumValue.Trim(), objEnumViewModel.Id);
                if (checkDuplicateEnumCount > 0)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Enum type-value combination"));
                    objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                    return View(objEnumViewModel);
                }

                objEnumViewModel.UpdatedBy = CurrentUserSession.UserID;
                _enumRepository.SaveChanges(objEnumViewModel);
                return RedirectToAction("Index", "Enum", new { Msg = "updated" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                objEnumViewModel = BindAllDropDowns(objEnumViewModel);
                return View(objEnumViewModel);
            }
        }
        #endregion

        #region Detail
        /// <summary>
        /// Enum detail get method to get enum data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns enum detail view</returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        public ActionResult Detail(int? id)
        {
            if (id != null)
            {
                EnumViewModel objEnumViewModel = _enumRepository.GetEnumDetailsByEnumId(id.Value);
                objEnumViewModel.EnumValue = objEnumViewModel.EnumValue;
                objEnumViewModel.EnumTypeName = objEnumViewModel.EnumTypeName;
                objEnumViewModel.Active = objEnumViewModel.IsActive ? GlobalCode.ActiveText : GlobalCode.InActiveText;
                objEnumViewModel.Deletable = objEnumViewModel.IsDeleteable ? GlobalCode.YesText : GlobalCode.NoText;
                objEnumViewModel.Editable = objEnumViewModel.IsEditable ? GlobalCode.YesText : GlobalCode.NoText;
                return View(objEnumViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Enum");
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete enum post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If enum data deleted succesfully then returns success message other wise returns error message on enum list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(long[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    _enumRepository.DeleteEnum(chkDelete);
                    return RedirectToAction("Index", "Enum", new { Msg = "deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "Enum", new { Msg = "NoSelect" });
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains(GlobalCode.foreignKeyReference) || ((ex.InnerException).InnerException).Message.Contains(GlobalCode.foreignKeyReference))
                {
                    return RedirectToAction("Index", "Enum", new { Msg = "inuse" });
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region JSON Method to Get Parent list for DropdownList
        /// <summary>
        /// Get parent list by parent type
        /// </summary>
        /// <param name="parentTypeID"></param>
        /// <param name="isFromCreate"></param>
        /// <returns>Returns list of parent data by parent type in json</returns>
        [HttpGet]
        public JsonResult GetParent(int ParentTypeID)
        {
            if (ParentTypeID > 0)
            {
                IEnumerable<DropDownBindViewModel> objEnumList = _enumRepository.GetEnumList(ParentTypeID);
                return Json(objEnumList);
            }
            else
            {
                return Json(null);
            }
        }
        #endregion

        #region JSON Method to Check Duplicate Enum
        /// <summary>
        /// Check if enum value already exists in table
        /// </summary>
        /// <param name="objValidateEnum"></param>
        /// <returns>If enum already exists then returns 0 otherwise 1 in json</returns>
        [HttpPost]
        public JsonResult ValidateDuplicateEnum(EnumViewModel objValidateEnum)
        {
            try
            {
                int intAttributeCount = _enumRepository.CheckEnumExist(objValidateEnum.EnumTypeId, (objValidateEnum.EnumValue ?? string.Empty).Trim(), objValidateEnum.Id);
                if (intAttributeCount > 0)
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Enum type-value combination"));
                    return Json(new { @status = "0" });
                }
                else
                {
                    return Json(new { @status = "1" });
                }
            }
            catch (Exception)
            {
                return Json(new { @status = "" });
            }
        }
        #endregion

        #region JSON Method to check Duplicate Enum by ParentID

        [HttpPost]
        public JsonResult ValidateDuplicateEnumByParentID(EnumViewModel objEnumViewModel)
        {
            try
            {
                int intEnumCount = _enumRepository.CheckEnumDuplicationByParantId(objEnumViewModel.EnumTypeId, (objEnumViewModel.EnumValue ?? string.Empty).Trim(), objEnumViewModel.ParentId.HasValue ? objEnumViewModel.ParentId.Value : 0, objEnumViewModel.Id);
                if (intEnumCount > 0)
                {
                    return Json(new { @status = "0" });
                }
                else
                {
                    return Json(new { @status = "1" });
                }
            }
            catch (Exception)
            {
                return Json(new { @status = "" });
            }
        }
        #endregion

        #region Bind All Drop Downs
        private EnumViewModel BindAllDropDowns(EnumViewModel objEnumViewModel)
        {
            List<EnumTypeViewModel> objEnumList = _enumRepository.GetEnumTypeList();

            objEnumViewModel.SelectEnumType = new SelectList(objEnumList.ToList(), "Id", "EnumTypeValue");

            objEnumList.Insert(0, new EnumTypeViewModel() { Id = 0, EnumTypeValue = "" });
            objEnumViewModel.ParentTypeList = new SelectList(objEnumList.ToList(), "Id", "EnumTypeValue");

            if (objEnumViewModel.ParentTypeId != null && objEnumViewModel.ParentTypeId != 0)
                objEnumViewModel.ParentList = new SelectList(_enumRepository.GetEnumList(objEnumViewModel.ParentTypeId.Value), "value", "name");
            else
                objEnumViewModel.ParentList = new SelectList(string.Empty);

            return objEnumViewModel;
        }
        #endregion
    }
}