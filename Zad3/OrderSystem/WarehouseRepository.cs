using System.Data.SqlClient;
using Zad3.OrderSystem.Models;

namespace Zad3.OrderSystem
{
    public class WarehouseRepository
    {
        private readonly IConfiguration _configuration;

        public WarehouseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Warehouse> GetWarehouse(int id)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Warehouse WHERE IdWarehouse = @id");

            command.Parameters.AddWithValue("@id", id);

            var response = await command.ExecuteReaderAsync();

            if (!response.HasRows)
                return null;

            response.Read();

            var warehouse = new Warehouse
            {
                IdWarehouse = (int)response["IdProduct"],
                Name = response["Name"].ToString(),
                Address = response["Address"].ToString(),
            };

            return warehouse;
        }
    }
}