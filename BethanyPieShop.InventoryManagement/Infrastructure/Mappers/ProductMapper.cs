using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using System.Data;

namespace BethanyPieShop.InventoryManagement.Infrastructure.Mappers
{
    public class ProductMapper
    {
        public Product Map(IDataReader reader, int productType)
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
