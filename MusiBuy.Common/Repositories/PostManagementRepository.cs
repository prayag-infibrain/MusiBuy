
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration;
using MusiBuy.Common.Enumeration.API;
using MusiBuy.Common.Helper;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using MusiBuy.Common.Models.API;
using System.Linq;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;
namespace MusiBuy.Common.Repositories
{
    public class PostManagementRepository : IPostManagement
    {
        #region Member Declaration
        /// <summary>
        /// Dependency injections
        /// </summary>
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;

        public PostManagementRepository(MusiBuyDB_Connection context, IConfiguration configuration)
        {
            this._Context = context;
            _config = configuration;
        }
        #endregion

        #region Get user data for grid
        /// <summary>
        /// Get user data for grid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchValue"></param>
        /// <returns>It returns user list</returns>
        public IQueryable<PostManagementViewModel> GetPostManagementList(int userId, string searchValue)
        {
            var query = _Context.PostManagements.Include(a => a.User).Where(p => string.IsNullOrWhiteSpace(searchValue) || p.Title.Contains(searchValue) || (p.User.FirstName + " " + p.User.LastName).Contains(searchValue))
                .Select(p => new
                {
                    p.Id,
                    p.CreatorId,
                    CreatorName = p.User.FirstName + " " + p.User.LastName,
                    CreatorRole = p.User.Role.EnumValue,
                    p.MediaTypeId,
                    p.Title,
                    p.Description,
                    p.Url,
                    p.Tags,
                    p.StatusId,
                    p.PublishDate
                }).AsEnumerable()
                .Select(p => new PostManagementViewModel
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId,
                    CreatorName = p.CreatorName + " (" + p.CreatorRole + ")",
                    MediaTypeId = p.MediaTypeId,
                    MediaTypeName = ((PostMediaTypeEnum)p.MediaTypeId).GetDisplayName(),
                    Title = p.Title,
                    Description = p.Description,
                    Url = p.Url,
                    Tags = p.Tags,
                    StatusId = p.StatusId,
                    PublishDate = p.PublishDate
                }).AsQueryable();


            return query;
        }
        #endregion

        #region Get User Details by UserID
        /// <summary>
        /// Get single user by UserID
        /// </summary>
        /// <returns>It returns user details by id </returns>
        public PostManagementViewModel GetPostManagementDetailsByID(int PostID)
        {
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.PostMediaFile.Replace("\\", "/").Trim('/');

            PostManagementViewModel? validateCreatore1 = _Context.PostManagements.Include(p => p.User).Where(p => p.Id == PostID)
                .Select(p => new PostManagementViewModel
                {
                    Id = p.Id,
                    CreatorId = p.CreatorId,
                    CreatorName = p.User.FirstName + " " + p.User.LastName,
                    CreatorType = p.User.Role.EnumValue,
                    MediaTypeId = p.MediaTypeId,
                    MediaTypeName = ((PostMediaTypeEnum)p.MediaTypeId).GetDisplayName(),
                    Title = p.Title,
                    Description = p.Description,
                    MediaFileName = p.MediaFile,
                    StrMediaFile = p.MediaFile != null ? $"{fileReadPath}{relativePath}/{p.CreatorId}/{p.MediaFile}" : null,
                    Url = p.Url,
                    Tags = p.Tags,
                    UserTypeId = p.UserTypeId,
                    RoleId = p.RoleId,
                    UserId = p.UserId,

                    ContentTypeId = p.ContentTypeId,
                    ContentTypeName = p.ContentType.EnumValue,
                    CountryId = p.CountryId,
                    CountryName = p.Country.Name,
                    GenreId = p.GenreId,
                    GenreName = p.Genre.GenreName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.EnumValue,
                    TypeId = p.TypeId,
                    TypeName = p.Type.EnumValue,
                    StatusId = p.StatusId,
                    StatusName = p.IsActive.Value ? GlobalCode.ActiveText : GlobalCode.InActiveText,
                    PublishDate = p.PublishDate


                }).FirstOrDefault();

            return validateCreatore1;

        }
        #endregion


        #region Delete multiple Users
        /// <summary>
        /// Delete multiple users
        /// </summary>
        /// <returns>void</returns>
        public bool DeletePost(int[] ids)
        {
            _Context.CommentsManagements.RemoveRange(_Context.CommentsManagements.Where(u => ids.Contains(u.PostId ?? 0)).AsEnumerable());
            _Context.PostManagements.RemoveRange(_Context.PostManagements.Where(u => ids.Contains(u.Id)).AsEnumerable());
            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion        

        #region Save Post
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <returns></returns>
        public bool Save(PostManagementViewModel PostManagementViewModel)
        {
            try
            {
                var PostData = new PostManagement();
                if (PostManagementViewModel.Id > 0)
                {
                    PostData = _Context.PostManagements.FirstOrDefault(x => x.Id == PostManagementViewModel.Id) ?? new PostManagement();
                }
                PostData.CreatorId = PostManagementViewModel.CreatorId;
                PostData.MediaTypeId = PostManagementViewModel.MediaTypeId;
                PostData.Title = PostManagementViewModel.Title;
                PostData.Description = PostManagementViewModel.Description;
                PostData.MediaFile = PostManagementViewModel.MediaFileName;
                PostData.Url = PostManagementViewModel.Url;
                PostData.Tags = PostManagementViewModel.Tags;

                PostData.UserTypeId = PostManagementViewModel.UserTypeId;
                PostData.RoleId = PostManagementViewModel.RoleId;
                PostData.UserId = PostManagementViewModel.UserId;

                PostData.ContentTypeId = PostManagementViewModel.ContentTypeId;
                PostData.CountryId = PostManagementViewModel.CountryId == 0 ? null : PostManagementViewModel.CountryId;
                PostData.GenreId = PostManagementViewModel.GenreId == 0 ? null : PostManagementViewModel.GenreId;
                PostData.CategoryId = PostManagementViewModel.CategoryId == 0 ? null : PostManagementViewModel.CategoryId;
                PostData.TypeId = PostManagementViewModel.TypeId == 0 ? null : PostManagementViewModel.TypeId;
                PostData.IsActive = PostManagementViewModel.IsActive;


                if (PostData.Id == 0)
                {
                    PostData.StatusId = (int)PostStatusEnum.In_Review;
                    PostData.PublishDate = DateTime.Now;
                    PostData.CreatedOn = DateTime.Now;
                    PostData.CreatedBy = PostManagementViewModel.CreatedBy;
                    _Context.PostManagements.Add(PostData);
                }
                else
                {
                    PostData.PublishDate = PostManagementViewModel.PublishDate;
                    PostData.UpdatedOn = DateTime.Now;
                    PostData.UpdatedBy = PostManagementViewModel.UpdatedBy;
                    _Context.PostManagements.Update(PostData);
                }

                if (_Context.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get_Customer_Profile_Picture
        public string? GetPostManagementMediaFile(int id)
        {
            return _Context.PostManagements.FirstOrDefault(d => d.Id == id)?.MediaFile;

        }
        #endregion

        #region Remove_Customer_Profile_Picture
        public void RemovePostManagementMediaFile(int id)
        {
            PostManagement? postManagement = _Context.PostManagements.FirstOrDefault(d => d.Id == id);
            if (postManagement != null)
            {
                postManagement.MediaFile = null;
                _Context.SaveChanges();
            }
        }
        #endregion


        #region Get Post Count By Type For Dashboard
        public Dictionary<string, int> PostCountByType()
        {
            List<PostManagement>? postManagement = _Context.PostManagements.ToList();
            Dictionary<string, int> Count = new Dictionary<string, int>();
            Count.Add("Audio", postManagement.Count(p => p.TypeId == (int)PostMediaTypeEnum.Audio));
            Count.Add("Video", postManagement.Count(p => p.TypeId == (int)PostMediaTypeEnum.Video));
            Count.Add("Image", postManagement.Count(p => p.TypeId == (int)PostMediaTypeEnum.Image));
            Count.Add("Text", postManagement.Count(p => p.TypeId == (int)PostMediaTypeEnum.Text));
            return Count;
        }
        #endregion

        #region Get Post Dropdown List
        /// <summary>
        /// Get Post List
        /// </summary>
        /// <returns>Returns list of All Post for dropdown</returns>
        public List<DropDownBindViewModel> GetPostManagementDropDownList()
        {
            List<DropDownBindViewModel> objPostList = _Context.PostManagements.Select(e => new DropDownBindViewModel { value = e.Id, name = e.Title }).ToList();
            return objPostList;
        }
        #endregion

        #region Get Post Dropdown List by Creator
        /// <summary>
        /// Get Post List
        /// </summary>
        /// <returns>Returns list of All Post for dropdown</returns>
        public List<DropDownBindViewModel> GetPostManagementDropDownListByCreatorId(int creatorId)
        {
            List<DropDownBindViewModel> objPostList = _Context.PostManagements.Where(a => a.CreatorId == creatorId).Select(e => new DropDownBindViewModel { value = e.Id, name = e.Title }).ToList();
            return objPostList;
        }
        #endregion



        #region API Method's
        #region Get Post Details by UserID
        public List<PostManagementViewModel> GetPostManagementDetailsByUserID(int UserId, int TypeId)
        {
            List<PostManagementViewModel> validatePost = new List<PostManagementViewModel>();
            validatePost = _Context.PostManagements
                .Include(a => a.User)
                .Include(a => a.ContentType)
                .Include(a => a.Role)
                .Where(a => a.UserId == UserId && a.IsActive == true)
                .Select(u => new PostManagementViewModel
                {
                    Id = u.Id,
                    UserId = u.UserId,
                    UserTypeId = u.UserTypeId,
                    CreatorName = u.User.FirstName + " " + u.User.LastName,
                    Title = u.Title,
                    Description = u.Description,
                    Tags = u.Tags,
                    ContentTypeId = u.ContentTypeId,
                    CountryId = u.CountryId,
                    GenreId = u.GenreId,
                    CategoryId = u.CategoryId,
                    TypeId = u.TypeId,
                    MediaTypeId = u.MediaTypeId,

                    MediaFileName = u.MediaFile,
                    StrMediaFile = CommonHelper.GetPostImageReadPath(_config, u.UserTypeId, u.UserId, u.MediaFile),
                    MediaFileSize = CommonHelper.GetMediaFileSize(_config, u.UserTypeId, u.UserId, u.MediaFile),
                    Url = u.Url,
                    StatusId = u.StatusId,
                    PublishDate = u.PublishDate,
                    DefaultImage = CommonHelper.GetDefaultImage(_config, u.MediaTypeId ?? 0),
                    DefaultIcon = CommonHelper.GetDefaultIcon(_config, u.MediaTypeId ?? 0)
                }).ToList();

            if (TypeId != 0)
            {
                validatePost = validatePost.Where(a => a.MediaTypeId == TypeId).ToList();
            }

            return validatePost;

        }
        #endregion

        #region Delete Post By Id        
        public bool DeleteContentById(DeleteContent model)
        {
            if (model.ContentTypeId == (int)ApiContentType.Post)
            {
                var Post = _Context.PostManagements.Where(a => a.Id == model.Id).FirstOrDefault();
                Post.IsActive = false;
            }
            else
            {
                var Post = _Context.EventManagements.Where(a => a.Id == model.Id).FirstOrDefault();
                Post.IsActive = false;
            }

            if (_Context.SaveChanges() > 0)
                return true;
            else return false;
        }
        #endregion   

        #region Get Post And Event List By UserID
        public async Task<List<HomeScreenContent>> GetHomeScreenData(int UserId)
        {
            try
            {
                string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
                string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');

                var events = await _Context.EventManagements.Include(a => a.User).Where(a => a.IsActive == true).OrderByDescending(x => x.Id).ToListAsync();
                var posts = await _Context.PostManagements
                    .Include(a => a.User)
                    .Include(a => a.Country)
                    .Include(a => a.Genre)
                    .Include(a => a.Category)
                    .Include(a => a.Type)
                    .Include(a => a.ContentType)
                    .Where(a => a.IsActive == true).OrderByDescending(x => x.Id).ToListAsync();
                var LikeData = await _Context.LikeManagements.ToListAsync();
                var CommentData = await _Context.CommentsManagements.ToListAsync();
                var ViewData = await _Context.ViewManagements.ToListAsync();

                //if (UserId > 0)
                //{
                //    events = events.Where(a => a.UserId == UserId).ToList();
                //    posts = posts.Where(a => a.UserId == UserId).ToList();
                //}
                var eventList = events.Select(e => new HomeScreenContent
                {
                    Id = e.Id,
                    CreateType = 1, // Event
                    Status = CommonHelper.GetEventStatusName(e.StatusId ?? 0),
                    CreatorName = e.User.FirstName + " " + e.User.LastName,
                    CreatorTypeId = e.User.RoleId ?? 0,
                    CreatorProfileUrl = e.User.Image != null ? $"{fileReadPath}{relativePath}/{e.User.Image}" : null,
                    Title = e.Title,
                    Description = e.Description,
                    StartDateTime = e.EventStartDateTime,
                    EndDateTime = e.EventEndDateTime,
                    EventTypeId = e.EventTypeId,
                    EventStatusId = e.StatusId,
                    CountryId = null,
                    GenreId = null,
                    CategoryId = null,
                    UserId = e.User.Id,

                    IsLike = UserId > 0 ? LikeData.Where(a => a.EventId == e.Id && a.UserId == UserId).Any() : false,
                    LikeCount = LikeData.Where(a => a.EventId == e.Id).Count(),
                    CommentsCount = CommentData.Where(a => a.EventId == e.Id).Count(),
                    ViewCount = ViewData.Where(a => a.EventId == e.Id).Count(),
                    CreatedOn = e.CreatedOn,
                    Token = ""
                }).ToList();

                var postList = posts.Select(p => new HomeScreenContent
                {
                    Id = p.Id,
                    CreateType = 2, // Post
                    Status = "Publish",
                    Title = p.Title,
                    CreatorName = p.User.FirstName + " " + p.User.LastName,
                    CreatorTypeId = p.User.RoleId ?? 0,
                    CreatorProfileUrl = p.User.Image != null ? $"{fileReadPath}{relativePath}/{p.User.Image}" : null,
                    Description = p.Description,
                    StrMediaFile = CommonHelper.GetPostImageReadPath(_config, p.UserTypeId, p.UserId, p.MediaFile),
                    PostMediaTypeId = p.MediaTypeId ?? 0,
                    PostTypeId = p.TypeId,
                    PostTypeName = p.Type?.EnumValue,
                    PostContentTypeId = p.ContentTypeId,
                    PostContentTypeName = p.ContentType?.EnumValue,
                    CountryId = p.CountryId,
                    CountryName = p.Country?.Name,
                    GenreId = p.GenreId,
                    GenreName = p.Genre?.GenreName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.EnumValue,
                    Tags = p.Tags,
                    UserId = p.User.Id,
                    IsLike = UserId > 0 ? LikeData.Where(a => a.PostId == p.Id && a.UserId == UserId).Any() : false,
                    LikeCount = LikeData.Where(a => a.PostId == p.Id).Count(),
                    CommentsCount = CommentData.Where(a => a.PostId == p.Id).Count(),
                    ViewCount = ViewData.Where(a => a.PostId == p.Id).Count(),
                    StrMediaFileSize = CommonHelper.GetMediaFileSize(_config, p.UserTypeId, p.UserId, p.MediaFile),
                    CreatedOn = p.CreatedOn,
                    Token = "",
                    DefaultImage = CommonHelper.GetDefaultImage(_config, p.MediaTypeId ?? 0),
                    DefaultIcon = CommonHelper.GetDefaultIcon(_config, p.MediaTypeId ?? 0)
                }).ToList();

                var finalList = eventList.Concat(postList).OrderByDescending(x => x.CreatedOn).ToList();
                return finalList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Get Artiest List
        public List<FrontUserViewModel> GetArtiestList(int UserId)
        {
            //var FollowingList = _Context.FollowersManagements.ToList();
            //string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            //string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');
            //List<FrontUserViewModel> validatePost = new List<FrontUserViewModel>();
            //validatePost = _Context.FrontUsers.Include(a => a.Role).Where(a => a.IsActive == true && a.UserTypeId != (int)APIUserTypeEnum.User && a.Id != UserId)
            //    .Select(User => new FrontUserViewModel
            //    {
            //        Id = User.Id,
            //        FirstName = User.FirstName,
            //        LastName = User.LastName,
            //        Mobile = User.Mobile,
            //        Email = User.Email,
            //        Bio = User.Bio,
            //        StrImage = User.Image != null ? $"{fileReadPath}{relativePath}/{User.Image}" : null,
            //        ImageName = User.Image,
            //        UserTypeId = User.UserTypeId,
            //        RoleId = User.RoleId,
            //        RoleName = User.UserTypeId == 2 ? (User.Role != null ? User.Role.EnumValue : string.Empty) : "User",
            //        IsFollowing = FollowingList.Where(a => a.FollowIngId == User.Id && a.UserId == UserId).Any(),
            //        Followers = FollowingList.Where(a => a.FollowIngId == User.Id).Count(),
            //        Followings = FollowingList.Where(a => a.UserId == User.Id).Count()
            //    }).ToList();

            //return validatePost;
            var FollowingList = _Context.FollowersManagements.ToList();
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');
            var users = _Context.FrontUsers.Include(a => a.Role).Where(a => a.IsActive == true && a.UserTypeId != (int)APIUserTypeEnum.User && a.Id != UserId)
                .Select(User => new FrontUserViewModel
                {
                    Id = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Mobile = User.Mobile,
                    Email = User.Email,
                    Bio = User.Bio,
                    StrImage = User.Image != null ? $"{fileReadPath}{relativePath}/{User.Image}" : null,
                    ImageName = User.Image,
                    UserTypeId = User.UserTypeId,
                    RoleId = User.RoleId,
                    RoleName = User.UserTypeId == 2 ? (User.Role != null ? User.Role.EnumValue : string.Empty) : "User",
                })
                .ToList();
            foreach (var u in users)
            {
                u.IsFollowing = FollowingList.Any(a => a.FollowIngId == u.Id && a.UserId == UserId);
                u.Followers = FollowingList.Count(a => a.FollowIngId == u.Id);
                u.Followings = FollowingList.Count(a => a.UserId == u.Id);
            }

            return users;


        }
        #endregion

        #region Get Post Details by PostId
        public HomeScreenContent GetPostManagementDetailsByIDAPI(int PostID)
        {
            string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
            string relativePath = GlobalCode.PostMediaFile.Replace("\\", "/").Trim('/');
            if (PostID == 0)
                return null;
            var p = _Context.PostManagements
                    .Include(a => a.User)
                    .Include(a => a.Country)
                    .Include(a => a.Genre)
                    .Include(a => a.Category)
                    .Include(a => a.Type)
                    .Include(a => a.ContentType)
                    .Where(a => a.IsActive == true && a.Id == PostID).OrderByDescending(x => x.Id).FirstOrDefault();
            var LikeData = _Context.LikeManagements.ToList();
            var CommentData = _Context.CommentsManagements.ToList();
            var ViewData = _Context.ViewManagements.ToList();

            HomeScreenContent? Post = new HomeScreenContent
            {
                Id = p.Id,
                CreateType = 2,
                Status = "Publish",
                Title = p.Title,
                CreatorName = p.User.FirstName + " " + p.User.LastName,
                Description = p.Description,
                StrMediaFile = CommonHelper.GetPostImageReadPath(_config, p.UserTypeId, p.UserId, p.MediaFile),
                StrMediaFileName = p.MediaFile,
                PostMediaTypeId = p.MediaTypeId ?? 0,

                PostTypeId = p.TypeId,
                PostTypeName = p.Type?.EnumValue,
                PostContentTypeId = p.ContentTypeId,
                PostContentTypeName = p.ContentType?.EnumValue,
                CountryId = p.CountryId,
                CountryName = p.Country?.Name,
                GenreId = p.GenreId,
                GenreName = p.Genre?.GenreName,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.EnumValue,

                Tags = p.Tags,
                LikeCount = LikeData.Where(a => a.PostId == p.Id).Count(),
                CommentsCount = CommentData.Where(a => a.PostId == p.Id).Count(),
                ViewCount = ViewData.Where(a => a.PostId == p.Id).Count(),
                StrMediaFileSize = CommonHelper.GetMediaFileSize(_config, p.UserTypeId, p.UserId, p.MediaFile),
                Token = "",
                DefaultImage = CommonHelper.GetDefaultImage(_config, p.MediaTypeId ?? 0),
                DefaultIcon = CommonHelper.GetDefaultIcon(_config, p.MediaTypeId ?? 0)
            };
            return Post;

        }
        #endregion


        public List<int> GetuserPrefIds(int UserId)
        {
            var userPrefIds = _Context.UserPrefrences.Where(a => a.FrontUserId == UserId).Select(a => a.PrefrenceId).ToList();
            return userPrefIds;
        }
        #endregion

    }
}
