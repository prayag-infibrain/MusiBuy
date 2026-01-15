using System;
using System.Collections.Generic;

namespace MusiBuy.Admin.DB;

public partial class Content
{
    public int Id { get; set; }

    public int PageId { get; set; }

    public string Title { get; set; } = null!;

    public string Content1 { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Enum Page { get; set; } = null!;
}
