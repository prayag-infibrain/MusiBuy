using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class BankMaster
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public string Ifsccode { get; set; } = null!;

    public string? AccountHolder { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsSelected { get; set; }

    public bool IsActive { get; set; }

    public virtual FrontUser User { get; set; } = null!;
}
