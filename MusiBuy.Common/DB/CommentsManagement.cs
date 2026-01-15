using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class CommentsManagement
{
    public int Id { get; set; }

    public int? PostId { get; set; }

    public int UserId { get; set; }

    public string? Comment { get; set; }

    public DateTime? Timestamp { get; set; }

    public int? StatusId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int UserType { get; set; }

    public int? EventId { get; set; }

    public virtual EventManagement? Event { get; set; }

    public virtual PostManagement? Post { get; set; }

    public virtual FrontUser User { get; set; } = null!;
}
