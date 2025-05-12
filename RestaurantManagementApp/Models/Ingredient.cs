using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal? CurrentStock { get; set; }

    public virtual ICollection<DishIngredient> DishIngredients { get; set; } = new List<DishIngredient>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
