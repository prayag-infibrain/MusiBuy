using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models
{
    public class EnumViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Enum type")]
        public int EnumTypeId { get; set; }
        public string? EnumTypeName { get; set; }
        public EnumTypeViewModel? EnumType { get; set; }
        public SelectList? EnumTypeList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Enum value")]
        public string EnumValue { get; set; } = string.Empty;
        public int? ParentTypeId { get; set; }
        public string? ParentType { get; set; }
        public SelectList? ParentTypeList { get; set; }
        public SelectList? SelectEnumType { get; set; }
        public int? ParentId { get; set; }
        public string? Parent { get; set; }
        public string? Description { get; set; }
        public SelectList? ParentList { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<EnumViewModel>? IListEnum { get; set; }
        public string? Active { get; set; }
        public CommonMessagesViewModel? ModuleName { get; set; }

        public bool IsEditable { get; set; }
        public string? Editable { get; set; }
        public bool IsDeleteable { get; set; }
        public string? Deletable { get; set; }
        public bool IsRecordUsed { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool Checked { get; set; }
        public List<EnumViewModel>? EnumList { get; set; }
        public int SortOrder { get; set; }
    }
}
