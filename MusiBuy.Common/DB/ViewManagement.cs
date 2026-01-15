using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class ViewManagement
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? PostId { get; set; }

    public int? EventId { get; set; }

    public DateTime CreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public virtual EventManagement? Event { get; set; }

    public virtual PostManagement? Post { get; set; }

    public virtual FrontUser User { get; set; } = null!;
}
