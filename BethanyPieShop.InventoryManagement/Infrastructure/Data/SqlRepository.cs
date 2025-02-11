using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using BethanyPieShop.InventoryManagement.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BethanyPieShop.InventoryManagement.Infrastructure.Data
{
    public class SqlRepository : IDataRepository<Product>
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly DatabaseConnection _connection;
        private readonly ProductMapper _productMapper;

        public SqlRepository()
        {
            _configuration = Configuration.Build();
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
            _connection = new DatabaseConnection(_connectionString);
            _productMapper = new ProductMapper();
        }

        public void AddProduct(Product product)
        {
            try
            {
                var query = "INSERT INTO Product (Name, Description, AmountInStock, Price, CurrencyID, UnitTypeID, ProductTypeID, MaxAmountInStock) " +
                    "        VALUES (@Name, @Description, @AmountInStock, @Price, @CurrencyID, @UnitTypeID, @ProductTypeID, @MaxAmountInStock)";

                using var connection = _connection.GetConnection();
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@AmountInStock", product.AmountInStock);
                command.Parameters.AddWithValue("@Price", product.Price.ItemPrice);
                command.Parameters.AddWithValue("@CurrencyID", (int)product.Price.Currency);
                command.Parameters.AddWithValue("@UnitTypeID", (int)product.UnitType);
                command.Parameters.AddWithValue("@ProductTypeID", product.ProductType);
                command.Parameters.AddWithValue("@MaxAmountInStock", product.MaxItemInStock);

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

            var productList = new List<Product>();

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

                    var productMapped = _productMapper.Map(reader, productType);

                    productList.Add(productMapped);
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

            return productList;
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
                    foundProduct = _productMapper.Map(reader, productType);
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
    }
}
