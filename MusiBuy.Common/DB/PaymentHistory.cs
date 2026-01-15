using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class PaymentHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public string StripeSessionId { get; set; } = null!;

    public int TokenType { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime PaidAt { get; set; }
}
