using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Dish
{
    public int DishId { get; set; }

    public string DishName { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
