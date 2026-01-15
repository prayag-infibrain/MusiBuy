using MusiBuy.Common.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class InfluencerCategoriesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Influencer Types")]
        public string InfluencerTypes { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Criteria")]
        public string Criteria { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Estimated Number")]
        public string EstimatedNumber { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }
        public bool IsCurrentAdminUser { get; set; }
        public bool IsAdminUser { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
