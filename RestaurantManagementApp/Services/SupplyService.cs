using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public static class SupplyService
    {
        private static string SqlConnectionString = "Server=USER-PC\\SQLEXPRESS;Database=Restaurant_Plaza_Kursach;Trusted_Connection=True;TrustServerCertificate=true;";

        public static async Task<int> AddSupplyAsync(int supplierId, int ingredientId, DateTime supplyDate, decimal quantity)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_AddSupply", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplierID", supplierId);
            cmd.Parameters.AddWithValue("@IngredientID", ingredientId);
            cmd.Parameters.AddWithValue("@SupplyDate", supplyDate);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public static async Task UpdateSupplyAsync(int supplyId, int supplierId, int ingredientId, DateTime supplyDate, decimal quantity)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_UpdateSupply", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplyID", supplyId);
            cmd.Parameters.AddWithValue("@SupplierID", supplierId);
            cmd.Parameters.AddWithValue("@IngredientID", ingredientId);
            cmd.Parameters.AddWithValue("@SupplyDate", supplyDate);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task DeleteSupplyAsync(int supplyId)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_DeleteSupply", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SupplyID", supplyId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
