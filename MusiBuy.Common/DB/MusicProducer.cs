using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class MusicProducer
{
    public int Id { get; set; }

    public string ProducerType { get; set; } = null!;

    public string PrimaryExpertise { get; set; } = null!;

    public string GenresSpecialized { get; set; } = null!;

    public string KeyContributions { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }
}
