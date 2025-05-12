using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public class DishIngredientService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public DishIngredientService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public async Task<List<DishIngredient>> GetAllAsync()
        {
            return await _context.DishIngredients
                .AsNoTracking()
                .Include(di => di.Dish)
                .Include(di => di.Ingredient)
                .ToListAsync();
        }

        public async Task AddAsync(DishIngredient dishIngredient)
        {
            bool exists = await _context.DishIngredients.AnyAsync(di =>
                di.DishId == dishIngredient.DishId && di.IngredientId == dishIngredient.IngredientId);

            if (exists)
                throw new InvalidOperationException("Такой рецепт уже существует!");

            if (dishIngredient.Quantity < 0.1m || dishIngredient.Quantity > 5000)
                throw new ArgumentOutOfRangeException("Количество должно быть от 0.1 до 5000");

            _context.DishIngredients.Add(dishIngredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DishIngredient updated)
        {
            var existing = await _context.DishIngredients
                .FirstOrDefaultAsync(di => di.DishId == updated.DishId && di.IngredientId == updated.IngredientId);

            if (existing == null)
                throw new InvalidOperationException("Рецепт не найден");

            if (updated.Quantity < 0.1m || updated.Quantity > 5000)
                throw new ArgumentOutOfRangeException("Количество должно быть от 0.1 до 5000");

            existing.Quantity = updated.Quantity;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DishIngredient recipe)
        {
            _context.DishIngredients.Remove(recipe);
            await _context.SaveChangesAsync();
        }
    }
}
