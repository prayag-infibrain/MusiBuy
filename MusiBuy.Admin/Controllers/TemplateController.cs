using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Common;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Common.Models;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class TemplateController : Controller
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private static int _totalCount = 0;
        private readonly ITemplate _templateRepository;
        private readonly ICommonSetting _commonSettingRepository;

        public TemplateController(ITemplate templateMaster, ICommonSetting commonSetting)
        {
            _templateRepository = templateMaster;
            _commonSettingRepository = commonSetting;
        }
        #endregion

        #region Index
        /// <summary>
        /// Template index method
        /// </summary>
        /// <returns>Returns template index view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult Index()
        {
            ViewBag.ModuleName = "Email Template";
            return View();
        }
        #endregion

        #region Grid Binding
        /// <summary>
        /// Enum kendo grid
        /// </summary>
        /// <param name="request">Kendo grid data source request object</param>
        /// <returns>List of Template</returns>
        /// [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult Template_Read([DataSourceRequest] DataSourceRequest request, string templateName)
        {
            var result = new DataSourceResult()
            {
                Data = GetTemplateGridData(request, templateName),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetTemplateGridData([DataSourceRequest] DataSourceRequest command, string templateName)
        {
            var result = _templateRepository.GetTemplateList(templateName);

            result = result.ApplyFiltering(command.Filters);

            _totalCount = result.Count();

            result = result.ApplySorting(command.Groups, command.Sorts);

            result = result.ApplyPaging(command.Page, command.PageSize);

            if (command.Groups.Any())
            {
                return result.ApplyGrouping(command.Groups);
            }
            var data = result.ToList();

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] != null && data[i].TemplateName != null )
                {
                    data[i].TemplateName= _templateRepository.ToSentenceCase(data[i].TemplateName ?? string.Empty);
                }
            } 
            return data;
        }
        #endregion

        #region Add Template Data
        /// <summary>
        /// Template create get method
        /// </summary>
        /// <returns>Returns template create view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create()
        {
            var viewModel = new TemplateViewModel
            {
                IsActive = true,
            };
            return View(viewModel);
        }

        /// <summary>
        /// Template create post method for saving template data
        /// </summary>
        /// <param name="templateViewModel"></param>
        /// <returns>If template data saved succesfully then returns success message other wise returns template view with error message</returns>
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        [HttpPost]
        public ActionResult Create(TemplateViewModel templateViewModel)
        {
            var viewModel = new TemplateViewModel();

            if (ModelState.IsValid)
            {
                if (!CkEditorValidator(templateViewModel.TemplateContent ?? string.Empty))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.RequiredField, "Content"));
                    return View(viewModel);
                }

                if (string.IsNullOrEmpty(templateViewModel.TemplateName))
                {
                    ModelState.AddModelError(string.Empty, Messages.NotEnteredRequiredField);
                    return View(viewModel);
                }

                templateViewModel.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _templateRepository.SaveTemplateData(templateViewModel);
                if (isSaved)
                    return RedirectToAction("Index", "Template", new { Msg = "added" });
                else
                    return RedirectToAction("Index", "Template", new { Msg = "not added" });

            }
            else
            {
                return View(viewModel);
            }
        }

        #endregion

        #region For Editor Validation
        /// <summary>
        ///  For Editor Validation
        /// </summary>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static bool CkEditorValidator(string strData)
        {
            bool flag = false;
            string strContent = "<html><head><title></title></head><body></body></html>";

            if (string.IsNullOrEmpty(strData) || Convert.ToString(strData).Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\r\n", "") == strContent)
            {
                return flag;
            }
            return !flag;
        }
        #endregion

        #region Edit Template
        /// <summary>
        /// Template edit get method to get fetch data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns template edit view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TemplateViewModel objTemplateViewModel = _templateRepository.GetTemplateByID(Convert.ToInt32(id));
            if (objTemplateViewModel == null)
            {
                return RedirectToAction("Index", "Template", new { Msg = "drop" });
            }
            return View(objTemplateViewModel);
        }

        /// <summary>
        /// Template edit post method for updating template data
        /// </summary>
        /// <param name="templateViewModel"></param>
        /// <returns>If template data updated succesfully then returns success message other wise returns template view with error message</returns>
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        [HttpPost]
        public ActionResult Edit(TemplateViewModel templateViewModel)
        {
            var viewModel = new TemplateViewModel();
            if (ModelState.IsValid)
            {
                if (!CkEditorValidator(templateViewModel.TemplateContent ?? string.Empty))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.RequiredField, "Content"));
                    return View(viewModel);
                }
                var objTemplate = _templateRepository.GetTemplateByID(Convert.ToInt32(templateViewModel.Id));
                if (objTemplate == null)
                {
                    return RedirectToAction("Index", "Template", new { Msg = "drop" });
                }
                if (string.IsNullOrEmpty(templateViewModel.TemplateName))
                {
                    ModelState.AddModelError(string.Empty, Messages.NotEnteredRequiredField);
                    return View(viewModel);
                }
                templateViewModel.UpdatedBy = CurrentAdminSession.User.UserID;
                bool isUpdtaed = _templateRepository.SaveTemplateData(templateViewModel);
                if (isUpdtaed)
                    return RedirectToAction("Index", "Template", new { Msg = "updated" });
                else
                    return RedirectToAction("Index", "Template", new { Msg = "not updated" });
            }
            else
            {
                return View(viewModel);
            }
        }

        #endregion

        #region Delete
        /// <summary>
        ///  Delete Template post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If template data deleted succesfully then returns success message other wise returns error message on template list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    bool isDelete = _templateRepository.DeleteTemplate(chkDelete);
                    if (isDelete)
                        return RedirectToAction("Index", "Template", new { Msg = "deleted" });
                    else
                        return RedirectToAction("Index", "Template", new { Msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "Template", new { Msg = "NoSelect" });
                }
            }
            catch (Exception)
            {
                    return RedirectToAction("Index", "Template", new { msg = "inuse" });
            }
        

        }
        #endregion

        #region Detail
        /// <summary>
        /// Template detail get method to get template data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns template detail view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        [HttpGet]
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TemplateViewModel objTemplateViewModel = _templateRepository.GetTemplateByID(Convert.ToInt32(id));
            objTemplateViewModel.TemplateName = _templateRepository.ToSentenceCase(objTemplateViewModel.TemplateName ?? string.Empty);
            objTemplateViewModel.Subject = _templateRepository.ToSentenceCase(objTemplateViewModel.Subject ?? string.Empty);
            if (objTemplateViewModel == null)
            {
                return RedirectToAction("Index", "Template", new { Msg = "drop" });
            }
            var SiteUrl = _commonSettingRepository.GetCommonSetting().SiteURL;
            objTemplateViewModel.TemplateContent = (objTemplateViewModel.TemplateContent ?? string.Empty).Replace("###SiteUrl###", Convert.ToString(SiteUrl));
            return View(objTemplateViewModel);
        }
        #endregion

    }
}