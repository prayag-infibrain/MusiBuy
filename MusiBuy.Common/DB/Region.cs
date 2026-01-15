using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Region
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
