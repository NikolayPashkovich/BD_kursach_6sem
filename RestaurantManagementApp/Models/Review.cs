using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int OrderId { get; set; }

    public int Rating { get; set; }

    public virtual Order Order { get; set; } = null!;
}
