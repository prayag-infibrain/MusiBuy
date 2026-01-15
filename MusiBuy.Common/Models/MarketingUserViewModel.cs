using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;  // Use this for ASP.NET Core


using Microsoft.AspNetCore.Mvc;

namespace MusiBuy.Common.Models
{
    public class MarketingUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Remote("ValidateDuplicateUser", "User", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ValueNotAvailable")]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidPassword")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Confirm password")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }


        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "First name")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        public string? FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Last name")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        public string? LastName { get; set; }


        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(250, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        [Remote("ValidateDuplicateEmail", "User", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ValueNotAvailable")]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(15, MinimumLength = 8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "User Type")]
        public int UserTypeId { get; set; }
        public string UserTypeValue { get; set; }

        [Display(Name = "Code")]
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public SelectList? SelectCountry { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }
    }
}
