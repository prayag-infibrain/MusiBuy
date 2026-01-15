using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Role name")]
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }       
        public string? Active { get; set; }
        public bool IsAdminRole { get; set; }
        public bool IsCurrentUserRole { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDeletable {  get; set; }
    }
}