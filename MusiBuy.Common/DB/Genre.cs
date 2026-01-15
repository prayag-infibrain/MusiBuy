using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Genre
{
    public int Id { get; set; }

    public string GenreName { get; set; } = null!;

    public string? Description { get; set; }

    public int CountryId { get; set; }

    public int RegionId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PostManagement> PostManagements { get; set; } = new List<PostManagement>();

    public virtual Region Region { get; set; } = null!;
}
