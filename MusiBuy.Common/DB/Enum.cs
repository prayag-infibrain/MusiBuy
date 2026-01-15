using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Enum
{
    public int Id { get; set; }

    public int EnumTypeId { get; set; }

    public string EnumValue { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? ParantId { get; set; }

    public bool IsDeletable { get; set; }

    public bool IsEditable { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();

    public virtual ICollection<Creatore> Creatores { get; set; } = new List<Creatore>();

    public virtual EnumType EnumType { get; set; } = null!;

    public virtual ICollection<EventManagement> EventManagements { get; set; } = new List<EventManagement>();

    public virtual ICollection<FrontUser> FrontUsers { get; set; } = new List<FrontUser>();

    public virtual ICollection<Enum> InverseParant { get; set; } = new List<Enum>();

    public virtual Enum? Parant { get; set; }

    public virtual ICollection<PostManagement> PostManagementCategories { get; set; } = new List<PostManagement>();

    public virtual ICollection<PostManagement> PostManagementContentTypes { get; set; } = new List<PostManagement>();

    public virtual ICollection<PostManagement> PostManagementRoles { get; set; } = new List<PostManagement>();

    public virtual ICollection<PostManagement> PostManagementTypes { get; set; } = new List<PostManagement>();

    public virtual ICollection<TokenPlan> TokenPlans { get; set; } = new List<TokenPlan>();

    public virtual ICollection<UserPrefrence> UserPrefrences { get; set; } = new List<UserPrefrence>();
}
