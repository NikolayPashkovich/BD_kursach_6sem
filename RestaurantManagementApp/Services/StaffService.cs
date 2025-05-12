using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantManagementApp.Services
{
    public class StaffService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public StaffService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }

        public async Task<List<Staff>> GetAllAsync()
        {
            return await _context.Staff.ToListAsync();
        }

        public async Task AddAsync(Staff staff)
        {
            _context.Staff.Add(staff);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Staff staff)
        {
            var existingStaff = await _context.Staff.FindAsync(staff.StaffId);
            if (existingStaff != null)
            {
                existingStaff.FirstName = staff.FirstName;
                existingStaff.LastName = staff.LastName;
                existingStaff.Position = staff.Position;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(Staff staff)
        {
            var orders = await _context.Orders
                .Where(o => o.StaffId == staff.StaffId)
                .ToListAsync();

            if (orders.Any())
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Этот сотрудник участвует в {orders.Count} заказах. Вы хотите удалить его вместе с заказами?",
                    "Предупреждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    _context.Orders.RemoveRange(orders);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }

            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }

    }
}
