using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class FrontUser
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? CountryId { get; set; }

    public string Mobile { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Department { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string? Image { get; set; }

    public bool IsActive { get; set; }

    public int? Otp { get; set; }

    public DateTime? OtpCreatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? RoleId { get; set; }

    public int UserTypeId { get; set; }

    public string? Bio { get; set; }

    public virtual ICollection<BankMaster> BankMasters { get; set; } = new List<BankMaster>();

    public virtual ICollection<CommentsManagement> CommentsManagements { get; set; } = new List<CommentsManagement>();

    public virtual ICollection<ContactU> ContactUs { get; set; } = new List<ContactU>();

    public virtual ICollection<EventManagement> EventManagements { get; set; } = new List<EventManagement>();

    public virtual ICollection<FollowersManagement> FollowersManagementFollowIngs { get; set; } = new List<FollowersManagement>();

    public virtual ICollection<FollowersManagement> FollowersManagementUsers { get; set; } = new List<FollowersManagement>();

    public virtual ICollection<LikeManagement> LikeManagements { get; set; } = new List<LikeManagement>();

    public virtual ICollection<PostManagement> PostManagements { get; set; } = new List<PostManagement>();

    public virtual Enum? Role { get; set; }

    public virtual ICollection<UserPrefrence> UserPrefrences { get; set; } = new List<UserPrefrence>();

    public virtual ICollection<ViewManagement> ViewManagements { get; set; } = new List<ViewManagement>();
}
