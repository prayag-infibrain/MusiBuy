using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models
{
    public class ProteinsDataViewModel : BaseModel
    {
        public int Id { get; set; }

        public string? Accession { get; set; }

        public string? Description { get; set; }

        public decimal? Score { get; set; }

        public decimal? Coverage { get; set; }

        public int? Proteins { get; set; }

        public int? UniquePeptides { get; set; }

        public int? Peptides { get; set; }

        public int? Psms { get; set; }

        public int? Aas { get; set; }

        public decimal? MwkDa { get; set; }

        public decimal? CalcpI { get; set; }

        public int? SubjectId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "File")]
        public IFormFile? ImportedFile { get; set; }
        public string? strSubjectId { get; set; }

        public int UniqueCount { get; set; }

        public List<string>? ErrorList { get; set; }
        public DateTime? ProtienFileUploadedDate {get; set;}
        public IEnumerable<ProteinsDataViewModel> ProteinsData { get; set; }
        
    }
}
