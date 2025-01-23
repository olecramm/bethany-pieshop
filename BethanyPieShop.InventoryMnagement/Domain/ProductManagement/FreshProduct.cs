using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryMnagement.Domain.General;
using BethanyPieShop.InventoryMnagement.Domain.ProductManagement;
using System.Text;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public class FreshProduct : Product, ISavable
    {
        #region Properties

        public DateTime ExpiryDateTime { get; set; }
        public string? StorageInstructions { get; set; }

        private const int freshProductType = 3;

        #endregion

        #region Constructor
        public FreshProduct(int id,
                            string name,
                            string? description,
                            Price price,
                            UnitType unitType,
                            int amountInStock,
                            int maxAmountInStock)
                            : base(id, name, description, price, unitType, amountInStock, maxAmountInStock, freshProductType)
        {
        }

        public FreshProduct(Price price) : base(price) { }
        #endregion

        #region Methods
        public override string DisplayDetailsFull()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

            if (IsBelowStockThreshold)
            {
                sb.AppendLine("\n!!STOCK LOW!!");
            }

            sb.AppendLine("storage instructions: " + StorageInstructions);
            sb.AppendLine("Expire date: " + ExpiryDateTime.ToShortDateString());

            return sb.ToString();
        }

        public override void IncreaseStock()
        {
            AmountInStock++;
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{(int)Price.Currency};{(int)UnitType};2;";
        }

        public override object Clone()
        {
            return new FreshProduct(0, Name, Description, new Price() { ItemPrice = this.Price.ItemPrice, Currency = this.Price.Currency }, this.UnitType, this.AmountInStock, this.MaxItemInStock);
        }

        #endregion
    }
}
