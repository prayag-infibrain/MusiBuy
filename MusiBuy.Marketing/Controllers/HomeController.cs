using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Models;
using MusiBuy.Common.Common;
using MusiBuy.Common.Interfaces;
using MusiBuy.Marketing.Helper;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Repositories;
using MusiBuy.Common.Enumeration;
using System.Reflection;
using Kendo.Mvc.Extensions;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MusiBuy.Common.Interfaces.Marketing;

namespace MusiBuy.Marketing.Controllers
{
    public class HomeController : MusiBuy
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly IMarketingUsersLogin _loginRepository;
        private readonly IConfiguration _config;
        private readonly IRolePrivileges _rolePrivilegesRepository;
        private readonly IContentManagement _contentManagement;
        private readonly IPostManagement _postManagement;

        private readonly IUser _userRepository;
        private readonly ICreatores _creatoresRepository;


        public HomeController(IConfiguration config, IMarketingUsersLogin login, IRolePrivileges rolePrivilegesRepository, IContentManagement content, IUser user, ICreatores creatores, IPostManagement postManagement)
        {
            _loginRepository = login;
            _rolePrivilegesRepository = rolePrivilegesRepository;
            _config = config;
            _contentManagement = content;
            _userRepository = user;
            _creatoresRepository = creatores;
            _postManagement = postManagement;
        }
        #endregion

        #region Index
        /// <summary>
        /// Action to render view 
        /// </summary>
        /// <returns>Returns  view</returns>
        public IActionResult Index()
        {

            return View();
        }


        #region  Privacy
        public IActionResult PrivacyPolicy()
        {
            ContentManagementsViewModel model = new ContentManagementsViewModel();
            ContentManagementsViewModel smsContent = _contentManagement.GetContentManagementsContentByID((int)ContentEnum.Privacy);
            if (smsContent != null && !string.IsNullOrEmpty(smsContent.Content))
            {
                model.Content = smsContent.Content;
            }
            return View(model);
        }
        #endregion

        #region  Terms
        public IActionResult TermsAndCondition()
        {
            ContentManagementsViewModel model = new ContentManagementsViewModel();
            ContentManagementsViewModel smsContent = _contentManagement.GetContentManagementsContentByID((int)ContentEnum.Terms);
            if (smsContent != null && !string.IsNullOrEmpty(smsContent.Content))
            {
                model.Content = smsContent.Content;
            }
            return View(model);
        }
        #endregion

        /// <summary>
        /// Post method for index view
        /// </summary>
        /// <param name="loginviewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(LoginViewModel loginviewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginviewModel);
            }

            MarketingUserViewModel objUserViewModel = _loginRepository.ValidateLogin(loginviewModel);

            if (objUserViewModel != null && objUserViewModel.Id > 0)
            {
                CurrentAdminUser User = new CurrentAdminUser();
                User.RoleName = objUserViewModel.UserTypeId == (int)MarketingUserTypeEnum.Publishers ? "Publishers"  : "Advertiser";
                User.UserID = objUserViewModel.Id;
                User.RoleID = objUserViewModel.UserTypeId;
                User.FirstName = objUserViewModel.FirstName ?? string.Empty;
                User.LastName = objUserViewModel.LastName ?? string.Empty;
                User.UserName = objUserViewModel.Username ?? string.Empty;
                User.Email = objUserViewModel.Email ?? string.Empty;
                CurrentUserSession.User = User;

                //var rolePrivilegesList = _rolePrivilegesRepository.GetRolePrivilegesByRoleId(objUserViewModel.RoleId);

                //CurrentRoleMenus roleMenu = new CurrentRoleMenus();
                //roleMenu.ViewRoleMenu = rolePrivilegesList.Select(x => x.MenuItem).ToList();
                //CurrentUserSession.ViewRoleMenus = roleMenu;

                //CurrentRolePrivileges rolePrivileges = new CurrentRolePrivileges();
                //rolePrivileges.ViewRolePrivileges = rolePrivilegesList;
                //CurrentUserSession.ViewRolePrivileges = rolePrivileges;

                return RedirectToAction("Home", "Home");
            }
            else
            {
                ModelState.AddModelError("", Messages.Invalidlogin);
                return View(loginviewModel);
            }
        }
        #endregion

        #region Home
        /// <summary>
        /// Action to render view 
        /// </summary>
        /// <returns>Returns Home view</returns>
        [ValidateMarketingLogin]
        public IActionResult Home(HomeViewModel homeView)
        {
            homeView.TotalUser = _userRepository.GetUserCount();
            homeView.TotalCreators = _creatoresRepository.GetCreatoreCount();
            //homeView.TotalTokenPuchased = _insuranceManagement.GetInsuranceCount();
            //homeView.RevenueGenerated = _insuranceManagement.GetInsuranceDueCount();
            
            Dictionary<string, int> Count = _postManagement.PostCountByType();
            homeView.TotalAudioPost = Count.ContainsKey("Audio") ? Count["Audio"] : 0;
            homeView.TotalVideoPost = Count.ContainsKey("Video") ? Count["Video"] : 0;
            homeView.TotalImagePost = Count.ContainsKey("Image") ? Count["Image"] : 0;
            homeView.TotalTextPost = Count.ContainsKey("Text") ? Count["Text"] : 0;

            //homeView.RevenueGenerated = _insuranceManagement.GetInsuranceDueCount();
            return View(homeView);
        }
        #endregion

        #region Logout
        /// <summary>
        /// Logout Method
        /// </summary>
        /// <returns>If user is logged out successfully it redirects to login page</returns>
        public ActionResult Logout()
        {
            SessionManager.ClearSession();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Success
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Success()
        {
            return View();
        }
        #endregion

        #region UploadImage
        /// <summary>
        /// Upload image
        /// </summary>
        /// <param name="file"></param>
        /// <returns>If image is uploaded successfully it returns image path with json response</returns>
        [ValidateMarketingLogin]
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            var path = Convert.ToString(_config.GetSection("FilePath:FilePath").Value);
            var readPath = Convert.ToString(_config.GetSection("FilePath:FileReadPath").Value);
            readPath = readPath + Path.Combine(GlobalCode.UserFiles, file.FileName).Replace("//", "/");

            string filePath = Path.Combine(path ?? string.Empty, "wwwroot//", GlobalCode.UserFiles, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return Json(new { data = readPath });
        }
        #endregion

        #region Editor Token
        /// <summary>
        /// Editor token 
        /// </summary>
        /// <returns>Editor token</returns>
        public IActionResult EditorToken()
        {
            return Json(Encryption.DecryptWithUrlDecode(GlobalCode.RandomString(15)));
        }
        #endregion

        #region Ganerate Profile Picture
        public IActionResult SvgAvatar(string userName)
        {
            string initials = GetInitials(userName);
            var svg = $@"<svg width='100' height='100' xmlns='http://www.w3.org/2000/svg'>
                        <rect width='100' height='100' fill='#1ED760'/>
                        <text x='50%' y='50%' dominant-baseline='middle' text-anchor='middle' fill='#0A364E' font-family='Arial' font-size='32'>
                            {initials}
                        </text>
                        </svg>";
            return Content(svg, "image/svg+xml");
        }
        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "NA";
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 1
                ? parts[0][0].ToString().ToUpper()
                : (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
        }
        #endregion

    }
}
