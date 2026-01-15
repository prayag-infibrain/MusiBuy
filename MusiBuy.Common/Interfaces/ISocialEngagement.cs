using MusiBuy.Common.DB;
using MusiBuy.Common.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusiBuy.Common.Models.API.PostAndEventViewModel;
using static MusiBuy.Common.Models.API.SocialEngagementViewModel;

namespace MusiBuy.Common.Interfaces
{
    public interface ISocialEngagement
    {
        (bool Status, string Message, int Count) ToggleLike(ContentLikeViewModel model);
        (bool Status, string Message, int Count) ToggleFollow(FollowUnfollowViewModel model);
        List<FrontUserViewModel> GetFollowrsList(FollowListViewModel model);
        (bool Status, string Message, int Count) AddView(ContentAddViewViewModel ViewModel);
    }
}
