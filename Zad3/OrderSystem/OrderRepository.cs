using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zad3.OrderSystem.Models;

namespace Zad3.OrderSystem
{
    public class OrderRepository
    {
        private readonly IConfiguration _configuration;

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Order WHERE IdOrder=@id");
            command.Parameters.AddWithValue("@id", orderId);

            var response = await command.ExecuteReaderAsync();
            response.Read();

            if (!response.HasRows)
                return null;

            var order = new Order
            {
                IdOrder = (int)response["IdOrder"],
                IdProduct = (int)response["IdProduct"],
                Amount = (int)response["Amount"],
                CreatedAt = DateTime.Parse(response["CratedAt"].ToString()),
                FulfilledAt = DateTime.Parse(response["FulfilledAt"].ToString())
            };

            return order;
        }

        public async Task<Order> GetOrdersByProduct(int productId)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Order WHERE IdProduct=@id");
            command.Parameters.AddWithValue("@id", productId);

            var response = await command.ExecuteReaderAsync();
            response.Read();

            if (!response.HasRows)
                return null;

            var order = new Order
            {
                IdOrder = (int)response["IdOrder"],
                IdProduct = (int)response["IdProduct"],
                Amount = (int)response["Amount"],
                CreatedAt = DateTime.Parse(response["CratedAt"].ToString()),
                FulfilledAt = DateTime.Parse(response["FulfilledAt"].ToString())
            };

            return order;
        }

        public async Task<ProductWarehouse> GetOrderRealizationByOrder(int orderId)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Product_Warehouse WHERE IdProduct=@id");
            command.Parameters.AddWithValue("@id", orderId);

            var response = await command.ExecuteReaderAsync();
            response.Read();

            if (!response.HasRows)
                return null;

            var productWarehouse = new ProductWarehouse
            {
                IdProductWarehouse = (int)response["IdProductWarehouse"],
                IdWarehouse = (int)response["IdWarehouse"],
                IdOrder = (int)response["IdOrder"],
                IdProduct = (int)response["IdProduct"],
                Amount = (int)response["Amount"],
                Price = (int)response["Price"],
                CreatedAt = DateTime.Parse(response["CratedAt"].ToString()),
            };

            return productWarehouse;
        }

        public async Task UpdateFullfilledAt(int orderId, DateTime date)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("UPDATE Order SET fulfilled_at = @date WHERE IdOrder = @id");
            command.Parameters.AddWithValue("@id", orderId);
            command.Parameters.AddWithValue("@date", date);

            var response = await command.ExecuteReaderAsync();
        }

        public async Task<int> AddOrderRealization(ProductWarehouse realization)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();

            string query = @"
                INSERT INTO ProductWarehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)
                SELECT SCOPE_IDENTITY();";

            var command = new SqlCommand(query);

            command.Parameters.AddWithValue("@IdProductWarehouse", realization.IdProductWarehouse);
            command.Parameters.AddWithValue("@IdWarehouse", realization.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", realization.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", realization.IdOrder);
            command.Parameters.AddWithValue("@Amount", realization.Amount);
            command.Parameters.AddWithValue("@Price", realization.Price);
            command.Parameters.AddWithValue("@CreatedAt", realization.CreatedAt);

            connection.Open();
            var newId = command.ExecuteScalar();
            return (int)newId;
        }
    }
}
