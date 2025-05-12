using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RestaurantManagementApp.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    [NotMapped]
    public List<string> DishNames => Dishes.Select(d => d.DishName).ToList();

}
