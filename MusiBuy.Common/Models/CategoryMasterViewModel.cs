using MusiBuy.Common.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class CategoryMasterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Name")]
        public string Name { get; set; } 

        public string? Description { get; set; }

        public bool? Status { get; set; }
        public string Active { get; set; }
    }
}
