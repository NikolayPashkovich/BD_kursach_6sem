using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class DishIngredient
{
    public int DishId { get; set; }

    public int IngredientId { get; set; }

    public decimal Quantity { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Ingredient Ingredient { get; set; } = null!;
}
