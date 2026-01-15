using System;
using System.Linq;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using MusiBuy.Admin.Helper;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Common;
using MusiBuy.Admin.CustomBinding;

namespace MusiBuy.Admin.Controllers
{
    [ValidateAdminLogin]
    public class RecipientController : Controller
    {
        #region Members
        /// <summary>
        /// Dependency injections
        /// </summary>
        private static int _totalCount = 0;
        private readonly ITemplate _templateRepository;
        private readonly IRecipient _recipientRepository;

        public RecipientController(ITemplate template, IRecipient recipientMaster)
        {
            _templateRepository = template;
            _recipientRepository = recipientMaster;
        }
        #endregion

        #region Index Method
        /// <summary>
        /// Recipient index method
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns recipient index view</returns>
        //[ValidatePageAccess(GlobalCode.Actions.Index)]
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Recipient", new { Msg = "drop" });
            }
            else
            {
                var viewModel = new RecipientViewModel
                {
                    TemplateId = id,
                };
                ViewBag.ModuleName = "Recipient";
                return View(viewModel);
            }
        }
        #endregion

        #region Grid Binding Event
        /// <summary>
        /// Get recipient grid data
        /// </summary>
        /// <param name="request">Kendo grid data source request object</param>
        /// <param name="id"></param>
        /// <returns>List of recipient</returns>
        public JsonResult Recipient_Read([DataSourceRequest] DataSourceRequest request, int? id)
        {
            var result = new DataSourceResult()
            {
                Data = GetRecipientGridData(request, id),
                Total = _totalCount
            };
            return Json(result);
        }

        public IEnumerable GetRecipientGridData([DataSourceRequest] DataSourceRequest command, int? id)
        {
            var result = _recipientRepository.GetRecipientList(id);

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
        /// <summary>
        /// Recipient create get method
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns recipient create view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(int id)
        {
            TemplateViewModel obj = _templateRepository.GetTemplateByID(id);
            obj.TemplateName = _templateRepository.ToSentenceCase(obj.TemplateName ?? string.Empty);
            var viewModel = new RecipientViewModel
            {
                TemplateId = id,
                TemplateName = obj.TemplateName
            };
            return View(viewModel);

        }

        /// <summary>
        /// Recipient create post method for saving recipient data
        /// </summary>
        /// <param name="collection">If recipient data saved succesfully then returns success message other wise returns recipient view with error message</param>
        /// <returns></returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Create)]
        public ActionResult Create(RecipientViewModel recipientViewModel)
        {
            var viewModel = new RecipientViewModel
            {
                TemplateId = recipientViewModel.TemplateId,
                TemplateName = _templateRepository.GetTemplateNameById(recipientViewModel.TemplateId.HasValue ? recipientViewModel.TemplateId.Value : 0)
            };
            if (ModelState.IsValid)
            {
                if (recipientViewModel.FirstName == null || recipientViewModel.Email == null)
                {
                    ModelState.AddModelError(string.Empty, Messages.NotEnteredRequiredField);
                    return View(viewModel);
                }
                if (_recipientRepository.IsRecepientExists(recipientViewModel))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.CombinationExists, "E-mail", "template"));
                    return View(viewModel);
                }
                bool isSaved=_recipientRepository.SaveRecipient(recipientViewModel, recipientViewModel.TemplateId.HasValue ? recipientViewModel.TemplateId.Value : 0, CurrentAdminSession.User.UserID);
                if (isSaved)
                return RedirectToAction("Index", "Recipient", new { Msg = "added", id = recipientViewModel.TemplateId });
               else return RedirectToAction("Index", "Recipient", new { Msg = "not added", id = recipientViewModel.TemplateId });
            }
            else
            {
                return View(viewModel);
            }
        }
        #endregion

        #region Edit
        /// <summary>
        /// Recipient edit get method to fetch recipient data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns recipient edit view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Edit)]
        public ActionResult Edit(int id)
        {
            int? tempid = _recipientRepository.GetTemplateIdByRecipientId(id);
            RecipientViewModel obj = _recipientRepository.GetRecipientById(id);
            obj.Id = id;
            obj.TemplateId = tempid;
            obj.TemplateName = _templateRepository.ToSentenceCase(obj.TemplateName ?? string.Empty);
            return View(obj);
        }

        /// <summary>
        /// Recipient edit post method for updating recipient data
        /// </summary>
        /// <param name="collection"></param>
        /// <returns>If enum recipient updated succesfully then returns success message other wise returns recipient view with error message</returns>
        [HttpPost]
        public ActionResult Edit(RecipientViewModel recipientViewModel)
        {
            int? tempid = _recipientRepository.GetTemplateIdByRecipientId(recipientViewModel.Id.HasValue ? recipientViewModel.Id.Value : 0);
            RecipientViewModel obj = _recipientRepository.GetRecipientById(recipientViewModel.Id.HasValue ? recipientViewModel.Id.Value : 0);
            var viewModel = new RecipientViewModel
            {
                TemplateId = tempid,
                TemplateName = obj.TemplateName
            };
            if (ModelState.IsValid)
            {
                if (_recipientRepository.IsRecepientExists(recipientViewModel))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Messages.CombinationExists, "E-mail", "template"));
                    return View(viewModel);
                }
                bool isUpadted=_recipientRepository.SaveRecipient(recipientViewModel,0, CurrentAdminSession.User.UserID);
                if (isUpadted)
                return RedirectToAction("Index", "Recipient", new { Msg = "updated", id = tempid });
                else return RedirectToAction("Index", "Recipient", new { Msg = "updated", id = tempid });
            }
            else
            {
                return View(viewModel);
            }
        }
        #endregion

        #region Recipient Detail
        /// <summary>
        /// Recipient detail get method to fetch recipient data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns recipient detail view</returns>
        [ValidatePageAccess(GlobalCode.Actions.Detail)]
        public ActionResult Detail(int id)
        {
            if (id > 0)
            {
                RecipientViewModel obj = _recipientRepository.GetRecipientById(id);
                obj.TemplateName = _templateRepository.ToSentenceCase(obj.TemplateName ?? string.Empty);
                if (obj == null)
                {
                    return RedirectToAction("Index", "Recipient", new { msg = "drop" });
                }
                else
                {
                    return View(obj);
                }
            }
            else
            {
                return RedirectToAction("Index", "Recipient", new { msg = "error" });
            }
        }
        #endregion

        #region Recipient Delete
        /// <summary>
        /// Delete recipient post method
        /// </summary>
        /// <param name="chkDelete"></param>
        /// <returns>If recipient data deleted succesfully then returns success message other wise returns error message on recipient list page</returns>
        [HttpPost]
        [ValidatePageAccess(GlobalCode.Actions.Delete)]
        public ActionResult Delete(int[] chkDelete)
        {
            try
            {
                if (chkDelete.Length > 0)
                {
                    int? tempid = _recipientRepository.GetTemplateIdByRecipientId(chkDelete[0]);
                    foreach (int recepientID in chkDelete)
                    {
                        _recipientRepository.DeleteRecipient(chkDelete);
                    }
                    return RedirectToAction("Index", "Recipient", new { Msg = "deleted", id = tempid });

                }
                else
                {
                    return RedirectToAction("Index", "Recipient", new { Msg = "NoSelect" });
                }
            }
            catch (Exception)
            {
                    return RedirectToAction("Index", "Recipient", new { Msg = "inuse" });
              }
        }
        #endregion

        #region Duplication Validation
        /// <summary>
        /// Check if recipient already exists
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="newValue"></param>
        /// <param name="recipientID"></param>
        /// <returns>If recipient already exists then returns 0 otherwise 1 in json</returns>
        [HttpPost]
        public JsonResult ValidateDuplicateRecipientCombination(int templateID, string newValue, int recipientID)
        {
            RecipientViewModel recipientViewModel = new RecipientViewModel { TemplateId = templateID, Id = recipientID, Email = newValue };
            var res = _recipientRepository.GetRecipientViewList(recipientViewModel);
            if (res != null && res.Count() > 0)
            {
                return Json(new { @status = "0" });
            }
            else
            {
                return Json(new { @status = "1" });
            }
        }
        #endregion
    }
}