using System;
using System.Collections.Generic;

namespace MusiBuy.Admin.DB;

public partial class AdminUser
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string? Designation { get; set; }

    public string? Department { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public bool IsAdminUser { get; set; }

    public string? OldPassword { get; set; }

    public string? OldPasswordHasKey { get; set; }

    public virtual Role Role { get; set; } = null!;
}
