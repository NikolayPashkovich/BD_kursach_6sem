using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Models
{
    public class OrderWithDetailsViewModel
    {
        // Идентификатор заказа
        public int OrderId { get; set; }

        // Имя клиента
        public string CustomerName { get; set; }

        // Имя сотрудника, обслуживающего заказ
        public string StaffName { get; set; }

        // Номер стола
        public string TableNumber { get; set; }

        // Дата и время заказа
        public DateTime OrderDateTime { get; set; }

        // Сумма заказа
        public decimal TotalAmount { get; set; }

        // Статус заказа (например, "В процессе", "Завершен" и т.д.)
        public string Status { get; set; }

        // Список блюд, связанных с этим заказом (будет хранить их названия)
        public List<string> DishNames { get; set; }
        public int? Rating { get; set; }

    }
}
