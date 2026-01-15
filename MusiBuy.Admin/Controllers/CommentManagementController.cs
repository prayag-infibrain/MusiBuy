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
using System.Buffers;
using Kendo.Mvc.Extensions;
using System.Xml.Linq;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class CommentManagementController : Controller
    {
        private readonly ICommentManagement _CommentManagement;
        private readonly IPostManagement _postManagement;
        private readonly ICreatores _creatoresRepository;
        private readonly IUser _user;
        private readonly IEnum _enum;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public CommentManagementController(IConfiguration config, IPostManagement postManagement, IEnum enumrepo, ICommentManagement CommentManagement,IUser user, ICreatores creatoresRepository)
        {
            _CommentManagement = CommentManagement;
            _postManagement = postManagement;
            _config = config;
            _enum = enumrepo;
            _user = user;
            _creatoresRepository = creatoresRepository;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index(int CreatorId = 0,int PostId = 0)
        {
            //ViewBag.PostList = new SelectList(_postManagement.GetPostManagementDropDownList(), "value", "name");
            //ViewBag.CreatorList = new SelectList(_creatoresRepository.GetCreatoreDropDownList(), "value", "name");

            ViewBag.ModuleName = "Comment Management";
            CommentManagementViewModel comment = new CommentManagementViewModel();
            comment.CreatorId = CreatorId;
            comment.PostId = PostId;

            comment.SelectCreator = new SelectList(_creatoresRepository.GetCreatoreDropDownList(), "value", "name");
            //if (CreatorId > 0)
            //    objValidateComment.SelectPost = new SelectList(_postManagement.GetPostManagementDropDownList().Where(a => a.), "value", "name");

            return View(comment);
        }

        #region Grid Comment
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue, int? creatorId, int? postId)
        {
            var result = new DataSourceResult()
            {
                Data = GetCommentManagementGridData(request, searchValue,creatorId, postId),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetCommentManagementGridData([DataSourceRequest] DataSourceRequest command, string searchValue, int? creatorId, int? postId)
        {
            var result = _CommentManagement.GetCommentManagementList(CurrentAdminSession.User.UserID, searchValue, creatorId, postId);

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
            CommentManagementViewModel objValidateComment = new CommentManagementViewModel();
            BindDropdown(objValidateComment);
            return View(objValidateComment);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(CommentManagementViewModel objValidateComment)
        {
            if (ModelState.IsValid)
            {
                objValidateComment.StatusId = (int)CommentStatusEnum.Pending;
                objValidateComment.UserId = CurrentAdminSession.UserID;
                objValidateComment.UserType = CurrentAdminSession.User.RoleID;
                objValidateComment.CreatedBy = CurrentAdminSession.UserID;
                bool isSaved = _CommentManagement.Save(objValidateComment);
                if (isSaved)
                    return RedirectToAction("Index", "CommentManagement", new { msg = "added" });
                else
                    return RedirectToAction("Index", "CommentManagement", new { msg = "not added" });
            }
            else
            {
                return View(objValidateComment);
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
            CommentManagementViewModel objValidateComment = _CommentManagement.GetCommentManagementDetailsByID(id);
            if (objValidateComment == null)
            {
                return RedirectToAction("Index", "CommentManagement", new { msg = "drop" });
            }
            BindDropdown(objValidateComment);
            return View(objValidateComment);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateComment"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(CommentManagementViewModel objValidateComment)
        {
            if (ModelState.IsValid)
            {
                var PostDetail = _CommentManagement.GetCommentManagementDetailsByID(objValidateComment.Id);
                if (PostDetail == null)
                {
                    return RedirectToAction("Index", "CommentManagement", new { msg = "drop" });
                }
                objValidateComment.StatusId = (int)CommentStatusEnum.Pending;
                objValidateComment.UserId = CurrentAdminSession.UserID;
                objValidateComment.UserType = CurrentAdminSession.User.UserTypeId;
                objValidateComment.UpdatedBy = CurrentAdminSession.UserID;
                bool isUpdated = _CommentManagement.Save(objValidateComment);
                if (isUpdated)
                    return RedirectToAction("Index", "CommentManagement", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "CommentManagement", new { msg = "not updated" });

            }
            else
            {
                return View(objValidateComment);
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
                CommentManagementViewModel objValidateComment = _CommentManagement.GetCommentManagementDetailsByID(id);
                if (objValidateComment == null)
                {
                    return RedirectToAction("Index", "CommentManagement", new { msg = "drop" });
                }
                else
                {
                    return View(objValidateComment);
                }
            }
            else
            {
                return RedirectToAction("Index", "CommentManagement", new { msg = "error" });
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Comment post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If Comment data deleted succesfully then returns success message other wise returns error message on user list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    bool isDeleted = _CommentManagement.DeleteComment(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "CommentManagement", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "CommentManagement", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "CommentManagement", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "CommentManagement", new { msg = "inuse" });
            }

        }
        #endregion

        public void BindDropdown(CommentManagementViewModel objValidateComment)
        {
            objValidateComment.SelectPost = new SelectList(_postManagement.GetPostManagementDropDownList(), "value", "name");
            objValidateComment.SelectCreator = new SelectList(_creatoresRepository.GetCreatoreDropDownList(), "value", "name");
            objValidateComment.SelectUser = new SelectList(_user.GetUserDropDownList(true), "value", "name");

            var Statusdata = _enum.GetEnumsListByType((int)EnumTypes.CommentStatus);
            if (Statusdata.Count() > 0)
                objValidateComment.SelectStatus = new SelectList(Statusdata.ToList(), "Id", "EnumValue");
        }

        public JsonResult BindPostDropdown(int CreatorId)
        {
            var posts = _postManagement.GetPostManagementDropDownListByCreatorId(CreatorId);
            return Json(posts);
            //objValidateComment.SelectPost = new SelectList(_postManagement.GetPostManagementDropDownListByCreatorId(CreatorId), "value", "name");
        }

        /// <summary>
        /// Change Status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="StatusId"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
      
        [HttpPost]
        public JsonResult UpdateStatus(int CommentId, int StatusId)
        {
            if (CommentId > 0 && StatusId > 0)
            {                
                bool isUpdated = _CommentManagement.UpdateStatus(CommentId, StatusId);
                if (isUpdated)
                    return Json(new { success = isUpdated });

                else
                    return Json(new { success = isUpdated });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid CommentId or StatusId."
                });
            }
        }
    }
}