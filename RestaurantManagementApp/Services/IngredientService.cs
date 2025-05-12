using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RestaurantManagementApp.Services
{
    public class IngredientService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public IngredientService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            using var context = new RestaurantPlazaKursachContext();
            return await _context.Ingredients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ingredient updated)
        {
            var existing = await _context.Ingredients.FindAsync(updated.IngredientId);
            if (existing != null)
            {
                existing.IngredientName = updated.IngredientName;
                existing.Unit = updated.Unit;
                existing.CurrentStock = updated.CurrentStock;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return !await _context.Supplies.AnyAsync(s => s.IngredientId == id);
        }

        public async Task DeleteAsync(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        public bool IsNameDuplicate(string name, int? id = null)
        {
            return _context.Ingredients.Any(i =>
                i.IngredientName.ToLower() == name.ToLower() &&
                (!id.HasValue || i.IngredientId != id.Value));
        }
    }
}
