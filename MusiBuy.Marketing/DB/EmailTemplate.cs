using System;
using System.Collections.Generic;

namespace MusiBuy.Admin.DB;

public partial class EmailTemplate
{
    public int Id { get; set; }

    public string TemplateName { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string TemplateContent { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
}
