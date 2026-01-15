using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class StripeWebhookLog
{
    public int Id { get; set; }

    public string? EventId { get; set; }

    public string? EventType { get; set; }

    public string? Payload { get; set; }

    public DateTime CreatedAt { get; set; }
}
