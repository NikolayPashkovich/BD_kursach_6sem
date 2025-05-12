using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public string Position { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
