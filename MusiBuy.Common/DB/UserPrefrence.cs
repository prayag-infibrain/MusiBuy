using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class UserPrefrence
{
    public int Id { get; set; }

    public int FrontUserId { get; set; }

    public int PrefrenceId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual FrontUser FrontUser { get; set; } = null!;

    public virtual Enum Prefrence { get; set; } = null!;
}
