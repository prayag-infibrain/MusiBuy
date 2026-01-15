using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using CompareAttribute = System.Web.Mvc.CompareAttribute;

namespace MusiBuy.Common.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }



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
        public string? Email { get; set; }

        
        [StringLength(20, MinimumLength = 8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Display(Name = "Mobile")]
        public string? Mobile { get; set; }


        public bool IsActive { get; set; }
        public bool IsCurrentAdminUser { get; set; }
        public string? Active { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Role")]
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }

        public SelectList? lstRoles {  get; set; }

    }
}
