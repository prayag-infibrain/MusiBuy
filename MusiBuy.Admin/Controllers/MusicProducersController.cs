using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using MusiBuy.Admin.CustomBinding;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System.Collections;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class MusicProducersController : Controller
    {
        private readonly IMusicProducers _musicProducers;
        private static int _totalCount = 0;

        public MusicProducersController(IMusicProducers musicProducers)
        {
            this._musicProducers = musicProducers;
        }

        [HttpGet]
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public IActionResult Index()
        {
            ViewBag.ModuleName = "MusicProducers";
            return View();
        }


        #region Grid Event
        [ValidatePageAccess(GlobalCode.Actions.Index)]
        public JsonResult _AjaxBinding([DataSourceRequest] DataSourceRequest request, string searchValue)
        {
            var result = new DataSourceResult()
            {
                Data = GetMusicProducersGridData(request, searchValue),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetMusicProducersGridData([DataSourceRequest] DataSourceRequest command, string searchValue)
        {
            var result = _musicProducers.GetMusicProducersList(CurrentAdminSession.User.UserID, searchValue);

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
            MusicProducersViewModel objValidateUser = new MusicProducersViewModel();
            objValidateUser.IsActive = true;
            return View(objValidateUser);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(MusicProducersViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {
                if (_musicProducers.IsMusicProducersExists(0, (objValidateUser.ProducerType ?? string.Empty).Trim()))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "ProducerType"));
                    return View(objValidateUser);
                }

                bool isSaved = _musicProducers.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "MusicProducers", new { msg = "added" });
                else
                    return RedirectToAction("Index", "MusicProducers", new { msg = "not added" });
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
            MusicProducersViewModel objValidateUser = _musicProducers.GetMusicProducersByID(id);
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
        public ActionResult Edit(MusicProducersViewModel objValidateUser)
        {
            if (ModelState.IsValid)
            {
                //if (_musicProducers.IsMusicProducersExists(0, (objValidateUser.ProducerType ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "ProducerType"));
                //    return View(objValidateUser);
                //}

                bool isSaved = _musicProducers.Save(objValidateUser);
                if (isSaved)
                    return RedirectToAction("Index", "MusicProducers", new { msg = "added" });
                else
                    return RedirectToAction("Index", "MusicProducers", new { msg = "not added" });
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
                    bool isDeleted = _musicProducers.DeleteMusicProducers(chkDelete);
                    if (isDeleted)
                    {
                        return RedirectToAction("Index", "MusicProducers", new { msg = "deleted" });

                    }
                    return RedirectToAction("Index", "MusicProducers", new { msg = "not deleted" });
                }
                else
                {
                    return RedirectToAction("Index", "MusicProducers", new { msg = "noselect" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "MusicProducers", new { msg = "inuse" });
            }

        }
        #endregion

    }
}
