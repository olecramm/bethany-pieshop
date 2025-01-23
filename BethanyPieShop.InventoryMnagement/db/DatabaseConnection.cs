using Microsoft.Data.SqlClient;

namespace BethanyPieShop.InventoryManagement.db
{
    public class DatabaseConnection
    {
        private readonly string _coonectionString;

        public DatabaseConnection(string connectionString)
        {
            _coonectionString = connectionString;
        }

        public SqlConnection GetConnection() { 

            return new SqlConnection(_coonectionString);
            
        }
    }
}
