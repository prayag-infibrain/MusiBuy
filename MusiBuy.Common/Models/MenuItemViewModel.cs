namespace MusiBuy.Common.Models
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public string MenuItemController { get; set; } = string.Empty;
        public string MenuItemView { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}
