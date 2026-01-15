using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class CommonSetting
{
    public int Id { get; set; }

    public string Smtpserver { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Port { get; set; }

    public bool IsSsl { get; set; }

    public string SiteUrl { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string MarktingSiteUrl { get; set; } = null!;
}
