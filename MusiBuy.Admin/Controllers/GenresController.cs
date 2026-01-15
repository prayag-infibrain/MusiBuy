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
using MusiBuy.Common.Enumeration;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class GenresController : Controller
    {
        
        private readonly IGenres _genres;
        private readonly IDropdown _dropdownrepository;
        private readonly IConfiguration _config;
        private static int _totalCount = 0;

        public GenresController(IGenres GenresRepository, IConfiguration config, IEnum enumrepo, IDropdown dropdownrepository)
        {
            this._genres = GenresRepository;
            this._config = config;
            _dropdownrepository = dropdownrepository;
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
            var result = _genres.GetGenresList(searchValue);

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
            GenresViewModel objValidateToken = new GenresViewModel();
            objValidateToken.IsActive = true;
            BindDropDown(objValidateToken);
            return View(objValidateToken);
        }

        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(GenresViewModel objValidateToken)
        {
            if (ModelState.IsValid)
            {
                //if (_genres.IsUserExists(0, (objValidateToken.Username ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                //    objValidateToken.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                //    return View(objValidateToken);
                //}

                bool isSaved = _genres.Save(objValidateToken);
                if (isSaved)
                    return RedirectToAction("Index", "Genres", new { msg = "added" });
                else
                    return RedirectToAction("Index", "Genres", new { msg = "not added" });
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
            GenresViewModel objValidateToken = _genres.GetGenresDetailsByID(id);
            if (objValidateToken == null)
                return RedirectToAction("Index", "Genres", new { msg = "drop" });
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
        public ActionResult Edit(GenresViewModel objValidateToken)
        {
            if (ModelState.IsValid)
            {
                var UserDetail = _genres.GetGenresDetailsByID(objValidateToken.Id);
                if (UserDetail == null)
                {
                    return RedirectToAction("Index", "Genres", new { msg = "drop" });
                }
                //if (_genres.IsUserExists(objValidateToken.Id, (objValidateToken.Username ?? string.Empty).Trim()))
                //{
                //    ModelState.AddModelError(string.Empty, string.Format(Messages.AlreadyExists, "Username"));
                //    objValidateToken.SelectRole = new SelectList(_roleRepository.GetRoleDropDownList(true), "value", "name");
                //    return View(objValidateToken);
                //}
                bool isUpdated = _genres.Save(objValidateToken);
                if (isUpdated)
                    return RedirectToAction("Index", "Genres", new { msg = "updated" });

                else
                    return RedirectToAction("Index", "Genres", new { msg = "not updated" });

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
                GenresViewModel objValidateToken = _genres.GetGenresDetailsByID(id);
                if (objValidateToken == null)
                    return RedirectToAction("Index", "Genres", new { msg = "drop" });
                else
                    return View(objValidateToken);
            }
            else
                return RedirectToAction("Index", "Genres", new { msg = "error" });
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
                    bool isDeleted = _genres.DeleteGenress(chkDelete);
                    if (isDeleted)
                        return RedirectToAction("Index", "Genres", new { msg = "deleted" });
                    return RedirectToAction("Index", "Genres", new { msg = "not deleted" });
                }
                else
                    return RedirectToAction("Index", "Genres", new { msg = "noselect" });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Genres", new { msg = "inuse" });
            }

        }
        #endregion


        public void BindDropDown(GenresViewModel objValidateToken)
        {
            //objValidateToken.SelectCountry = new SelectList(_dropdownrepository.GetCountryDropDownList(), "value", "name");
            // Instead of SelectList, assign List<MySelectListItem>
            objValidateToken.SelectCountry = _dropdownrepository.GetCountryDropDownList().Select(c => new MySelectListItem { Value = c.value.ToString(), Text = c.name, Key = c.key }).ToList();  // List<MySelectListItem>


            //objValidateToken.SelectRegion = new SelectList(_dropdownrepository.GetRegionDropDownList(), "value", "name");
        }
    }
}