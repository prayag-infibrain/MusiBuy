using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models
{
    public class ProteinSummaryViewModel:BaseModel
    {
        public int Id { get; set; }
        public string? AccessionID { get; set; }
        public string? AccessiionName { get; set; }
        public string? Description { get; set; }
        public string? GeneNames { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "File")]
        public IFormFile? ImportedFile { get; set; }
        public List<string>? ErrorList { get; set; }
        public int? TotalPsms { get; set; }

        public decimal? MeanPsms { get; set; }

        public int? Count { get; set; }
   
    }
}
