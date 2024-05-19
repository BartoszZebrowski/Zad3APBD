using System.Data.SqlClient;
using Zad3.OrderSystem.Models;

namespace Zad3.OrderSystem
{
    public class ProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Product> GetProduct(int id)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnetion"]);
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Product @id");

            command.Parameters.AddWithValue("@id", id);

            var response = await command.ExecuteReaderAsync();

            if (!response.HasRows)
                return null;

            response.Read();
            
            var product = new Product
            {
                IdProduct = (int)response["IdProduct"],
                Name = response["Name"].ToString(),
                Description = response["Description"].ToString(),
                Price = (int)response["Price"]
            };

            return product;
        }
    }
}
