using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class ProducerExpertise
{
    public int Id { get; set; }

    public string ProducerExpertiseName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }
}
