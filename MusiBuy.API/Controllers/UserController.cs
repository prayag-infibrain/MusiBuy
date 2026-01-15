using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Enumeration.API;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.Repositories;
using MusiBuy.Common.ResponseModels;
using System.Linq;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;

namespace MusiBuy.API.Controllers
{
    //[Authorize]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Member Declaration
        private readonly IFrontUser _User;
        private readonly IConfiguration _config;
        private readonly ICommonSetting _commonSettingRepository;
        private readonly IDropdown _dropdown;
        private readonly IPostManagement _postmanagement;
        private readonly IContactUs _contactUs;
        private readonly ITemplate _templateRepository;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticate"></param>
        public UserController(IConfiguration configuration, IFrontUser User, ICommonSetting commonSettingRepository, IDropdown dorpdown, IPostManagement postmanagement, IContactUs contactUs, ITemplate templateRepository)
        {
            this._User = User;
            this._config = configuration;
            _commonSettingRepository = commonSettingRepository;
            _dropdown = dorpdown;
            _postmanagement = postmanagement;
            _contactUs = contactUs;
            _templateRepository = templateRepository;
        }
        #endregion

        #region Get Profile By Id
        [HttpPost("GetUserProfile")]
        [EndpointSummary("GetUserProfile")]
        [EndpointDescription("Get User Profile Record For Edit")]
        [EndpointName("GetUserProfile")]
        public IActionResult GetUserProfile(CommonModel model)
        {
            if (model.Id == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            var User = _User.GetFrontUserDetailById(model.Id);
            if (User == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));

            var response = new
            {
                UserId = User.Id,
                UserTypeId = User.UserTypeId,
                RoleId = User.RoleId,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                CountryId = User.CountryId,
                MobileNumber = User.Mobile,
                Bio = User.Bio,
                Role = User.RoleName,
                IsActive = User.IsActive,
                ImageUrl = User.StrImage,
                UserPrefrence = User.UserPrefrence,
                Followers = User.Followers,
                Followings = User.Followings,
                TokenErned = User.TokenErned,
                //PostList = Creator.PostList,
            };
            return Ok(new ApiResponse(true, "Sucess", response));
        }
        #endregion

        #region Update User Profile
        [HttpPost("UpdateUserProfile")]
        [EndpointSummary("UpdateUserProfile")]
        [EndpointDescription("Update User Profile by Id")]
        [EndpointName("UpdateUserProfile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] FrontUserUpdateProfileModel userViewModel)
        {
            //if (_User.IsUserExists(0, (userViewModel.Username ?? string.Empty).Trim()))
            //    return Ok(new ApiResponse(false, "Username Already Exist", null));

            if (userViewModel.Id == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            if (userViewModel.UserTypeId == (int)APIUserTypeEnum.Creator)
            {
                if (userViewModel.RoleId == 0)
                    return Ok(new ApiResponse(false, "Invalid Role", null));
            }
            else if (userViewModel.UserTypeId == (int)APIUserTypeEnum.User)
                userViewModel.RoleId = 0;
            else
                return Ok(new ApiResponse(false, "Invalid User Type Id", null));


            if (_User.IsEmailExists(userViewModel.Id, (userViewModel.Email ?? string.Empty).Trim()))
                return Ok(new ApiResponse(false, "Email Address Already Exist", null));

            if (userViewModel.CountryId == 0)
                return Ok(new ApiResponse(false, "Select Country", null));

            var User = _User.GetFrontUserDetailById(userViewModel.Id);
            if (User == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));

            #region Save Images
            string? uniqueFileName = null;
            if (userViewModel.ProfileImage != null && userViewModel.ProfileImage.Length > 0)
            {
                string imagePath = _config.GetSection("FilePath:FilePath").Value;  // e.g. C:\Projects\Uploads
                string imgUpload = Path.Combine(imagePath, "wwwroot", GlobalCode.FrontUserImages);

                if (!Directory.Exists(imgUpload))
                    Directory.CreateDirectory(imgUpload);

                string fileExt = Path.GetExtension(userViewModel.ProfileImage.FileName);
                uniqueFileName = Guid.NewGuid() + fileExt;
                string filePath = Path.Combine(imgUpload, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userViewModel.ProfileImage.CopyToAsync(stream);
                }
            }
            #endregion

            List<int> userPref = new List<int>();
            if (!string.IsNullOrEmpty(userViewModel.UserPrefrence))
            {
                userPref = userViewModel.UserPrefrence.Split(',').Select(x => int.Parse(x)).ToList();
            }

            FrontUserViewModel frontUserViewModel = new FrontUserViewModel();
            frontUserViewModel.Id = userViewModel.Id;
            frontUserViewModel.UserTypeId = userViewModel.UserTypeId;
            frontUserViewModel.RoleId = userViewModel.RoleId;
            frontUserViewModel.FirstName = userViewModel.FirstName;
            frontUserViewModel.LastName = userViewModel.LastName;
            frontUserViewModel.Email = userViewModel.Email;
            frontUserViewModel.Mobile = userViewModel.Mobile;
            frontUserViewModel.CountryId= userViewModel.CountryId;
            frontUserViewModel.Bio = userViewModel.Bio;
            frontUserViewModel.UserPrefrence = userPref;
            frontUserViewModel.IsActive = true;
            frontUserViewModel.Username = "";
            frontUserViewModel.StrImage = uniqueFileName ?? User.ImageName;

            if (_User.Save(frontUserViewModel))
                return Ok(new ApiResponse(true, "Profile Update Successful", null));
            else
                return Ok(new ApiResponse(false, "Error While Save Record", null));
        }
        #endregion

        #region Update Password
        [HttpPost("ChangePassword")]
        [EndpointSummary("ChangePassword")]
        [EndpointDescription("Change Password by UserId")]
        [EndpointName("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword ChngePassword)
        {
            if (ChngePassword.Id == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User", null));
            }
            if (string.IsNullOrWhiteSpace(ChngePassword.Oldpassword))
            {
                return Ok(new ApiResponse(false, "Enter Old Password", null));
            }
            if (string.IsNullOrWhiteSpace(ChngePassword.Newpassword))
            {
                return Ok(new ApiResponse(false, "Enter New Password", null));
            }
            if (ChngePassword.Oldpassword == ChngePassword.Newpassword)
            {
                return Ok(new ApiResponse(false, "Old And New Password are not Same", null));
            }
            var (Status, Msg) = _User.ChangePasswordByUserId(ChngePassword.Id, ChngePassword.Oldpassword, ChngePassword.Newpassword);
            return Ok(new ApiResponse(Status, Msg, null));
        }
        #endregion

        #region Get Post And Event By User Id For Home Screen
        [HttpPost("GetHomeScreenForUser")]
        [EndpointSummary("GetHomeScreenForUser")]
        [EndpointDescription("Get Event And Post By User Id For Get Tranding And Latest Content")]
        [EndpointName("GetHomeScreenForUser")]
        public async Task<IActionResult> GetHomeScreenForUser(GetHomeScreenRecordCount model)
        {
            //if (model.Id == 0)
            //    return Ok(new ApiResponse(false, "Invalid User", null));

            int takeData = model.DisplayRecordCount == 0 ? 10 : model.DisplayRecordCount;
            int UserId = model.UserId;//Use For Get Personalized Content (Pending)
            var HomeScreenList = await _postmanagement.GetHomeScreenData(UserId);

            if (HomeScreenList == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));
            if (HomeScreenList.Count == 0)
                return Ok(new ApiResponse(false, "No Record Found", null));

            var eventList = HomeScreenList.Where(a => a.CreateType == (int)ApiContentType.Event);
            var postList = HomeScreenList.Where(a => a.CreateType == (int)ApiContentType.Post);

            #region Search & Filter
            //Search
            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                string search = model.Search.Trim().ToLower();
                eventList = eventList.Where(x => x.Title != null && x.Title.ToLower().Contains(search));
                postList = postList.Where(x => x.Title != null && x.Title.ToLower().Contains(search));
            }

            //Filter : CreatorTypeId
            if (model.CreatorTypeId != null && model.CreatorTypeId.Any())
            {
                var creatorIds = model.CreatorTypeId;
                eventList = eventList.Where(x => creatorIds.Contains(x.CreatorTypeId));
                postList = postList.Where(x => creatorIds.Contains(x.CreatorTypeId));
            }

            //Filter : ContentType
            if (model.ContentTypeId != null && model.ContentTypeId.Any())
            {
                var contentIds = model.ContentTypeId;
                postList = postList.Where(x => contentIds.Contains(x.PostMediaTypeId));
            }
            #endregion

            var trendingList = postList.OrderByDescending(x => x.LikeCount).Take(takeData).Select(MapPost).ToList();
            var latestList = postList.OrderByDescending(x => x.CreatedOn).Take(takeData).Select(MapPost).ToList();
            var PersonalizedList = new List<object>();
            #region Get Personalized List By User
            if (model.UserId != 0)
            {
                List<int> PrefIds = _postmanagement.GetuserPrefIds(model.UserId);
                if (PrefIds.Count > 0 && PrefIds.Any())
                {
                    PersonalizedList = postList.Where(a => PrefIds.Contains(a.PostContentTypeId)).Take(takeData).Select(MapPost).ToList();
                }
            }
            #endregion

            var eventResponse = eventList.Select(e => new
            {
                Id = e.Id,
                Status = e.Status,
                CreateType = e.CreateType,
                Title = e.Title,
                CreatorName = e.CreatorName,
                CreatorProfileUrl = e.CreatorProfileUrl,
                Description = e.Description,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                EventTypeId = e.EventTypeId,
                EventStatusId = e.EventStatusId,
                IsLike = e.IsLike,
                TotalLike = e.LikeCount,
                TotalComments = e.CommentsCount,
                TotalView = e.ViewCount,
                Token = e.Token
            }).ToList();
            object MapPost(HomeScreenContent p) => new
            {
                Id = p.Id,
                Status = p.Status,
                CreateType = p.CreateType,
                Title = p.Title,
                CreatorName = p.CreatorName,
                CreatorProfileUrl = p.CreatorProfileUrl,
                Description = p.Description,

                MediaFile = p.StrMediaFile,
                MediaFileSize = p.StrMediaFileSize,
                PostMediaTypeId = p.PostMediaTypeId,
                Tags = p.Tags,

                PostTypeId = p.PostTypeId,
                PostTypeName = p.PostTypeName,
                PostContentTypeId = p.PostContentTypeId,
                PostContentTypeName = p.PostContentTypeName,
                CountryId = p.CountryId,
                CountryName = p.CountryName,
                GenreId = p.GenreId,
                GenreName = p.GenreName,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,

                IsLike = p.IsLike,
                TotalLike = p.LikeCount,
                TotalComments = p.CommentsCount,
                TotalView = p.ViewCount,
                Token = p.Token,
                DefaultImage = p.DefaultImage,
                DefaultIcon = p.DefaultIcon
            };

            var combinedList = trendingList.Concat(latestList).Concat(PersonalizedList).Concat(eventResponse).ToList();
            string listName = model.GetListByName?.Trim().ToLower();
            object result = listName switch
            {
                "event" => new { Event = eventResponse },
                "trendinglist" => new { TrendingList = trendingList },
                "latestlist" => new { LatestList = latestList },
                "personalizedlist" => new { PersonalizedList = PersonalizedList },
                "all" => new { CombinedList = combinedList },
                _ => new
                {
                    Event = eventResponse,
                    TrendingList = trendingList,
                    LatestList = latestList,
                    PersonalizedList = PersonalizedList,
                    CombinedList = combinedList
                }
            };

            return Ok(new ApiResponse(true, "Success", result));
        }
        #endregion

        #region Contact Us
        [HttpPost("ContactUs")]
        [EndpointSummary("ContactUs")]
        [EndpointDescription("APi For Add Recod in Contact Us")]
        [EndpointName("ContactUs")]
        public IActionResult ContactUs(ContactUsApiModel contactUsmodel)
        {
            if (contactUsmodel.UserId == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            if (string.IsNullOrWhiteSpace(contactUsmodel.Email))
                return Ok(new ApiResponse(false, "Enter Email", null));
            if (string.IsNullOrWhiteSpace(contactUsmodel.Subject))
                return Ok(new ApiResponse(false, "Enter Subject", null));
            if (string.IsNullOrWhiteSpace(contactUsmodel.Message))
                return Ok(new ApiResponse(false, "Enter Message", null));


            if (contactUsmodel != null && !string.IsNullOrWhiteSpace(contactUsmodel.Email))
            {
                var Replaytemplate = _templateRepository.GetEmailTemplateByName(TemplateEnum.ContactUsReply.ToString());
                var ContactUstemplate = _templateRepository.GetEmailTemplateByName(TemplateEnum.ContactUs.ToString());

                if (Replaytemplate.TemplateContent != null && ContactUstemplate.TemplateContent != null)
                {
                    var commonSetting = _commonSettingRepository.GetCommonSetting();

                    string ReplayemailBody = Replaytemplate.TemplateContent;
                    ReplayemailBody = PopulateEmailBody(contactUsmodel, ReplayemailBody, commonSetting.SiteURL ?? string.Empty);
                    bool Replaystatus = GlobalCode.SendEmailForContactUs(commonSetting.Email,contactUsmodel.Email, Replaytemplate.Subject ?? string.Empty, ReplayemailBody, commonSetting);

                    string ContactemailBody = ContactUstemplate.TemplateContent;
                    ContactemailBody = PopulateEmailBody(contactUsmodel, ContactemailBody, commonSetting.SiteURL ?? string.Empty);
                    bool ContactUsstatus = GlobalCode.SendEmailForContactUs(commonSetting.Email, commonSetting.Email, ContactUstemplate.Subject ?? string.Empty, ContactemailBody, commonSetting);


                    if (Replaystatus && ContactUsstatus)
                        return Ok(new ApiResponse(true, "Your message has been sent. We'll contact you soon!", null));
                    else
                        return Ok(new ApiResponse(false, "Error While Sending Mail!", null));
                }
                else
                    return Ok(new ApiResponse(false, "Email Content Not Found", null));
            }
            else
                return Ok(new ApiResponse(false, "Email Not Found", null));
        }
        private string PopulateEmailBody(ContactUsApiModel contactUsmodel, string emailBody, string siteUrl)
        {
            emailBody = emailBody.Replace("###SiteUrl###", Convert.ToString(siteUrl));
            emailBody = emailBody.Replace("###FullName###", Convert.ToString(contactUsmodel.FullName));
            emailBody = emailBody.Replace("###Email###", Convert.ToString(contactUsmodel.Email));
            emailBody = emailBody.Replace("###Subject###", Convert.ToString(contactUsmodel.Subject));
            emailBody = emailBody.Replace("###Message###", Convert.ToString(contactUsmodel.Message));
            emailBody = emailBody.Replace("###Year###", DateTime.UtcNow.Year.ToString());

            return emailBody;
        }
        #endregion
    }
}
