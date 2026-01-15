using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class PostAndEventViewModel
    {
        public class CreateEventViewModel
        {
            public int Id { get; set; }
            public int UserTypeId { get; set; }
            public int RoleId { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public int TypeId { get; set; }
            //public int StatusId { get; set; }

        }
        public class CreatePostViewModel
        {
            public int Id { get; set; }
            public int UserTypeId { get; set; }
            public int RoleId { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Tags { get; set; }
            public int ContentTypeId { get; set; }
            public int CountryId { get; set; } // For Content Type :  Music
            public int GenreId { get; set; } // For Content Type :  Music, Music Producer
            public int CategoryId { get; set; } // For Content Type :  Prodcast, Social Media Infulencer
            public int TypeId { get; set; } // For Content Type :  Music Producer
            public int MediaTypeId { get; set; }
            public IFormFile? MediaFile { get; set; }
            public string? CollaborationList { get; set; }

        }

        public class HomeScreenContent
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Status { get; set; }
            public int CreateType { get; set; } // 1 = Event, 2 = Post
            public string CreatorName { get; set; }
            public int CreatorTypeId { get; set; }
            public string CreatorProfileUrl { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? StartDateTime { get; set; }
            public DateTime? EndDateTime { get; set; }
            public int? EventTypeId { get; set; }
            public int? EventStatusId { get; set; }
            public string? EventStatus { get; set; }
            public int? PostTypeId { get; set; }
            public string? PostTypeName { get; set; }
            public string StrMediaFile { get; set; }
            public string StrMediaFileName { get; set; }
            public string StrMediaFileSize { get; set; }
            public string Tags { get; set; }
            public int PostContentTypeId { get; set; }
            public string? PostContentTypeName { get; set; }
            public int? CountryId { get; set; }
            public string? CountryName { get; set; }
            public int? GenreId { get; set; }
            public string? GenreName { get; set; }
            public int? CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public int? TypeId { get; set; }
            public string? TypeName { get; set; }
            public int PostMediaTypeId { get; set; }
            public bool IsLike { get; set; }
            public int? LikeCount { get; set; }
            public int? CommentsCount { get; set; }
            public int? ViewCount { get; set; }
            public string Token { get; set; }
            public DateTime CreatedOn { get; set; }
            public object PersonalizedList { get; set; }
            public string DefaultImage { get; set; }
            public string DefaultIcon { get; set; }
        }
    }
}
