using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class FollowersManagement
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FollowIngId { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public virtual FrontUser FollowIng { get; set; } = null!;

    public virtual FrontUser User { get; set; } = null!;
}
