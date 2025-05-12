using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionName { get; set; } = null!;

    public decimal DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    public List<string> DishNames => Dishes.Select(d => d.DishName).ToList();
}
