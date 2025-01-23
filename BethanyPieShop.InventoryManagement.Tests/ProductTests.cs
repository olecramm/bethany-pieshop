using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using BethanyPieShop.InventoryMnagement.Domain.General;
using BethanyPieShop.InventoryMnagement.Domain.ProductManagement;

namespace BethanyPieShop.InventoryManagement.Tests
{
    public class ProductTests
    {
        [Fact]
        public void UseProduct_Reduce_AmountInStock()
        {
            //Arrange
            Product p1 = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100, 100);

            p1.IncreaseStock(100);

            //Act
            p1.UseProduct(20);

            //Assert
            Assert.Equal(80, p1.AmountInStock);
        }

        [Fact]
        public void UseProduct_ItemsHigherThanStock()
        {
            //Arrange
            Product p1 = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100, 100);

            p1.IncreaseStock(10);

            //Act
            p1.UseProduct(100);

            //Assert
            Assert.Equal(10, p1.AmountInStock);
        }

        [Fact]
        public void UseProduct_Reduces_AmountInStock_StockBelowThreshold()
        {
            //Arrange
            Product p1 = new RegularProduct(1, "Sugar", "Lorem ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro }, UnitType.PerKg, 100, 100);
            int increaseValue = 100;

            p1.IncreaseStock(increaseValue);

            //Act
            p1.UseProduct(increaseValue -1);

            //Assert
            Assert.True(p1.IsBelowStockThreshold);
        }
    }
}