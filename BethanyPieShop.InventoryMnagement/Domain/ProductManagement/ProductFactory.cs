using BethanyPieShop.InventoryManagement.Domain.General;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public static class ProductFactory
    {
        public static Product CreateProduct(int productType)
        {
            return productType switch
            {
                1 => new RegularProduct(new Price { }),
                2 => new BulkProduct(new Price { }),
                3 => new FreshProduct(new Price { }),
                4 => new BoxedProduct(new Price { }),
                _ => throw new ArgumentException($"Invalid product type: {productType}")

            };
        }
    }
}
