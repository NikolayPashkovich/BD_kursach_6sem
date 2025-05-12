using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace RestaurantManagementApp.Services
{
    public class MenuService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public MenuService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetAllAsync()
        {
            return await _context.Menus
                .AsNoTracking()
                .Include(m => m.Dishes) // ⬅ важно
                .OrderBy(m => m.StartDate)
                .ToListAsync();
        }


        public async Task AddAsync(Menu menu)
        {
            if (string.IsNullOrWhiteSpace(menu.MenuName))
                throw new ArgumentException("Название меню не может быть пустым.");

            var exists = await _context.Menus.AnyAsync(m => m.MenuName == menu.MenuName);
            if (exists)
                throw new InvalidOperationException("Меню с таким названием уже существует.");

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Menu updatedMenu)
        {
            var existingMenu = await _context.Menus.FindAsync(updatedMenu.MenuId);
            if (existingMenu == null)
                throw new InvalidOperationException("Меню не найдено.");

            var duplicate = await _context.Menus
                .AnyAsync(m => m.MenuName == updatedMenu.MenuName && m.MenuId != updatedMenu.MenuId);
            if (duplicate)
                throw new InvalidOperationException("Меню с таким названием уже существует.");

            existingMenu.MenuName = updatedMenu.MenuName;
            existingMenu.StartDate = updatedMenu.StartDate;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Menu menu)
        {
            var existingMenu = await _context.Menus.FindAsync(menu.MenuId);
            if (existingMenu != null)
            {
                _context.Menus.Remove(existingMenu);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateMenuDishesAsync(int menuId, List<int> selectedDishIds)
        {
            var menu = await _context.Menus
                .Include(m => m.Dishes)
                .FirstOrDefaultAsync(m => m.MenuId == menuId);

            if (menu == null)
                throw new InvalidOperationException("Меню не найдено.");

            // Удаление всех текущих блюд
            menu.Dishes.Clear();

            // Добавление новых
            var dishes = await _context.Dishes.Where(d => selectedDishIds.Contains(d.DishId)).ToListAsync();
            foreach (var dish in dishes)
            {
                menu.Dishes.Add(dish);
            }

            await _context.SaveChangesAsync();
        }
        public List<Dish> GetAllDishes()
        {
            return _context.Dishes.OrderBy(d => d.DishName).ToList();
        }

        public List<int> GetMenuDishIds(int menuId)
        {
            return _context.Menus
                .Where(m => m.MenuId == menuId)
                .SelectMany(m => m.Dishes.Select(d => d.DishId))
                .ToList();
        }

    }

}
