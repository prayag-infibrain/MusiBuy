using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Common.Common;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Admin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusiBuy.Common.Enumeration;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class ContentController : Controller
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IContentManagement _cmsRepository;
        private readonly IEnum _enumRepository;

        public ContentController(IContentManagement cmsRepository, IEnum enumRepository)
        {
            _cmsRepository = cmsRepository;
            _enumRepository = enumRepository;
        }
        #endregion

        #region Index
        /// <summary>
        /// Action to render Index view of content management
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult Index()
        {
            ViewBag.ModuleName = "Content Managment ";
            ContentManagementsViewModel contentManagementViewModel = new ContentManagementsViewModel();
            contentManagementViewModel = BindDropDown(contentManagementViewModel);
            contentManagementViewModel.IsActive=true;
            return View(contentManagementViewModel);
        }

        /// <summary>
        /// Content management post method to view content of page
        /// </summary>
        /// <param name="contentManagementViewModel"></param>
        /// <returns>If content is saved successfully it redirects to the index view with success messege other wise with failed messege.</returns>
        [HttpPost]
        public ActionResult Index(ContentManagementsViewModel contentManagementViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!CkEditorValidator(contentManagementViewModel.Content ?? string.Empty))
                {
                    contentManagementViewModel = BindDropDown(contentManagementViewModel);
                    ModelState.AddModelError(string.Empty, string.Format(Messages.RequiredField, "Content"));
                    return View(contentManagementViewModel);
                }

                bool isSaved=_cmsRepository.Save(contentManagementViewModel);
                if (isSaved)
                   return RedirectToAction("Index", "Content", new { Msg = "change" });
                else   return RedirectToAction("Index", "Content", new { Msg = "not change" });

            }
            else
            {
                contentManagementViewModel = BindDropDown(contentManagementViewModel);
                return View(contentManagementViewModel);
            }
        }
        #endregion

        #region Bind Dropdown
        /// <summary>
        /// Bind Dropdown
        /// </summary>
        /// <param name="contentManagementViewModel"></param>
        /// <returns>It binds all dropdown</returns>
        private ContentManagementsViewModel BindDropDown(ContentManagementsViewModel contentManagementViewModel)
        {
            var data = _enumRepository.GetEnumsListByType((int)EnumTypes.ContentManagement);
            if (data.Count() > 0)
            {
                contentManagementViewModel.PageList = new SelectList(data.ToList(), "Id", "EnumValue");
            }
            return contentManagementViewModel;
        }
        #endregion

        #region Get CMS Page content json call
        /// <summary>
        /// Get page content
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns>Json</returns>
        public JsonResult GetPageContent(int pageId)
        {
            var data = _cmsRepository.GetContentManagementsContentByID(pageId);
            if (data != null)
            {
                return Json(data);
            }
            return Json(new ContentManagementsViewModel());
        }
        #endregion

        #region For Editor Validation
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
    }
}
