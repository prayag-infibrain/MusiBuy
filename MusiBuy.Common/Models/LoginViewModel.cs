using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
