using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Admin.Helper;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using MusiBuy.Common.Repositories;
using MusiBuy.Common.Enumeration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Hosting.Server;
using MusiBuy.Common.DB;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class EventManagementController : Controller
    {
        private readonly IEventManagement _eventManagement;
        private readonly ICreatores _creatoresRepository;
        private readonly IEnum _enum;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public EventManagementController(IConfiguration config, ICreatores creatoresRepository, IEnum enumrepo, IEventManagement eventManagement)
        {
            _eventManagement = eventManagement;
            _creatoresRepository = creatoresRepository;
            _config = config;
            _enum = enumrepo;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Event Management";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetEventManagementGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetEventManagementGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _eventManagement.GetEventManagementList(CurrentAdminSession.User.UserID, searchValue);

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
            EventManagementViewModel objValidatePost = new EventManagementViewModel();
            BindDropdown(objValidatePost);
            objValidatePost.RecordingURL = " ";
            return View(objValidatePost);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(EventManagementViewModel objValidatePost)
        {
            ModelState.Remove("RecordingURL");
            if (ModelState.IsValid)
            {
                var dates = objValidatePost.Daterange.Split(" - ");
                objValidatePost.EventStartDateTime = DateTime.ParseExact(dates[0], "yyyy-MM-dd HH:mm:ss", null);
                objValidatePost.EventEndDateTime = DateTime.ParseExact(dates[1], "yyyy-MM-dd HH:mm:ss", null);

                objValidatePost.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _eventManagement.Save(objValidatePost);
                if (isSaved)
                    return RedirectToAction("Index", "EventManagement", new { msg = "added" });
                else
                    return RedirectToAction("Index", "EventManagement", new { msg = "not added" });
            }
            else
            {
                BindDropdown(objValidatePost);
                return View(objValidatePost);
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
            EventManagementViewModel objValidatePost = _eventManagement.GetEventManagementDetailsByID(id);
            if (objValidatePost == null)
            {
                return RedirectToAction("Index", "EventManagement", new { msg = "drop" });
            }
            BindDropdown(objValidatePost);
            if (string.IsNullOrEmpty(objValidatePost.RecordingURL))
                objValidatePost.RecordingURL = "  ";
            return View(objValidatePost);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidatePost"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(EventManagementViewModel objValidatePost)
        {
            ModelState.Remove("RecordingURL");
            if (ModelState.IsValid)
            {
                var PostDetail = _eventManagement.GetEventManagementDetailsByID(objValidatePost.Id);
                if (PostDetail == null)
                {
                    return RedirectToAction("Index", "EventManagement", new { msg = "drop" });
                }
                var dates = objValidatePost.Daterange.Split(" - ");
                objValidatePost.EventStartDateTime = DateTime.ParseExact(dates[0], "yyyy-MM-dd HH:mm:ss", null);
                objValidatePost.EventEndDateTime = DateTime.ParseExact(dates[1], "yyyy-MM-dd HH:mm:ss", null);

                objValidatePost.UpdatedBy = CurrentAdminSession.UserID;
                bool isUpdated = _eventManagement.Save(objValidatePost);
                if (isUpdated)
                    return RedirectToAction("Index", "EventManagement", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "EventManagement", new { msg = "not updated" });

            }
            else
            {
                BindDropdown(objValidatePost);
                return View(objValidatePost);
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
                EventManagementViewModel objValidatePost = _eventManagement.GetEventManagementDetailsByID(id);
                if (objValidatePost == null)
                {
                    return RedirectToAction("Index", "EventManagement", new { msg = "drop" });
                }
                else
                {
                    return View(objValidatePost);
                }
            }
            else
            {
                return RedirectToAction("Index", "EventManagement", new { msg = "error" });
            }
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
                    bool isDeleted = _eventManagement.DeleteEvent(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "EventManagement", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "EventManagement", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "EventManagement", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "EventManagement", new { msg = "inuse" });
            }

        }
        #endregion

        public void BindDropdown(EventManagementViewModel objValidatePost)
        {
            objValidatePost.SelectCreator = new SelectList(_creatoresRepository.GetCreatoreDropDownList(true), "value", "name");

            var Typedata = _enum.GetEnumsListByType((int)EnumTypes.EventType);
            if (Typedata.Count() > 0)
                objValidatePost.SelectEventType = new SelectList(Typedata.ToList(), "Id", "EnumValue");

            var Statusdata = _enum.GetEnumsListByType((int)EnumTypes.EventStatus);
            if (Statusdata.Count() > 0)
                objValidatePost.SelectStatus = new SelectList(Statusdata.ToList(), "Id", "EnumValue");
        }
    }
}