using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Repositories;
using System.Collections;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class PodcastGenresController : Controller
    {
        private readonly IPodcastGenres  _podcastGenres;
        private static int _totalCount = 0;

        public PodcastGenresController(IPodcastGenres podcastGenres)
        {
            this._podcastGenres = podcastGenres;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "PodcastGenres";
            return View();
        }

        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetPodcastGenresGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetPodcastGenresGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _podcastGenres.GetPodcastGenresList(CurrentAdminSession.User.UserID, searchValue);

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
            PodcastGenresViewModel  objValidateUser = new PodcastGenresViewModel();
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(PodcastGenresViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {
                if (_podcastGenres.IsPodcastGenresExit(0, (objValidateUser.PodcastGenresName ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "InfluencerTypes"));
                    return View(objValidateUser);
                }

                bool isSaved = _podcastGenres.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "added" });
                else
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "not added" });
            }
            else
            {
                return View(objValidateUser);
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
            PodcastGenresViewModel objValidateUser = _podcastGenres.GetPodcastGenresByID(id);
            if (objValidateUser == null)
            {
                return RedirectToAction("Index", "User", new { msg = "drop" });
            }
            return View(objValidateUser);
        }

        /// <summary>
        /// User edit post method for updating user data
        /// </summary>
        /// <param name="objValidateUser"></param>
        /// <returns>If user data updated succesfully then returns success message other wise returns user view with error message</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(PodcastGenresViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {
                //if (_podcastGenres.IsPodcastGenresExit(0, (objValidateUser.PodcastGenresName ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "InfluencerTypes"));
                //    return View(objValidateUser);
                //}

                bool isSaved = _podcastGenres.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "added" });
                else
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "not added" });
            }
            else
            {
                return View(objValidateUser);
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
                    bool isDeleted = _podcastGenres.DeletePodcastGenres(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "PodcastGenres", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "PodcastGenres", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "PodcastGenres", new { msg = "inuse" });
            }

        }
        #endregion

    }
}
