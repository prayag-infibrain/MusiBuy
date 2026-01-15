using System;
using System.Collections.Generic;

namespace MusiBuy.Admin.DB;

public partial class EnumType
{
    public int Id { get; set; }

    public string EnumTypeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Enum> Enums { get; set; } = new List<Enum>();
}
