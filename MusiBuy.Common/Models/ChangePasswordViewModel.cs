using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models
{
    public class ChangePasswordViewModel 
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; } = string.Empty;
        public CommonMessagesViewModel? ModuleName { get; set; }
    }
}