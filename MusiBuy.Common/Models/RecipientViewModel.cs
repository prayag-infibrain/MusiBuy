using MusiBuy.Common.Common; 
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class RecipientViewModel
    {
        public int? Id { get; set; }
        public int? TemplateId { get; set; }        

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name="First name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name="Last name")]
        public string? LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(250, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]        
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }
        public string? TemplateName { get; set; }       
        public string? DuplicateEmail { get; set; }       
       
    }
}