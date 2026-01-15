using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Creatore
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Bio { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string Password { get; set; } = null!;

    public int? Otp { get; set; }

    public DateTime? OtpCreatedOn { get; set; }

    public virtual Enum Role { get; set; } = null!;
}
