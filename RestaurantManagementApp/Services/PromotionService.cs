using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public class PromotionService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public PromotionService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetAllAsync()
        {
            return await _context.Promotions
                .AsNoTracking()
                .Include(p => p.Dishes)
                .OrderBy(p => p.StartDate)
                .ToListAsync();
        }

        public async Task AddAsync(Promotion promotion)
        {
            if (string.IsNullOrWhiteSpace(promotion.PromotionName))
                throw new ArgumentException("Название акции не может быть пустым.");

            if (promotion.DiscountPercent <= 0 || promotion.DiscountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            if (promotion.EndDate < promotion.StartDate)
                throw new ArgumentException("Дата окончания не может быть раньше даты начала.");

            var exists = await _context.Promotions.AnyAsync(p => p.PromotionName == promotion.PromotionName);
            if (exists)
                throw new InvalidOperationException("Акция с таким названием уже существует.");

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promotion updatedPromotion)
        {
            var existing = await _context.Promotions.FindAsync(updatedPromotion.PromotionId);
            if (existing == null)
                throw new InvalidOperationException("Акция не найдена.");

            var duplicate = await _context.Promotions
                .AnyAsync(p => p.PromotionName == updatedPromotion.PromotionName && p.PromotionId != updatedPromotion.PromotionId);
            if (duplicate)
                throw new InvalidOperationException("Акция с таким названием уже существует.");

            if (updatedPromotion.DiscountPercent <= 0 || updatedPromotion.DiscountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            if (updatedPromotion.EndDate < updatedPromotion.StartDate)
                throw new ArgumentException("Дата окончания не может быть раньше даты начала.");

            existing.PromotionName = updatedPromotion.PromotionName;
            existing.DiscountPercent = updatedPromotion.DiscountPercent;
            existing.StartDate = updatedPromotion.StartDate;
            existing.EndDate = updatedPromotion.EndDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Promotion promotion)
        {
            var existing = await _context.Promotions.FindAsync(promotion.PromotionId);
            if (existing != null)
            {
                _context.Promotions.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
        public List<Dish> GetAllDishes()
        {
            return _context.Dishes.OrderBy(d => d.DishName).ToList();
        }
        public List<int> GetPromotionDishIds(int promotionId)
        {
            return _context.Promotions
                .Where(p => p.PromotionId == promotionId)
                .SelectMany(p => p.Dishes.Select(d => d.DishId))
                .ToList();
        }
        public async Task UpdatePromotionDishesAsync(int promotionId, List<int> selectedDishIds)
        {
            var promotion = await _context.Promotions
                .Include(p => p.Dishes)
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId);

            if (promotion == null)
                throw new InvalidOperationException("Акция не найдена.");

            // Удаляем все текущие привязанные блюда
            promotion.Dishes.Clear();

            // Добавляем новые блюда
            var dishes = await _context.Dishes.Where(d => selectedDishIds.Contains(d.DishId)).ToListAsync();
            foreach (var dish in dishes)
            {
                promotion.Dishes.Add(dish);
            }

            await _context.SaveChangesAsync();
        }

    }
}
