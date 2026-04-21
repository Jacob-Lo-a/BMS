using System;
using System.Collections.Generic;

namespace BMS.Core.Models;

public partial class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AppUser User { get; set; } = null!;
}
