using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class ContactU
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int UserId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual FrontUser User { get; set; } = null!;
}
