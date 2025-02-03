using BethanyPieShop.InventoryManagement.Domain.General;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public class FreshBoxedProduct : BoxedProduct
    {

        #region Constructors
        public FreshBoxedProduct(int id, 
                                string name, 
                                string? description, 
                                Price price, int amounInStock, int amountPerBox, int maxAmountInStock) 
                                : base(id, name, description, price, amounInStock, amountPerBox, maxAmountInStock)
        {
        }
        #endregion
        #region Methods
        public void UseFreshBoxedProduct(int items)
        {
            //UseBoxedProduct(items);
        }
        #endregion

    }
}
