using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class PodcastGenre
{
    public int Id { get; set; }

    public string PodcastGenresName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProvidersNumber { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }
}
