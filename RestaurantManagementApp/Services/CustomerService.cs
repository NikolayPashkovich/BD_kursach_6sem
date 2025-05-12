using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public class CustomerService
    {
        private readonly RestaurantPlazaKursachContext _context;

        public CustomerService(RestaurantPlazaKursachContext context)
        {
            _context = context;
        }
        // Проверка на уникальность телефона
        public bool IsPhoneDuplicate(string phone, int? excludeCustomerId = null)
        {
            return _context.Customers
                .Any(c => c.Phone == phone && c.CustomerId != excludeCustomerId);
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer updatedCustomer)
        {
            var existingCustomer = _context.Customers.Find(updatedCustomer.CustomerId);
            if (existingCustomer != null)
            {
                existingCustomer.FirstName = updatedCustomer.FirstName;
                existingCustomer.LastName = updatedCustomer.LastName;
                existingCustomer.Phone = updatedCustomer.Phone;

                _context.SaveChanges();
            }
        }


        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

    }

}
