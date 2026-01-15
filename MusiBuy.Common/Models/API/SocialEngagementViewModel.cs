using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class SocialEngagementViewModel
    {
        public class ContentLikeViewModel
        {
            public int ContentId { get; set; }
            public int ContentTypeId { get; set; }
            public int UserId { get; set; }
            public bool IsLike { get; set; }
        }

        public class FollowUnfollowViewModel
        {
            public int UserId { get; set; }
            public int UserTypeId { get; set; }
            public int FollowingId { get; set; }
            public bool IsFollow { get; set; }
        }

        public class FollowListViewModel
        {
            public int UserId { get; set; }
            public int TypeId { get; set; }
        }

        public class ContentAddViewViewModel
        {
            public int UserId { get; set; }
            public int ContentTypeId { get; set; }
            public int ContentId { get; set; }
        }
    }
}
