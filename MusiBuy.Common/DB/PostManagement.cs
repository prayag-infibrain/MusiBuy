using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class PostManagement
{
    public int Id { get; set; }

    public int CreatorId { get; set; }

    public int? TypeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? MediaFile { get; set; }

    public string? Url { get; set; }

    public string? Tags { get; set; }

    public int StatusId { get; set; }

    public DateTime PublishDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int UserTypeId { get; set; }

    public int RoleId { get; set; }

    public int UserId { get; set; }

    public int ContentTypeId { get; set; }

    public int? CountryId { get; set; }

    public int? GenreId { get; set; }

    public int? CategoryId { get; set; }

    public int? MediaTypeId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Enum? Category { get; set; }

    public virtual ICollection<CommentsManagement> CommentsManagements { get; set; } = new List<CommentsManagement>();

    public virtual Enum ContentType { get; set; } = null!;

    public virtual Country? Country { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<LikeManagement> LikeManagements { get; set; } = new List<LikeManagement>();

    public virtual Enum Role { get; set; } = null!;

    public virtual Enum? Type { get; set; }

    public virtual FrontUser User { get; set; } = null!;

    public virtual ICollection<ViewManagement> ViewManagements { get; set; } = new List<ViewManagement>();
}
