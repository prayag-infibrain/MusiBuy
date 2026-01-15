using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class EventManagement
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CreatorId { get; set; }

    public int EventTypeId { get; set; }

    public DateTime EventStartDateTime { get; set; }

    public DateTime EventEndDateTime { get; set; }

    public int? StatusId { get; set; }

    public string? RecordingUrl { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int UserTypeId { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CommentsManagement> CommentsManagements { get; set; } = new List<CommentsManagement>();

    public virtual ICollection<LikeManagement> LikeManagements { get; set; } = new List<LikeManagement>();

    public virtual Enum Role { get; set; } = null!;

    public virtual FrontUser User { get; set; } = null!;

    public virtual ICollection<ViewManagement> ViewManagements { get; set; } = new List<ViewManagement>();
}
