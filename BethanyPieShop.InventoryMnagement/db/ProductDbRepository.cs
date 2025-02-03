using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BethanyPieShop.InventoryManagement.db
{
    public class ProductDbRepository : IRepository<Product>
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly DatabaseConnection _connection;

        public ProductDbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
            _connection = new DatabaseConnection(_connectionString);
        }

        public void AddProduct(Product entity)
        {
            try
            {
                var query = "INSERT INTO Product (Name, Description, AmountInStock, Price, CurrencyID, UnitTypeID, ProductTypeID, MaxAmountInStock) " +
                    "        VALUES (@Name, @Description, @AmountInStock, @Price, @CurrencyID, @UnitTypeID, @ProductTypeID, @MaxAmountInStock)";

                using var connection = _connection.GetConnection();
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@AmountInStock", entity.AmountInStock);
                command.Parameters.AddWithValue("@Price", entity.Price.ItemPrice);
                command.Parameters.AddWithValue("@CurrencyID", (int)entity.Price.Currency);
                command.Parameters.AddWithValue("@UnitTypeID", (int)entity.UnitType);
                command.Parameters.AddWithValue("@ProductTypeID", entity.ProductType);
                command.Parameters.AddWithValue("@MaxAmountInStock", entity.MaxItemInStock);

                connection.Open();
                command.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                // Handle SQL Server-specific errors
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                // Handle timeout
                Console.WriteLine($"Timeout Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Handle invalid operations
                Console.WriteLine($"Invalid Operation: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // Handle argument errors
                Console.WriteLine($"Argument Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }

        public List<Product> GetAllProducts()
        {

            var entities = new List<Product>();

            try
            {
                var query = "SELECT ProductID, Name, Description, AmountInStock, Price, CurrencyID, UnitTypeID, ProductTypeID, MaxAmountInStock FROM Product";

                using var connection = _connection.GetConnection();
                using var command = new SqlCommand(query, connection);
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var productType = reader.GetInt32(7);

                    var productMapped = MapToProduct(reader, productType);

                    entities.Add(productMapped);
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server-specific errors
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                // Handle timeout
                Console.WriteLine($"Timeout Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Handle invalid operations
                Console.WriteLine($"Invalid Operation: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // Handle argument errors
                Console.WriteLine($"Argument Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return entities;
        }

        public Product GetProductById(int id)
        {
            Product? foundProduct = null;

            try
            {
                using SqlConnection connection = new(_connectionString);
                string query = "SELECT ProductID, Name, Description, AmountInStock, Price, CurrencyID, UnitTypeID, ProductTypeID, MaxAmountInStock FROM Product WHERE ProductID = @Id ";

                using SqlCommand command = new(query, connection);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var productType = reader.GetInt32(7);
                    foundProduct = ProductFactory.CreateProduct(productType);

                    MapToProduct(reader, productType);
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server-specific errors
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                // Handle timeout
                Console.WriteLine($"Timeout Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Handle invalid operations
                Console.WriteLine($"Invalid Operation: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                // Handle argument errors
                Console.WriteLine($"Argument Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
            return foundProduct ?? throw new InvalidOperationException("Product not found");
        }

        public void UpdateProduct(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(
                "UPDATE Product SET AmountInStock = @AmountInStock WHERE ProductId = @ProductId",
                connection);
            command.Parameters.AddWithValue("@AmountInStock", product.AmountInStock);
            command.Parameters.AddWithValue("@ProductId", product.Id);

            command.ExecuteNonQuery();
        }

        private static Product MapToProduct(SqlDataReader reader, int productType)
        {
            var product = ProductFactory.CreateProduct(productType);

            product.Id = reader.GetInt32(0);
            product.Name = reader.GetString(1);
            product.Description = reader.GetString(2);
            product.AmountInStock = reader.GetInt32(3);
            product.Price.ItemPrice = (double)(reader.GetDecimal(4));
            product.Price.Currency = (Currency)reader.GetInt32(5);
            product.UnitType = (UnitType)reader.GetInt32(6);
            product.ProductType = reader.GetInt32(7);
            product.MaxItemInStock = reader.GetInt32(8);

            return product;
        }
    }
}
