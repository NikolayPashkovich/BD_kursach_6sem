using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace RestaurantManagementApp.Services
{
    public static class OrderService
    {
        private static string SqlConnectionString = "Server=USER-PC\\SQLEXPRESS;Database=Restaurant_Plaza_Kursach;Trusted_Connection=True;TrustServerCertificate=true;";
        public static async Task<int> CreateOrderAsync(OrderDto order)
        {
            var dt = new DataTable();
            dt.Columns.Add("DishID", typeof(int));
            dt.Columns.Add("PromotionID", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("PaymentMethod", typeof(string));
            dt.Columns.Add("PaymentDate", typeof(DateTime));

            foreach (var d in order.Details)
            {
                dt.Rows.Add(d.DishID, (object?)d.PromotionID ?? DBNull.Value, d.Quantity, d.PaymentMethod, d.PaymentDate);
            }

            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_AddOrderWithDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@StaffID", order.StaffID);
            cmd.Parameters.AddWithValue("@TableID", order.TableID);
            cmd.Parameters.AddWithValue("@OrderDateTime", order.OrderDateTime);
            cmd.Parameters.AddWithValue("@Status", order.Status);

            var tvpParam = cmd.Parameters.AddWithValue("@Details", dt);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "OrderDetailsType";

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public static async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            using (var context = new RestaurantPlazaKursachContext())
            {
                var order = await context.Orders
                    .AsNoTracking()
                    .Include(o => o.Customer)
                    .Include(o => o.Staff)
                    .Include(o => o.Table)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Dish)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Promotion)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    return null;

                return new OrderDto
                {
                    OrderID = order.OrderId,
                    CustomerID = order.CustomerId,
                    StaffID = order.StaffId,
                    TableID = order.TableId,
                    Status = order.Status,
                    OrderDateTime = (DateTime)order.OrderDateTime,
                    TotalAmount = order.TotalAmount,
                    Details = order.OrderDetails.Select(od => new OrderDetailDto
                    {
                        DishID = od.DishId,
                        DishName = od.Dish?.DishName,
                        Quantity = od.Quantity,
                        PaymentMethod = od.PaymentMethod,
                        PaymentDate = (DateTime)od.PaymentDate,
                        PriceAtOrder = od.PriceAtOrder,
                        PromotionID = od.PromotionId,
                        PromotionName = od.Promotion?.PromotionName
                    }).ToList()
                };
            }
        }
        public static async Task UpdateOrderAsync(OrderDto dto)
        {
            // Собираем TVP с деталями заказа
            var dt = new DataTable();
            dt.Columns.Add("DishID", typeof(int));
            dt.Columns.Add("PromotionID", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("PaymentMethod", typeof(string));
            dt.Columns.Add("PaymentDate", typeof(DateTime));

            foreach (var d in dto.Details)
            {
                dt.Rows.Add(d.DishID, d.PromotionID ?? (object)DBNull.Value, d.Quantity, d.PaymentMethod, d.PaymentDate);
            }

            using (var conn = new SqlConnection(SqlConnectionString))
            {
                using (var cmd = new SqlCommand("sp_UpdateOrderWithDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrderID", dto.OrderID);
                    cmd.Parameters.AddWithValue("@CustomerID", dto.CustomerID ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@StaffID", dto.StaffID);
                    cmd.Parameters.AddWithValue("@TableID", dto.TableID);
                    cmd.Parameters.AddWithValue("@Status", dto.Status);
                    cmd.Parameters.AddWithValue("@OrderDateTime", dto.OrderDateTime);

                    var tvp = cmd.Parameters.AddWithValue("@Details", dt);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "OrderDetailsType";

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task DeleteOrderAsync(int orderId)
        {
            using (var context = new RestaurantPlazaKursachContext())
            {
                await context.Database.ExecuteSqlRawAsync("EXEC sp_DeleteOrder @p0", orderId);
            }
        }

    }


}
