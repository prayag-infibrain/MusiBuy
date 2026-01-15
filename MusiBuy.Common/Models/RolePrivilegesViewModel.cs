using MusiBuy.Common.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace MusiBuy.Common.Models
{
    public class RolePrivilegesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
        public int? SortOrder { get; set; }
        public SelectList? Roles { get; set; }
        public int MenuItemId { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? Detail { get; set; }
        public bool IsActive { get; set; }
        public MenuItemViewModel? MenuItem { get; set; }
    }
}
