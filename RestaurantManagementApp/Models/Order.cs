using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int StaffId { get; set; }

    public int TableId { get; set; }

    public int? PromotionId { get; set; }

    public DateTime? OrderDateTime { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Review? Review { get; set; }

    public virtual Staff Staff { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
    
    [NotMapped]
    public object Rating { get; internal set; }
}
