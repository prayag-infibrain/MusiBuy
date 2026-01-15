using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Helper;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using System;
using System.Globalization;
using System.Net.NetworkInformation;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;

namespace MusiBuy.API.Controllers
{
    //[Authorize]
    [Route("api/PostAndEvent")]
    [ApiController]
    public class PostAndEventController : ControllerBase
    {
        #region Member Declaration
        private readonly IConfiguration _config;
        private readonly ICommonSetting _commonSettingRepository;
        private readonly IDropdown _dropdown;
        private readonly IPostManagement _postmanagement;
        private readonly IEventManagement _eventmanagement;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authenticate"></param>
        public PostAndEventController(IConfiguration configuration, ICommonSetting commonSettingRepository, IDropdown dorpdown, IPostManagement postmanagement, IEventManagement eventmanagement)
        {
            this._config = configuration;
            _commonSettingRepository = commonSettingRepository;
            _dropdown = dorpdown;
            _postmanagement = postmanagement;
            _eventmanagement = eventmanagement;
        }
        #endregion

        #region Create Event
        [HttpPost("SaveEvent")]
        [EndpointSummary("SaveEvent")]
        [EndpointDescription("Add/Update Event By User Id")]
        [EndpointName("SaveEvent")]
        public IActionResult SaveEvent(CreateEventViewModel eventViewModel)
        {
            //if (_User.IsUserExists(0, (userViewModel.Username ?? string.Empty).Trim()))
            //    return Ok(new ApiResponse(false, "Username Already Exist", null));


            if (eventViewModel.UserTypeId == 0)
                return Ok(new ApiResponse(false, "Invalid User Type Id", null));
            else if (eventViewModel.UserTypeId == 2)
            {
                if (eventViewModel.RoleId == 0)
                    return Ok(new ApiResponse(false, "Invalid Role Id", null));
            }

            DateTime Start_Date = DateTime.Now;
            DateTime End_Date = DateTime.Now;

            if (!string.IsNullOrEmpty(eventViewModel.StartDate))
                Start_Date = DateTime.ParseExact(eventViewModel.StartDate, "MMM dd, yyyy hh:mm tt", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(eventViewModel.EndDate))
                End_Date = DateTime.ParseExact(eventViewModel.EndDate, "MMM dd, yyyy hh:mm tt", CultureInfo.InvariantCulture);


            EventManagementViewModel eventManagement = new EventManagementViewModel();
            eventManagement.Id = eventViewModel.Id;
            eventManagement.UserTypeId = eventViewModel.UserTypeId;
            eventManagement.RoleId = eventViewModel.RoleId;
            eventManagement.UserId = eventViewModel.UserId;
            eventManagement.Title = eventViewModel.Title;
            eventManagement.Description = eventViewModel.Description;
            eventManagement.CreatorId = eventViewModel.UserId;
            eventManagement.EventTypeId = eventViewModel.TypeId;
            eventManagement.EventStartDateTime = Start_Date; //Convert.ToDateTime(eventViewModel.StartDate);
            eventManagement.EventEndDateTime = End_Date; //Convert.ToDateTime(eventViewModel.EndDate);
            eventManagement.StatusId = (int)EventStatusEnum.Upcoming;//Upcoimming//eventViewModel.StatusId;
            eventManagement.CreatedBy = eventViewModel.UserId;
            eventManagement.RecordingURL = "";
            if (eventManagement.Id > 0)
                eventManagement.UpdatedBy = eventViewModel.UserId;
            else
                eventManagement.CreatedBy = eventViewModel.UserId;

            if (_eventmanagement.Save(eventManagement))
                return Ok(new ApiResponse(true, "Event created successfully", null));
            else
                return Ok(new ApiResponse(false, "Error While Create Event", null));
        }
        #endregion

        #region Create Post
        [HttpPost("SavePost")]
        [EndpointSummary("SavePost")]
        [EndpointDescription("Save Post By Creator Id (Add/Edit)")]
        [EndpointName("SavePost")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SavePost([FromForm] CreatePostViewModel PostViewModel)
        {
            //if (_User.IsUserExists(0, (userViewModel.Username ?? string.Empty).Trim()))
            //    return Ok(new ApiResponse(false, "Username Already Exist", null));


            if (PostViewModel.UserTypeId == 0)
                return Ok(new ApiResponse(false, "Invalid User Type Id", null));
            else if (PostViewModel.UserTypeId == 2)
            {
                if (PostViewModel.RoleId == 0)
                    return Ok(new ApiResponse(false, "Invalid Role Id", null));
            }


            var PostData = _postmanagement.GetPostManagementDetailsByIDAPI(PostViewModel.Id);
            if (PostViewModel.Id > 0 && PostData == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));


            #region Save Images
            string? uniqueFileName = null;
            if (PostViewModel.MediaFile != null && PostViewModel.MediaFile.Length > 0)
            {
                string ImagePath = CommonHelper.GetPostImagePath(_config, PostViewModel.UserTypeId, PostViewModel.UserId);

                string fileExt = Path.GetExtension(PostViewModel.MediaFile.FileName);
                uniqueFileName = Guid.NewGuid() + fileExt;
                string filePath = Path.Combine(ImagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PostViewModel.MediaFile.CopyToAsync(stream);
                }
            }
            #endregion


            PostManagementViewModel PostManagement = new PostManagementViewModel();
            PostManagement.Id = PostViewModel.Id;
            PostManagement.UserTypeId = PostViewModel.UserTypeId;
            PostManagement.RoleId = PostViewModel.RoleId;
            PostManagement.UserId = PostViewModel.UserId;
            PostManagement.Title = PostViewModel.Title;
            PostManagement.Description = PostViewModel.Description;
            PostManagement.Tags = PostViewModel.Tags;
            PostManagement.ContentTypeId = PostViewModel.ContentTypeId;
            PostManagement.CountryId = PostViewModel.CountryId;
            PostManagement.GenreId = PostViewModel.GenreId;
            PostManagement.CategoryId = PostViewModel.CategoryId;
            PostManagement.TypeId = PostViewModel.TypeId;
            PostManagement.MediaTypeId = PostViewModel.MediaTypeId;
            PostManagement.MediaFileName = uniqueFileName ?? PostData.StrMediaFileName;
            PostManagement.IsActive = true;
            if (PostViewModel.Id > 0)
                PostManagement.UpdatedBy = PostViewModel.UserId;
            else
                PostManagement.CreatedBy = PostViewModel.UserId;



            if (_postmanagement.Save(PostManagement))
                return Ok(new ApiResponse(true, "Post Save successfully", null));
            else
                return Ok(new ApiResponse(false, "Error While Save Post", null));
        }
        #endregion

        #region Get Post By User Id
        //[HttpPost("GetPostByUserId")]
        //[EndpointSummary("GetPostByUserId")]
        //[EndpointDescription("Get Post By User Id And Type Id")]
        //[EndpointName("GetPostByUserId")]
        //public IActionResult GetPostByUserId(GetByIdAndType model)
        //{
        //    if (model.Id == 0)
        //        return Ok(new ApiResponse(false, "Invalid User", null));

        //    var PostList = _postmanagement.GetPostManagementDetailsByUserID(model.Id, model.TypeId);
        //    if (PostList.Count == 0)
        //        return Ok(new ApiResponse(false, "No Record Found", null));

        //    if (PostList == null)
        //        return Ok(new ApiResponse(false, "Error While Fatch Data", null));

        //    var response = PostList.Select(Post => new
        //    {
        //        Id = Post.Id,
        //        UserId = Post.UserId,
        //        UserTypeId = Post.UserTypeId,
        //        CreatorName = Post.CreatorName,
        //        Title = Post.Title,
        //        Description = Post.Description,
        //        Tags = Post.Tags,
        //        ContentTypeId = Post.ContentTypeId,
        //        CountryId = Post.CountryId,
        //        GenreId = Post.GenreId,
        //        CategoryId = Post.CategoryId,
        //        MediaTypeId = Post.MediaTypeId,
        //        TypeId = Post.TypeId,
        //        MediaFile = Post.StrMediaFile,
        //        MediaFileSize = Post.MediaFileSize,
        //        Url = Post.Url,
        //        LikeCount = 0,
        //        CommentsCount = 0,
        //        ViewCount = 0,
        //        //PostList = Creator.PostList,
        //    }).ToList();
        //    return Ok(new ApiResponse(true, "Sucess", response));
        //}
        #endregion

        #region Delete Content By Id
        [HttpPost("DeleteContentById")]
        [EndpointSummary("DeleteContentById")]
        [EndpointDescription("Delete Post/Event By Id")]
        [EndpointName("DeleteContentById")]
        public IActionResult DeleteContentById(DeleteContent model)
        {
            if (model.Id == 0)
                return Ok(new ApiResponse(false, "Invalid Data", null));

            bool IsDelete = _postmanagement.DeleteContentById(model);
            if (IsDelete)
                return Ok(new ApiResponse(true, "Sucess", null));
            else
                return Ok(new ApiResponse(false, "Error While Delete Post", null));
        }
        #endregion

        #region Get Post And Event By User Id And TypeId For Profile
        [HttpPost("GetPostByUserId")]
        [EndpointSummary("GetPostByUserId")]
        [EndpointDescription("Get Post And Event By User Id And Type Id")]
        [EndpointName("GetPostByUserId")]
        public async Task<IActionResult> GetPostByUserId(GetUserProfileDataById model)
        {
            if (model.Id == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));

            var HomeScreenList = await _postmanagement.GetHomeScreenData(model.Id);

            if (HomeScreenList == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));

            if (HomeScreenList.Count == 0)
                return Ok(new ApiResponse(false, "No Record Found", null));

            if (model.Id > 0)
                HomeScreenList = HomeScreenList.Where(a => a.UserId == model.Id).ToList();
            else
                return Ok(new ApiResponse(false, "Invalid User", null));



            if (model.createTypeId != 0)
                HomeScreenList = HomeScreenList.Where(a => a.CreateType == model.createTypeId).ToList();

            if (model.MediaTypeId != 0)
                HomeScreenList = HomeScreenList.Where(a => a.PostMediaTypeId == model.MediaTypeId).ToList();



            var response = HomeScreenList.Select(Post => new
            {
                Id = Post.Id,
                Status = Post.Status,
                CreateType = Post.CreateType,   // 1 = Event, 2 = Post
                Title = Post.Title,
                CreatorName = Post.CreatorName,
                Description = Post.Description,

                // EVENT Fields
                StartDateTime = Post.StartDateTime,
                EndDateTime = Post.EndDateTime,
                EventTypeId = Post.EventTypeId,
                EventStatusId = Post.EventStatusId,

                // POST Fields
                MediaFile = Post.StrMediaFile,
                MediaFileSize = Post.StrMediaFileSize,
                PostMediaTypeId = Post.PostMediaTypeId,
                Tags = Post.Tags,

                //Extra Fields For Post
                PostTypeId = Post.PostTypeId,
                PostTypeName = Post.PostTypeName,
                PostContentTypeId = Post.PostContentTypeId,
                PostContentTypeName = Post.PostContentTypeName,
                CountryId = Post.CountryId,
                CountryName = Post.CountryName,
                GenreId = Post.GenreId,
                GenreName = Post.GenreName,
                CategoryId = Post.CategoryId,
                CategoryName = Post.CategoryName,

                // Home UI Extra Counts
                IsLike = Post.IsLike,
                TotalLike = Post.LikeCount,
                TotalComments = Post.CommentsCount,
                TotalView = Post.ViewCount,
                Token = Post.Token,
                DefaultImage = Post.DefaultImage,
                DefaultIcon = Post.DefaultIcon
            }).ToList();
            return Ok(new ApiResponse(true, "Sucess", response));
        }
        #endregion

        #region Get Artiest List
        [HttpPost("GetArtiestList")]
        [EndpointSummary("GetArtiestList")]
        [EndpointDescription("Get Artiest List")]
        [EndpointName("GetArtiestList")]
        public IActionResult GetArtiestList(CommonModel model)
        {

            var ArtiestList = _postmanagement.GetArtiestList(model.Id);
            if (ArtiestList.Count == 0)
                return Ok(new ApiResponse(false, "No Record Found", null));

            if (ArtiestList == null)
                return Ok(new ApiResponse(false, "Error While Fatch Data", null));

            var response = ArtiestList.Select(User => new
            {
                UserId = User.Id,
                UserTypeId = User.UserTypeId,
                RoleId = User.RoleId,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                MobileNumber = User.Mobile,
                Bio = User.Bio,
                Role = User.RoleName,
                IsActive = User.IsActive,
                ImageUrl = User.StrImage,
                //UserPrefrence = User.UserPrefrence,
                IsFollowing = User.IsFollowing,
                Followers = User.Followers,
                Followings = User.Followings,
                TokenErned = "0",
            }).ToList();
            return Ok(new ApiResponse(true, "Sucess", response));
        }
        #endregion
    }
}
