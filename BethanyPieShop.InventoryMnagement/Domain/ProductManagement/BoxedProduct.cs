using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryMnagement.Domain.General;
using BethanyPieShop.InventoryMnagement.Domain.ProductManagement;
using System.Text;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public class BoxedProduct : Product, ISavable, ILoggable
    {
        #region Properties
        private int amountPerBox;

        public int AmountPerBox
        {
            get { return amountPerBox; }
            set
            {
                amountPerBox = value;
            }
        }

        private const int boxedProductType = 4;

        #endregion

        #region Contructor
        public BoxedProduct(int id,
                            string name,
                            string? description,
                            Price price,
                            int amountInStock,
                            int amountPerBox,
                            int maxAmountInStock)
                            : base(id, name, description, price, UnitType.PerBox, amountInStock, maxAmountInStock, boxedProductType)
        {
            AmountPerBox = amountPerBox;
        }

        public BoxedProduct(Price price) : base(price) { }
        #endregion

        #region Methods

        //public string DisplayBoxedProductDetails()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("Boxed Product\n");

        //    sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

        //    if (IsBelowStockThreshold)
        //    {
        //        sb.Append("\n!!STOCK LOW!!");
        //    }

        //    return sb.ToString();
        //}

        public override string DisplayDetailsFull()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Boxed Product\n");

            sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

            if (IsBelowStockThreshold) {
                sb.Append("\n!!STOCK LOW!!");
            }

            return sb.ToString();
        }

        public override void UseProduct(int items)
        {
            int smallestMultiple = 0;
            int batchSize;

            while (true)
            {
                smallestMultiple++;
                if (smallestMultiple * AmountPerBox > items)
                {
                    batchSize = smallestMultiple * AmountPerBox;
                    break;
                }
            }

            base.UseProduct(batchSize);
        }

        public override void IncreaseStock(int amount)
        {

            int newStock = AmountInStock + amount * AmountPerBox;

            if (newStock <= maxItemsInStock)
            {
                AmountInStock += amount * AmountPerBox;
            }
            else 
            { 
                AmountInStock = maxItemsInStock;

                Log($"{CreateSimpleProductRepresentention} stock overflow. {newStock - AmountInStock} item(s) ordered that couldn't be stored");            
            }

            if (AmountInStock > StockThreshold)
            {
                IsBelowStockThreshold = false;
            }

            AmountInStock += AmountPerBox;
        }

        public override void IncreaseStock()
        {
            AmountInStock++;
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{(int)Price.Currency};{(int)UnitType};1;{AmountPerBox};";
        }

        void ILoggable.Log(string message)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            return new BoxedProduct(0, this.Name, this.Description, new Price() { ItemPrice = this.Price.ItemPrice, Currency = this.Price.Currency }, this.maxItemsInStock, this.amountPerBox, this.MaxItemInStock);
        }

        #endregion
    }
}
