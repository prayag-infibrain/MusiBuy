using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class TokenPlan
{
    public int Id { get; set; }

    public int TokenTypeId { get; set; }

    public decimal? TokenQuantity { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Enum TokenType { get; set; } = null!;
}
