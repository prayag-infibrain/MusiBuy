using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Models
{
    public class TemplateViewModel : IValidatableObject
    {
        public int? Id { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Template name")]
        public string? TemplateName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        public string? Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Content")]
        public string? TemplateContent { get; set; }

        public bool IsActive { get; set; }
        public string? Active { get; set; }
        public string? Recipients { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Subject))
            {
                yield return new ValidationResult("Subject is required.");
            }
            if (string.IsNullOrWhiteSpace(TemplateContent))
            {
                yield return new ValidationResult("Content is required.");
            }
        }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public List<RecipientViewModel>? LstRecipients { get; set; }

    }
}