using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class ChangeProfileViewModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Last name")]
        public string? Lastname { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(250, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(15, MinimumLength = 8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }
        [Display(Name = "Code")]
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public List<SelectListItem> SelectCountry { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
    }
}