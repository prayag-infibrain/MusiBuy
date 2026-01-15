namespace MusiBuy.Common.Models
{
    public class EnumTypeViewModel
    {
        public int Id { get; set; }
        public string? EnumTypeValue { get; set; }
        public bool IsActive { get; set; }
        public string? Active { get; set; }
        public bool IsDeleted { get; set; }
        public string? Deleted { get; set; }
        public List<EnumTypeViewModel>? IListEnumType { get; set; }
        public CommonMessagesViewModel? ModuleName { get; set; }

    }
}
