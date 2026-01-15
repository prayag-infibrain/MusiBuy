using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class InfluencerCategory
{
    public int Id { get; set; }

    public string InfluencerTypes { get; set; } = null!;

    public string Criteria { get; set; } = null!;

    public string EstimatedNumber { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }
}
