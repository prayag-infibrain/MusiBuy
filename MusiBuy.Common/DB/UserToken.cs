using System;
using System.Collections.Generic;

namespace MusiBuy.Common.DB;

public partial class UserToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenType { get; set; }

    public int Quantity { get; set; }

    public DateTime UpdatedAt { get; set; }
}
