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
using MusiBuy.Common.Repositories;
using MusiBuy.Common.Enumeration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Hosting.Server;

namespace MusiBuy.Marketing.Controllers
{
    [ValidateMarketingLogin]
    public class PostManagementController : Controller
    {
        private readonly IPostManagement _postManagementRepo;
        private readonly ICreatores _creatoresRepository;
        private readonly IEnum _enum;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public PostManagementController(IPostManagement postManagement, IConfiguration config, ICreatores creatoresRepository, IEnum enumrepo)
        {
            this._postManagementRepo = postManagement;
            this._config = config;
            _creatoresRepository = creatoresRepository;
            _enum = enumrepo;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "Post Management";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetPostManagementGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetPostManagementGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _postManagementRepo.GetPostManagementList(CurrentUserSession.User.UserID, searchValue);

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
            PostManagementViewModel objValidatePost = new PostManagementViewModel();
            BindDropdown(objValidatePost);
            return View(objValidatePost);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(PostManagementViewModel objValidatePost)
        {
            //ModelState.Remove("StatusId");
            if (ModelState.IsValid)
            {
                #region Upload Image
                string rootPath = _config.GetSection("FilePath:FilePath").Value;
                string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.PostMediaFile, objValidatePost.CreatorId.ToString());

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                if (!string.IsNullOrEmpty(objValidatePost.Url) || objValidatePost.MediaFile != null)
                {
                    if (objValidatePost.MediaFile != null)
                    {
                        if (MediaFileValidation(objValidatePost))
                        {
                            string extension = Path.GetExtension(objValidatePost.MediaFile.FileName);
                            string fileName = $"{Guid.NewGuid()}{extension}";
                            string fullPath = Path.Combine(targetPath, fileName);

                            //var path = Path.Combine(Server.MapPath("~/Uploaded/"), fileName);
                            //file.SaveAs(path);


                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                objValidatePost.MediaFile.CopyTo(stream);
                            }

                            objValidatePost.StrMediaFile = fileName;
                            objValidatePost.MediaFileName = fileName;
                        }
                        else
                        {
                            BindDropdown(objValidatePost);
                            return View(objValidatePost);

                        }
                    }
                }
                else
                {
                    BindDropdown(objValidatePost);
                    ModelState.AddModelError("MediaFile", "Enter a URL or select a media file.");
                    return View(objValidatePost);
                }

                #endregion
                objValidatePost.CreatedBy = CurrentUserSession.UserID;
                bool isSaved = _postManagementRepo.Save(objValidatePost);
                if (isSaved)
                    return RedirectToAction("Index", "PostManagement", new { msg = "added" });
                else
                    return RedirectToAction("Index", "PostManagement", new { msg = "not added" });
            }
            else
            {
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
            PostManagementViewModel objValidatePost = _postManagementRepo.GetPostManagementDetailsByID(id);
            if (objValidatePost == null)
            {
                return RedirectToAction("Index", "PostManagement", new { msg = "drop" });
            }
            BindDropdown(objValidatePost);
            return View(objValidatePost);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidatePost"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(PostManagementViewModel objValidatePost)
        {
            if (ModelState.IsValid)
            {
                var PostDetail = _postManagementRepo.GetPostManagementDetailsByID(objValidatePost.Id);
                if (PostDetail == null)
                {
                    return RedirectToAction("Index", "PostManagement", new { msg = "drop" });
                }

                #region Updaload Image
                if (!string.IsNullOrEmpty(objValidatePost.Url) || objValidatePost.MediaFile != null || !string.IsNullOrEmpty(objValidatePost.StrMediaFile))
                {
                    if (objValidatePost.MediaFile != null)
                    {
                        string rootPath = _config.GetSection("FilePath:FilePath").Value;
                        string targetPath = Path.Combine(rootPath, "wwwroot", GlobalCode.PostMediaFile, objValidatePost.CreatorId.ToString());

                        if (!Directory.Exists(targetPath))
                            Directory.CreateDirectory(targetPath);

                        if (objValidatePost.MediaFile != null)
                        {
                            if (MediaFileValidation(objValidatePost))
                            {
                                string extension = Path.GetExtension(objValidatePost.MediaFile.FileName);
                                string fileName = $"{Guid.NewGuid()}{extension}";
                                string fullPath = Path.Combine(targetPath, fileName);

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    objValidatePost.MediaFile.CopyTo(stream);
                                }

                                objValidatePost.StrMediaFile = fileName;
                            }
                        }
                    }
                }
                else
                {
                    BindDropdown(objValidatePost);
                    ModelState.AddModelError("MediaFile", "Enter a URL or select a media file.");
                    return View(objValidatePost);
                }
                #endregion

                objValidatePost.UpdatedBy = CurrentUserSession.UserID;


                bool isUpdated = _postManagementRepo.Save(objValidatePost);
                if (isUpdated)
                    return RedirectToAction("Index", "PostManagement", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "PostManagement", new { msg = "not updated" });

            }
            else
            {
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
                PostManagementViewModel objValidatePost = _postManagementRepo.GetPostManagementDetailsByID(id);
                if (objValidatePost == null)
                {
                    return RedirectToAction("Index", "PostManagement", new { msg = "drop" });
                }
                else
                {
                    return View(objValidatePost);
                }
            }
            else
            {
                return RedirectToAction("Index", "PostManagement", new { msg = "error" });
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
                    bool isDeleted = _postManagementRepo.DeletePost(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "PostManagement", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "PostManagement", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "PostManagement", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "PostManagement", new { msg = "inuse" });
            }

        }
        #endregion

        #region Delete Media File
        [HttpGet]
        public IActionResult DeleteFile(int id,int CreatorId)
        {
            string? fileName = _postManagementRepo.GetPostManagementMediaFile(id);
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new { success = false, msg = "No image to delete." });
            }

            string fileReadPath = _config.GetSection("FilePath:FilePath").Value;
            string relativePath = "/wwwroot/" + GlobalCode.PostMediaFile.Replace("\\", "/").Trim('/');
            string fullPath = $"{fileReadPath}{relativePath}/{CreatorId}/{fileName}";

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                _postManagementRepo.RemovePostManagementMediaFile(id);

                return Json(new { success = true, msg = "Profile Picture deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "Error deleting Profile Picture: " + ex.Message });
            }
        }
        #endregion

        public void BindDropdown(PostManagementViewModel objValidatePost)
        {
            objValidatePost.SelectCreator = new SelectList(_creatoresRepository.GetCreatoreDropDownList(true), "value", "name");

            //objValidatePost.SelectType = new SelectList(_enum.GetEnumsListByType((int)EnumTypes.PostMediaType), "value", "name");
            var Typedata = _enum.GetEnumsListByType((int)EnumTypes.PostMediaType);
            if (Typedata.Count() > 0)
                objValidatePost.SelectType = new SelectList(Typedata.ToList(), "Id", "EnumValue");

            ////objValidatePost.SelectStatus = new SelectList(_enum.GetEnumsListByType((int)EnumTypes.PostStatus), "value", "name");
            //var Statusdata = _enum.GetEnumsListByType((int)EnumTypes.PostMediaType);
            //if (Statusdata.Count() > 0)
            //    objValidatePost.SelectType = new SelectList(Statusdata.ToList(), "Id", "EnumValue");
        }

        public bool MediaFileValidation(PostManagementViewModel objValidatePost)
        {
            var file = objValidatePost.MediaFile;
            var contentType = file.ContentType;

            // Server-side validation
            if (objValidatePost.TypeId == (int)PostMediaTypeEnum.Audio && !contentType.StartsWith("audio/"))
            { ModelState.AddModelError("MediaFile", "Please upload a valid audio file."); return false; }
            else if (objValidatePost.TypeId == (int)PostMediaTypeEnum.Video && !contentType.StartsWith("video/"))
            { ModelState.AddModelError("MediaFile", "Please upload a valid video file."); return false; }
            else if (objValidatePost.TypeId == (int)PostMediaTypeEnum.Image && !contentType.StartsWith("image/"))
            { ModelState.AddModelError("MediaFile", "Please upload a valid image file."); return false; }
            else if (objValidatePost.TypeId == (int)PostMediaTypeEnum.Text && !(contentType == "text/plain" || contentType.Contains("pdf") || contentType.Contains("word")))
            { ModelState.AddModelError("MediaFile", "Please upload a valid text document."); return false; }

            return true;
        }

        #region Upload File
        [HttpPost]
        public async Task<IActionResult> SaveChunkedMedia(IFormFile file, [FromForm] int creatorId)
        {
            if (file == null || creatorId <= 0)
                return BadRequest("Invalid upload.");
            var rootPath = Path.Combine(_config.GetSection("FilePath:FilePath").Value, "appdata", "PostMediaFile", creatorId.ToString());

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var filePath = Path.Combine(rootPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                await file.CopyToAsync(stream);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveMedia([FromForm] string fileName, [FromForm] int creatorId)
        {
            if (string.IsNullOrEmpty(fileName) || creatorId <= 0)
                return BadRequest("Invalid file or creator.");

            var filePath = Path.Combine(_config.GetSection("FilePath:FilePath").Value, "appdata", "PostMediaFile", creatorId.ToString(), fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok();
            }

            return NotFound("File not found.");
        }
        #endregion
    }
}