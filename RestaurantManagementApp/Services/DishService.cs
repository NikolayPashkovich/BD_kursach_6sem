using RestaurantManagementApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantManagementApp.Services
{
    public class DishService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public DishService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public List<Dish> GetAllDishes()
        {
            return _context.Dishes.ToList();
        }

        public void AddDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            _context.SaveChanges();
        }

        public void UpdateDish(Dish dish)
        {
            var existingDish = _context.Dishes.FirstOrDefault(d => d.DishId == dish.DishId);
            if (existingDish != null)
            {
                existingDish.DishName = dish.DishName;
                existingDish.Price = dish.Price;
                existingDish.Category = dish.Category;
                existingDish.IsActive = dish.IsActive;
                _context.SaveChanges();
            }
        }

        public void DeleteDish(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                _context.SaveChanges();
            }
        }

        public bool IsDishNameDuplicate(string dishName, int? dishId = null)
        {
            return _context.Dishes.Any(d => d.DishName == dishName && d.DishId != dishId);
        }
        public void DeactivateDish(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.DishId == dishId);
            if (dish != null)
            {
                dish.IsActive = false;
                _context.SaveChanges();
            }
        }
    }
}
