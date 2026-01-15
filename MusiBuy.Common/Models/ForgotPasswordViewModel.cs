using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MusiBuy.Common.Common;


namespace MusiBuy.Common.Models
{
    public class ForgotPasswordViewModel
    {
        public int? UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(254, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

      
        public string Mobile { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public SelectList? CountrytRole { get; set; }
    }
}