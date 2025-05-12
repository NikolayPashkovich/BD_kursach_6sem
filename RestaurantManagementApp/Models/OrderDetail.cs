using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int DishId { get; set; }

    public int? PromotionId { get; set; }

    public int Quantity { get; set; }

    public decimal PriceAtOrder { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Promotion? Promotion { get; set; }
}
