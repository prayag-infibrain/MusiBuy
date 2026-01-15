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
    public class ClinicalDataViewModel:BaseModel
    {
        public List<string>? ErrorList { get; set; }
        public string? SubjectID { get; set; }
        public string? SampleID { get; set; }
        public DateTime? CollectionTime { get; set; }   
        public string? Eye { get; set; }
        public int? MmOfSample { get; set; }
        public decimal? OSDIScore { get; set; }
        public int? TBUT { get; set; }
        public int? CornealStainingMid { get; set; }
        public int? CornealStainingTop { get; set; }
        public int? CornealStainingLeft { get; set; }
        public int? CornealStainingRight { get; set; }
        public int? CornealStainingBottom { get; set; }
        public int? ConjunctivalStainingLeft { get; set; }
        public int? ConjunctivalStainingLeftTop { get; set; }
        public int? ConjunctivalStainingLeftBottom { get; set; }
        public int? ConjunctivalStainingRight { get; set; }
        public int? ConjunctivalStainingRightTop { get; set; }
        public int? ConjunctivalStainingRightBottom { get; set; }
        public int? LidMarginScore { get; set; }
        public int? ExpressibilityScore { get; set; }
        public int? MeibumScore { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? Race { get; set; }
        public string? Ethnicity { get; set; }
        public string? OcularDiseases { get; set; }
        public string? OcularMedications { get; set; }
        public string? OtherDiseases { get; set; }
        public string? OtherMedications { get; set; }
        public bool? ArtificialTears { get; set; }
        public bool? Xiidra { get; set; }
        public bool? Restasis { get; set; }
        public bool? Sequoa { get; set; }
        public bool? TerviaNasalDrops { get; set; }
        public bool? TopicalSteroids { get; set; }
        public bool? Antihistamine { get; set; }
        public bool? Prostaglandin { get; set; }
        public bool? BetaBlocker { get; set; }
        public int? Iop { get; set; }
        public string? Comments { get; set; }
        public string? KeyName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "File")]
        public IFormFile? ImportedFile { get; set; }
        public string? FileName { get; set; }

        public int Id { get; set; }
        public int? RowNo { get; set; }
    }
}
