using System;
using System.Collections.Generic;

namespace MusiBuy.Admin.DB;

public partial class Enum
{
    public int Id { get; set; }

    public int EnumTypeId { get; set; }

    public string EnumValue { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? ParantId { get; set; }

    public bool IsDeletable { get; set; }

    public bool IsEditable { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();

    public virtual EnumType EnumType { get; set; } = null!;

    public virtual ICollection<Enum> InverseParant { get; set; } = new List<Enum>();

    public virtual Enum? Parant { get; set; }
}
