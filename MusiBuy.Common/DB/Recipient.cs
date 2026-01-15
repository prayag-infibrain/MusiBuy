using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class Recipient
{
    public int Id { get; set; }

    public int TemplateId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual EmailTemplate Template { get; set; } = null!;
}
