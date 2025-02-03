using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public class RegularProduct : Product, ISavable
    {
        private const int regularProductType = 1;

        public RegularProduct(int id, string name, string? description, Price price, UnitType unitType, int amountInStock, int maxAmountInStock) 
                              : base(id, name, description, price, unitType, amountInStock, maxAmountInStock, regularProductType)
        {
        }

        public RegularProduct(Price price) : base(price) { }


        public override object Clone()
        {
            return new RegularProduct(0, Name, Description, new Price() {ItemPrice = this.Price.ItemPrice, Currency = this.Price.Currency }, this.UnitType, this.AmountInStock, this.MaxItemInStock);
        }

        public string ConvertToStringForSaving()
        {
            return $"{Id};{Name};{Description};{maxItemsInStock};{Price.ItemPrice};{(int)Price.Currency};{(int)UnitType};4;";
        }

        public override void IncreaseStock()
        {
            AmountInStock++;
        }
    }
}
