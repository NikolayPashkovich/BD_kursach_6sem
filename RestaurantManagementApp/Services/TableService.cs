using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Services
{
    public static class TableService
    {
        private static string SqlConnectionString = "Server=USER-PC\\SQLEXPRESS;Database=Restaurant_Plaza_Kursach;Trusted_Connection=True;TrustServerCertificate=true;";

        public static async Task<int> AddTableAsync(string tableNumber, int capacity, bool isAvailable)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_AddTable", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@TableNumber", tableNumber);
            cmd.Parameters.AddWithValue("@Capacity", capacity);
            cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);

            await conn.OpenAsync();
            var result = await cmd.ExecuteNonQueryAsync();
            return result;
        }

        public static async Task<int> UpdateTableAsync(int tableId, string tableNumber, int capacity, bool isAvailable)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_UpdateTable", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@TableId", tableId);
            cmd.Parameters.AddWithValue("@TableNumber", tableNumber);
            cmd.Parameters.AddWithValue("@Capacity", capacity);
            cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);

            await conn.OpenAsync();
            var result = await cmd.ExecuteNonQueryAsync();
            return result;
        }

        public static async Task<int> DeleteTableAsync(int tableId)
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("sp_DeleteTable", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@TableId", tableId);

            await conn.OpenAsync();
            var result = await cmd.ExecuteNonQueryAsync();
            return result;
        }

        public static async Task<List<Table>> GetAllTablesAsync()
        {
            using var conn = new SqlConnection(SqlConnectionString);
            using var cmd = new SqlCommand("SELECT TableID, TableNumber, Capacity, IsAvailable FROM Tables", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            var tables = new List<Table>();
            while (await reader.ReadAsync())
            {
                tables.Add(new Table
                {
                    TableId = reader.GetInt32(0),
                    TableNumber = reader.GetString(1),
                    Capacity = reader.GetInt32(2),
                    IsAvailable = reader.GetBoolean(3)
                });
            }

            return tables;
        }
    }


}
