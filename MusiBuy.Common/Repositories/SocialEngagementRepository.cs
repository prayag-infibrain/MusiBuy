using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusiBuy.Common.Common;
using MusiBuy.Common.DB;
using MusiBuy.Common.Enumeration.API;
using MusiBuy.Common.Helper;
using MusiBuy.Common.Helpers;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models.API;
using MusiBuy.Common.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;
using static MusiBuy.Common.Models.API.SocialEngagementViewModel;

namespace MusiBuy.Common.Repositories
{
    public class SocialEngagementRepository : ISocialEngagement
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        private readonly IConfiguration _config;
        public SocialEngagementRepository(MusiBuyDB_Connection context, IConfiguration configuration)
        {
            _Context = context;
            _config = configuration;
        }
        #endregion


        #region Add/Remove Like And Get Like Count On Post by ID
        public (bool Status, string Message, int Count) ToggleLike(ContentLikeViewModel model)
        {
            try
            {
                var existing = _Context.LikeManagements.FirstOrDefault(a => a.UserId == model.UserId &&
                         (
                             (model.ContentTypeId == (int)ApiContentType.Event && a.EventId == model.ContentId) ||
                             (model.ContentTypeId == (int)ApiContentType.Post && a.PostId == model.ContentId)
                         )
                     );

                if (model.IsLike)
                {
                    if (existing == null)
                    {
                        //ADD LIKE
                        var newLike = new LikeManagement
                        {
                            UserId = model.UserId,
                            EventId = model.ContentTypeId == (int)ApiContentType.Event ? model.ContentId : null,
                            PostId = model.ContentTypeId == (int)ApiContentType.Post ? model.ContentId : null
                        };
                        _Context.LikeManagements.Add(newLike);
                    }
                }
                else
                {
                    //REMOVE LIKE
                    if (existing != null)
                        _Context.LikeManagements.Remove(existing);
                }
                _Context.SaveChanges();

                // 4. Count updated likes
                int totalCount = _Context.LikeManagements.Count(a =>
                    (model.ContentTypeId == (int)ApiContentType.Event && a.EventId == model.ContentId) ||
                    (model.ContentTypeId == (int)ApiContentType.Post && a.PostId == model.ContentId)
                );

                return (true, "Success", totalCount);
            }
            catch (Exception ex)
            {
                return (false, "Error While Operation, Ex : " + ex.Message, 0);
            }

        }
        #endregion

        #region Add/Remove Followers And Get Followers Count On Artiest by ID
        public (bool Status, string Message, int Count) ToggleFollow(FollowUnfollowViewModel model)
        {
            try
            {
                bool Status = false;
                string Message = null;
                int FollowCount = 0;
                FollowersManagement followersManagement = new FollowersManagement();
                var Existing = _Context.FollowersManagements.Where(a => a.UserId == model.UserId && a.FollowIngId == model.FollowingId).FirstOrDefault();

                if (model.IsFollow == false && Existing == null)
                    return (false, "Invalid Operation", 0);

                //For Unfollow Remove REcord From DB
                if (model.IsFollow == false && Existing != null)
                {
                    _Context.FollowersManagements.Remove(Existing);
                    _Context.SaveChanges();
                    Status = true;
                    Message = "UnFollow";
                }

                //For Follow Add Record in DB
                if (model.IsFollow == true && Existing == null)
                {
                    followersManagement.UserId = model.UserId;
                    followersManagement.FollowIngId = model.FollowingId;
                    followersManagement.CreatedOn = DateTime.Now;
                    followersManagement.CreatedBy = model.UserId;
                    _Context.FollowersManagements.Add(followersManagement);
                    _Context.SaveChanges();
                    Status = true;
                    Message = "Follow";
                }

                FollowCount = _Context.FollowersManagements.Count(a => a.FollowIngId == model.FollowingId);
                return (Status, Message, FollowCount);
            }
            catch (Exception ex)
            {
                return (false, "Error While Operation, Ex : " + ex.Message, 0);
            }

        }
        #endregion


        #region Get Followers/Following List By User Id
        public List<FrontUserViewModel> GetFollowrsList(FollowListViewModel model)
        {
            try
            {
                string fileReadPath = _config.GetSection("FilePath:FileReadPath").Value;
                string relativePath = GlobalCode.FrontUserImages.Replace("\\", "/").Trim('/');
                string FilePath = Path.Combine(fileReadPath, relativePath);

                if (model.TypeId == 1)
                    return CommonHelper.GetFollowers(_Context, model.UserId, FilePath);
                else
                    return CommonHelper.GetFollowing(_Context, model.UserId,FilePath);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion



        #region Add View And Return Count
        public (bool Status, string Message, int Count) AddView(ContentAddViewViewModel ViewModel)
        {
            try
            {
                bool Status = false;
                string Message = null;
                int ViewCount = 0;
                ViewManagement followersManagement = new ViewManagement();
                if (ViewModel.UserId == 0)
                    return (false, "Invalid User", 0);
                if (ViewModel.ContentTypeId == 0)
                    return (false, "Invalid Content Type", 0);
                if (ViewModel.ContentId == 0)
                    return (false, "Invalid Invalid Content", 0);


                var Existing = _Context.ViewManagements.ToList();
                if (ViewModel.ContentTypeId == (int)ApiContentType.Event)
                {
                    var EventData = Existing.Where(a => a.EventId == ViewModel.ContentId && a.UserId == ViewModel.UserId).FirstOrDefault();
                    if (EventData == null)
                    {
                        followersManagement.UserId = ViewModel.UserId;
                        followersManagement.EventId = ViewModel.ContentId;
                        followersManagement.CreatedOn = DateTime.Now;
                        followersManagement.CreatedBy = ViewModel.UserId;
                        _Context.ViewManagements.Add(followersManagement);
                        _Context.SaveChanges();
                    }
                    ViewCount = _Context.ViewManagements.Where(a => a.EventId == ViewModel.ContentId).Count();
                }
                else if (ViewModel.ContentTypeId == (int)ApiContentType.Post)
                {
                    var EventData = Existing.Where(a => a.PostId == ViewModel.ContentId && a.UserId == ViewModel.UserId).FirstOrDefault();
                    if (EventData == null)
                    {
                        followersManagement.UserId = ViewModel.UserId;
                        followersManagement.PostId = ViewModel.ContentId;
                        followersManagement.CreatedOn = DateTime.Now;
                        followersManagement.CreatedBy = ViewModel.UserId;
                        _Context.ViewManagements.Add(followersManagement);
                        _Context.SaveChanges();
                    }
                    ViewCount = _Context.ViewManagements.Count(a => a.PostId == ViewModel.ContentId);
                }
                return (true, "Sucess", ViewCount);
            }
            catch (Exception ex)
            {
                return (false, "Error While Operation, Ex : " + ex.Message, 0);
            }

        }
        #endregion
    }
}
