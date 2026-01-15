using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Use this for ASP.NET Core
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MusiBuy.Common.Models
{
    public class CreatoresViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public SelectList? SelectRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(250, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        [Remote("ValidateDuplicateEmail", "Creatores", AdditionalFields = "Id", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ValueNotAvailable")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(maximumLength: 25, MinimumLength = 8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = null!;

        public string?  StrProfilePicture { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? Bio { get; set; }
        public string Password { get; set; }
        public int? Otp { get; set; }
        public DateTime? OtpCreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }

        public bool IsRecordUsed { get; set; }

        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
