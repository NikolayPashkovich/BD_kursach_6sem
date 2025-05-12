using System;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models;

public partial class Supply
{
    public int SupplyId { get; set; }

    public int SupplierId { get; set; }

    public int IngredientId { get; set; }

    public DateTime SupplyDate { get; set; }

    public decimal Quantity { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
