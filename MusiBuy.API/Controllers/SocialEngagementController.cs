using Microsoft.AspNetCore.Mvc;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using System.ComponentModel;
using System.Net.NetworkInformation;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;
using static MusiBuy.Common.Models.API.SocialEngagementViewModel;

namespace MusiBuy.API.Controllers
{
    //[Authorize]
    [Route("api/SocialEngagement")]
    [ApiController]
    public class SocialEngagementController : ControllerBase
    {
        #region Member Declaration
        private readonly ISocialEngagement _social;
        public SocialEngagementController(ISocialEngagement social)
        {
            _social = social;
        }
        #endregion

        #region Add/Remove Like On Content
        [HttpPost("ToggleLike")]
        [EndpointSummary("ToggleLike")]
        [EndpointDescription("Add Or remove Like On Content")]
        [EndpointName("ToggleLike")]
        public IActionResult ToggleLike(ContentLikeViewModel LikeModel)
        {
            if (LikeModel.UserId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User", null));
            }
            if (LikeModel.ContentTypeId == 0 )
            {
                return Ok(new ApiResponse(false, "Invalid Content Type", null));
            }
            if (LikeModel.ContentId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid Content", null));
            }
            if (LikeModel.IsLike == null)
            {
                return Ok(new ApiResponse(false, "Invalid Data on IsLike", null));
            }
            var (Status, Msg, Count) = _social.ToggleLike(LikeModel);
            var response = new
            {
                Count = Count,
                IsLike = LikeModel.IsLike
            };
            return Ok(new ApiResponse(Status, Msg , response));
        }
        #endregion

        #region Add/Remove Follow On Content
        [HttpPost("ToggleFollow")]
        [EndpointSummary("ToggleFollow")]
        [EndpointDescription("Follow Or UnFollow On Artiest")]
        [EndpointName("ToggleFollow")]
        public IActionResult ToggleFollow(FollowUnfollowViewModel FollowModel)
        {
            if (FollowModel.UserId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User", null));
            }
            if (FollowModel.UserTypeId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid User Type", null));
            }
            if (FollowModel.FollowingId == 0)
            {
                return Ok(new ApiResponse(false, "Invalid Following", null));
            }
            if (FollowModel.IsFollow == null)
            {
                return Ok(new ApiResponse(false, "Invalid Data on IsFollow", null));
            }
            var (Status, Msg, Count) = _social.ToggleFollow(FollowModel);
            var response = new
            {
                Count = Count,
                IsFollow = FollowModel.IsFollow
            };
            return Ok(new ApiResponse(Status, Msg, response));
        }
        #endregion

        #region Get Followers/Following List By User Id
        [HttpPost("GetFollowrsList")]
        [EndpointSummary("GetFollowrsList")]
        [EndpointDescription("Get Followers/Following List By User Id")]
        [EndpointName("GetFollowrsList")]
        public IActionResult GetFollowrsList(FollowListViewModel FollowModel)
        {
            if (FollowModel.UserId == 0)
                return Ok(new ApiResponse(false, "Invalid User", null));
            if (FollowModel.TypeId == 0)
                return Ok(new ApiResponse(false, "Invalid Type", null));

            var List = _social.GetFollowrsList(FollowModel);
            if (List.Count == 0)
                return Ok(new ApiResponse(true, "No Record Found.", null));

            var FinalList = List.Select(r => new
            {
                id = r.Id,
                Name =  r.FirstName +" "+ r.LastName,
                role = r.RoleName,
                ImageUrl = r.StrImage,
                Token = 0.00
            });

            return Ok(new ApiResponse(true, "Sucess", FinalList));
        }
        #endregion


        #region Add/Remove Follow On Content
        [HttpPost("AddView")]
        [EndpointSummary("AddView")]
        [EndpointDescription("Add View")]
        [EndpointName("AddView")]
        public IActionResult AddView(ContentAddViewViewModel ViewModel)
        {
            var (Status, Msg, Count) = _social.AddView(ViewModel);
            var response = new
            {
                Count = Count,
            };
            return Ok(new ApiResponse(Status, Msg, response));
        }
        #endregion
    }
}
