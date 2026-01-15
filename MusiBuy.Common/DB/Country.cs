using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Country
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public int? RegionId { get; set; }

    public string? CountryShortCode { get; set; }

    public virtual ICollection<AdminUser> AdminUsers { get; set; } = new List<AdminUser>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<MarketingUser> MarketingUsers { get; set; } = new List<MarketingUser>();

    public virtual ICollection<PostManagement> PostManagements { get; set; } = new List<PostManagement>();

    public virtual Region? Region { get; set; }
}
