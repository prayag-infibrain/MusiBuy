using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class RolePrivilege
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int MenuItemId { get; set; }

    public bool? View { get; set; }

    public bool? Add { get; set; }

    public bool? Edit { get; set; }

    public bool? Delete { get; set; }

    public bool? Detail { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
