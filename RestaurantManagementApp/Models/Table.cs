using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Table
{
    public int TableId { get; set; }

    public string TableNumber { get; set; } = null!;

    public int Capacity { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
