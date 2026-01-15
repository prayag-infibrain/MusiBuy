using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Use this for ASP.NET Core
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;


namespace MusiBuy.Common.Models
{
    public class CommentManagementViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Creator")]
        public int CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public SelectList? SelectCreator { get; set; }
        [Display(Name = "Post")]
        public int? PostId { get; set; }
        public string? PostName { get; set; }
        public SelectList? SelectPost { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public SelectList? SelectUser { get; set; }


        public int UserType { get; set; }


        [Display(Name = "Content")]
        public string Comment { get; set; } = null!;

        public DateTime? Timestamp { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public SelectList? SelectStatus { get; set; }

        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
