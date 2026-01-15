using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering; // Use this for ASP.NET Core
using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;


namespace MusiBuy.Common.Models
{
    public class EventManagementViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(500, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }


        [Display(Name = "Creator")]
        public int CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public SelectList? SelectCreator { get; set; }


        [Display(Name = "Event Type")]
        public int EventTypeId { get; set; }
        public string? EventTypeName { get; set; }
        public SelectList? SelectEventType { get; set; }

        public string? Daterange { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Event Start Date & Time")]
        public DateTime EventStartDateTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Event End Date & Time")]
        public DateTime EventEndDateTime { get; set; }
        public string Duration { get; set; }

        [Display(Name = "Status")]
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public SelectList? SelectStatus { get; set; }

        public string RecordingURL { get; set; }


        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ValidateNever]
        public int UserTypeId { get; set; }
        [ValidateNever]
        public int RoleId { get; set; }
        [ValidateNever]
        public int UserId { get; set; }
    }
}
