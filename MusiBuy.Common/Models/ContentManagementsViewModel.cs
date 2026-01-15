using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;



namespace MusiBuy.Common.Models
{
    public class ContentManagementsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Page type")]
        public int PageId { get; set; }
        public EnumViewModel? PageDetails { get; set; }
        public string? Page { get; set; }
        public SelectList? PageList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Page title")]
        public string? Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Page content")]
        public string? Content { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }
        public CommonMessagesViewModel? ModuleName { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public IEnumerable<ContentManagementsViewModel>? IListCMS { get; set; }
        public bool IsAdminUser { get; set; }
        public bool IsCurrentAdminUser { get; set; }
    }
}
