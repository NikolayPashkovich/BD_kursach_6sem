﻿using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
