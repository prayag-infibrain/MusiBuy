using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class CommonSettingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "SMTP server")]
        public string? SMTPServer { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(255, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InvalidEmail")]
        public string? Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(4, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinMaxAllowedLength")]
        [Display(Name = "Port")]
        public string? Port { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression(@"http(s)?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{2,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InValid")]
        [Display(Name = "Site url")]
        public string? SiteURL { get; set; }


        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(500, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [RegularExpression(@"http(s)?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{2,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "InValid")]
        [Display(Name = "Site url (For Marketing)")]
        public string? MarktingSiteURL { get; set; }



        public bool IsSSL { get; set; }
        public CommonMessagesViewModel? ModuleName { get; set; }

        

        

    }
}
