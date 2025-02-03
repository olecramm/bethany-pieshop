using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using System.Text;

namespace BethanyPieShop.InventoryManagement.Domain.ProductManagement
{
    public abstract class Product : ICloneable
    {

        #region Properties

        private int id;
        private string name = string.Empty;
        private string? description;

        protected int maxItemsInStock = 0;

        public static int StockThreshold = 5;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value.Length > 50 ? value[..50] : value;
            }
        }

        public string? Description
        {
            get { return description; }
            set
            {
                if (value == null)
                {
                    description = string.Empty;
                }
                else
                {
                    description = value.Length > 250 ? value[..250] : value;
                }
            }
        }

        public UnitType UnitType { get; set; }

        public int AmountInStock { get; set; }

        public bool IsBelowStockThreshold { get; protected set; }

        public Price Price { get; set; }

        public int MaxItemInStock
        {
            get { return maxItemsInStock; }
            set
            {
                maxItemsInStock = value;
            }
        }

        public int ProductType { get; set; }


        #endregion

        #region Constructors
        public Product(Price price)
        {
            Price = price;
        }

        public Product(int id) : this(id, string.Empty)
        {
        }

        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Product(int id, string name, string? description, Price price, UnitType unitType, int amountInStock, int maxItemInStock, int productType)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            UnitType = unitType;
            AmountInStock = amountInStock;
            MaxItemInStock = maxItemInStock;

            UpdateLowStock();
            ProductType = productType;
        }
        #endregion

        #region Methods

        public static void ChangeStockThreshold(int newStockThreshold)
        {
            if (newStockThreshold > 0)
            {
                StockThreshold = newStockThreshold;
            }
        }

        public virtual void UseProduct(int items)
        {
            if (items <= AmountInStock)
            {

                AmountInStock -= items;

                UpdateLowStock();

                Log($"Amount in stock update. Now {AmountInStock} items in stock");

            }
            else
            {
                Log($"Not enough items in stock for {items} requested.");
            }
        }

        //public virtual void IncreaseStock()
        //{
        //    AmountInStock++;
        //}

        public abstract void IncreaseStock();

        public virtual void IncreaseStock(int amount)
        {
            int newStock = AmountInStock + amount;

            if (newStock <= maxItemsInStock)
            {

                AmountInStock += amount;

            }
            else
            {

                AmountInStock = maxItemsInStock;
                Log($"{CreateSimpleProductRepresentention} stock eoverflow. {newStock - AmountInStock} item(s) ordered couldn't be stored.");

            }

            if (AmountInStock > StockThreshold)
            {
                IsBelowStockThreshold = false;
            }
        }

        protected virtual void DecreaseStock(int items, string reason)
        {
            if (items <= AmountInStock)
            {

                AmountInStock -= items;
            }
            else
            {
                AmountInStock = 0;
            }

            UpdateLowStock();

            Log(reason);
        }

        public virtual string DisplayDetailsShort()
        {
            return $"{id}. {name} \n{AmountInStock} items in stock";
        }

        public virtual string DisplayDetailsFull()
        {
            StringBuilder sb = new();

            sb.Append($"{id} {name} \n{description}\n{Price}\n{AmountInStock} item(s) in stock");

            if (IsBelowStockThreshold)
            {
                sb.Append("\n !!STOCK LOW");
            }

            return sb.ToString();

            //return DisplayDetailsFull("");
        }

        public virtual string DisplayDetailsFull(string extraDetails)
        {
            StringBuilder sb = new();

            sb.Append($"{Id} {Name} \n{Description}\n{Price}\n{AmountInStock} item(s) in stock");

            sb.Append(extraDetails);

            if (IsBelowStockThreshold)
            {
                sb.Append("\n!! STOCK LOW!!");
            }

            return sb.ToString();
        }

        public virtual void UpdateLowStock()
        {

            if (AmountInStock < StockThreshold)
            {

                IsBelowStockThreshold = true;

            }
        }

        protected static void Log(string message)
        {

            Console.WriteLine(message);

        }

        protected string CreateSimpleProductRepresentention()
        {
            return $"Product {id} ({name})";
        }

        public abstract object Clone();
    }
    #endregion
}