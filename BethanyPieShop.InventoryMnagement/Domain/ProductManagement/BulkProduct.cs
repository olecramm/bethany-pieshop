using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public class BulkProduct : Product, ISavable
    {
        private const int bulkProductType = 2; 

        public BulkProduct(int id, string name, string? description, Price price, UnitType unitType, int amountInStock, int maxAmountInStock) 
                           : base(id, name, description, price, unitType, amountInStock, maxAmountInStock, bulkProductType)
        {

        }

        public BulkProduct(Price price) : base(price) { }

        public override object Clone()
        {
            return new BulkProduct(0, this.Name, this.Description, new Price() { ItemPrice = this.Price.ItemPrice, Currency = Price.Currency }, this.UnitType, this.AmountInStock ,this.maxItemsInStock);
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{(int)Price.Currency};{(int)UnitType};3;";
        }

        public override void IncreaseStock()
        {
            AmountInStock++;
        }
    }
}
