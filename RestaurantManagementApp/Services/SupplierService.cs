using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public static class SupplierService
    {
        public static async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            using var context = new RestaurantPlazaKursachContext();
            return await context.Suppliers.AsNoTracking().ToListAsync();
        }

        public static async Task AddSupplierAsync(Supplier supplier)
        {
            using var context = new RestaurantPlazaKursachContext();

            if (await context.Suppliers.AnyAsync(s => s.ContactPhone == supplier.ContactPhone))
                throw new Exception("Поставщик с таким номером телефона уже существует.");

            context.Suppliers.Add(supplier);
            await context.SaveChangesAsync();
        }

        public static async Task UpdateSupplierAsync(Supplier supplier)
        {
            using var context = new RestaurantPlazaKursachContext();

            if (await context.Suppliers.AnyAsync(s => s.ContactPhone == supplier.ContactPhone && s.SupplierId != supplier.SupplierId))
                throw new Exception("Поставщик с таким номером телефона уже существует.");

            context.Suppliers.Update(supplier);
            await context.SaveChangesAsync();
        }

        public static async Task DeleteSupplierAsync(int supplierId)
        {
            using var context = new RestaurantPlazaKursachContext();

            bool hasSupplies = await context.Supplies.AnyAsync(s => s.SupplierId == supplierId);
            if (hasSupplies)
                throw new Exception("Нельзя удалить поставщика, у которого есть поставки.");

            var supplier = await context.Suppliers.FindAsync(supplierId);
            if (supplier != null)
            {
                context.Suppliers.Remove(supplier);
                await context.SaveChangesAsync();
            }
        }
    }

}
