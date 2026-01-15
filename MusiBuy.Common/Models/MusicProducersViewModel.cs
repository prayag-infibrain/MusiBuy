using MusiBuy.Common.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class MusicProducersViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Producer Type")]
        public string ProducerType { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Primary Expertise")]
        public string PrimaryExpertise { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Genres Specialized")]
        public string GenresSpecialized { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Key Contributions")]
        public string KeyContributions { get; set; } = null!;
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
