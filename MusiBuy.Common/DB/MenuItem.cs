using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class MenuItem
{
    public int Id { get; set; }

    public string MenuItemName { get; set; } = null!;

    public string? MenuItemController { get; set; }

    public string? MenuItemView { get; set; }

    public int SortOrder { get; set; }

    public int? ParentId { get; set; }

    public bool IsActive { get; set; }

    public string? ImageName { get; set; }

    public virtual ICollection<MenuItem> InverseParent { get; set; } = new List<MenuItem>();

    public virtual MenuItem? Parent { get; set; }

    public virtual ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
}
