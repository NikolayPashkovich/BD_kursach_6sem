using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Models
{
    public class OrderDetailDto
    {
        public int DishID { get; set; }
        public int? PromotionID { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        // для отображения в таблице
        public string DishName { get; set; }
        public string PromotionName { get; set; }
        public decimal PriceAtOrder { get;  set; }
    }

    public class OrderDto
    {
        public int OrderID { get; set; } 
        public int? CustomerID { get; set; }
        public int StaffID { get; set; }
        public int TableID { get; set; }
        public string Status { get; set; }
        public DateTime OrderDateTime { get; set; }
        public List<OrderDetailDto> Details { get; set; }
        public decimal TotalAmount { get;  set; }
    }
}
