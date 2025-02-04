using BethanyPieShop.InventoryManagement.db;
using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using BethanyPieShop.InventoryManagement.Domain.General;
using BethanyPieShop.InventoryManagement.Domain.OrderManagement;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BethanyPieShop.InventoryManagement
{
    internal class Utilities
    {

        private static List<Product> inventory = [];
        private static readonly List<Order> orders = [];
        private static readonly ProductDbRepository productDbRepository = new(ConfigurationBuilder());


        internal static void InitializeStock()
        {
            //BoxedProduct bp = new BoxedProduct(6, "Eggs", "Lorem Ipsum", new Price() { ItemPrice = 10, Currency = Currency.Euro },100, 6);

            //bp.IncreaseStock(100);
            //bp.UseProduct(10);

            //ProductRepository productRepository = new();

            //inventory = productRepository.LoadProductFromFile(product);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded {inventory.Count} products");

            Console.WriteLine("Press enter to continue!.");
            Console.ResetColor();

            Console.ReadLine();

            //Product p1 = new Product(1) { 
            //    Name = "Sugar", 
            //    Description = "Lorem ipsum", 
            //    Price = new Price() { ItemPrice = 10, Currency = Currency.Euro }, 
            //    UnitType = UnitType.PerKg };

            //var p2 = new Product(2)
            //{
            //    Name = "Cake decorations",
            //    Description = "Lorem ipsum",
            //    Price = new Price() { ItemPrice = 8, Currency = Currency.Euro },
            //    UnitType = UnitType.PerItem
            //};

            //Product p3 = new Product(3)
            //{
            //    Name = "Straberry",
            //    Description = "Lorem ipsum",
            //    Price = new Price() { ItemPrice = 3, Currency = Currency.Euro },
            //    UnitType = UnitType.PerBox
            //};

            //inventory.Add(p1);
            //inventory.Add(p2);
            //inventory.Add(p3);
        }

        internal static void ShowMainMenu()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("********************");
            Console.WriteLine("* Select an action *");
            Console.WriteLine("********************");

            Console.WriteLine("1: Inventory management");
            Console.WriteLine("2: Order management");
            Console.WriteLine("3: Settings");
            Console.WriteLine("4: Save all data");
            Console.WriteLine("0: Close application");

            Console.Write("Your selection: ");

            string? userSelection = Console.ReadLine();

            switch (userSelection)
            {
                case "1":
                    ShowInventoryManagementMenu();
                    break;
                case "2":
                    ShowOrderManagementMenu();
                    break;
                case "3":
                    ShowSettingsMenu();
                    break;
                case "4":
                    SaveAllData();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again");
                    break;
            }
        }

        private static void SaveAllData()
        {
            ProductRepository productRepository = new();

            List<ISavable> savables = [];

            foreach (var item in inventory)
            {
                if (item is ISavable)
                {
                    savables.Add(item as ISavable);
                }
            }

            productRepository.SaveToFile(savables);

            Console.ReadLine();
            ShowMainMenu();
        }

        private static void ShowSettingsMenu()
        {
            string? userSelection;

            do
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("************");
                Console.WriteLine("* Settings *");
                Console.WriteLine("************");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What do you want to do?");
                Console.ResetColor();

                Console.WriteLine("1: Change stock treshold");
                Console.WriteLine("0: Back to main menu");

                Console.Write("Your selection: ");

                userSelection = Console.ReadLine();

                switch (userSelection)
                {
                    case "1":
                        ShowChangeStockTreshold();
                        break;

                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            while (userSelection != "0");
            ShowMainMenu();
        }

        private static void ShowChangeStockTreshold()
        {
            Console.WriteLine($"Enter the new stock treshold (current value: {Product.StockThreshold}). This applies to all products!");
            Console.Write("New value: ");
            int newValue = int.Parse(Console.ReadLine() ?? "0");
            Product.StockThreshold = newValue;
            Console.WriteLine($"New stock treshold set to {Product.StockThreshold}");

            foreach (var product in inventory)
            {
                product.UpdateLowStock();
            }

            Console.ReadLine();
        }

        private static void ShowOrderManagementMenu()
        {
            string? userSelection;
            do
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("********************");
                Console.WriteLine("* Select an action *");
                Console.WriteLine("********************");

                Console.WriteLine("1: Open order overview");
                Console.WriteLine("2: Add new order");
                Console.WriteLine("0: Back to main menu");

                Console.Write("Your selection: ");

                userSelection = Console.ReadLine();

                switch (userSelection)
                {
                    case "1":
                        ShowOpenOrderOverview();
                        break;
                    case "2":
                        ShowAddNewOrder();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            while (userSelection != "0");
            ShowMainMenu();
        }

        private static void ShowAddNewOrder()
        {
            Order newOrder = new();
            string? selectedProductId = string.Empty;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Creating new order");
            Console.ResetColor();

            do
            {
                ShowAllProductsOverview();

                Console.WriteLine("Which product do you want to order? (enter 0 to stop adding new products to the order)");

                Console.Write("Enter the ID of product: ");
                selectedProductId = Console.ReadLine();

                if (selectedProductId != "0")
                {
                    Product? selectedProduct = inventory.Where(p => p.Id == int.Parse(selectedProductId)).FirstOrDefault();

                    if (selectedProduct != null)
                    {
                        Console.Write("How many do you want to order?: ");
                        int amountOrdered = int.Parse(Console.ReadLine() ?? "0");

                        OrderItem orderItem = new()
                        {
                            ProductId = selectedProduct.Id,
                            ProductName = selectedProduct.Name,
                            AmountOrdered = amountOrdered
                        };

                        //OrderItem orderItem = new OrderItem();
                        //orderItem.ProductId = selectedProduct.Id;
                        //orderItem.ProductName = selectedProduct.Name;
                        //orderItem.AmountOrdered = amountOrdered;

                        newOrder.OrderItems.Add(orderItem);
                    }
                }

            } while (selectedProductId != "0");

            Console.WriteLine("Creating order...");

            orders.Add(newOrder);

            Console.WriteLine("Order now created.");
            Console.ReadLine();
        }

        private static void ShowOpenOrderOverview()
        {
            //check to handle fulfilled orders
            ShowFulfilledOrders();

            if (orders.Count > 0)
            {
                Console.WriteLine("Open orders:");

                foreach (var order in orders)
                {
                    Console.WriteLine(order.ShowOrderDetails());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("There are no open orders");
            }

            Console.ReadLine();
        }

        private static void ShowInventoryManagementMenu()
        {
            string? userSelection;

            do
            {
                Console.ResetColor();
                Console.Clear();
                Console.WriteLine("************************");
                Console.WriteLine("* Inventory management *");
                Console.WriteLine("************************");

                ShowAllProductsOverview();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("What do you want to do?");
                Console.ResetColor();

                Console.WriteLine("1: View details of product");
                Console.WriteLine("2: Add new product");
                Console.WriteLine("3: Clone product");
                Console.WriteLine("4: View products with low stock");
                Console.WriteLine("0: Back to main menu");

                Console.Write("Your selection: ");

                userSelection = Console.ReadLine();

                switch (userSelection)
                {
                    case "1":
                        ShowDetailsAndUseProduct();
                        break;

                    case "2":
                        ShowCreateNewProduct();
                        break;

                    case "3":
                        ShowCloneExistingProduct();
                        break;

                    case "4":
                        ShowProductsLowOnStock();
                        break;

                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            while (userSelection != "0");
            ShowMainMenu();
        }

        private static void ShowCloneExistingProduct()
        {
            string? userSelection = string.Empty;
            string? newId = string.Empty;

            Console.WriteLine("Enter the Id of product to clone: ");

            userSelection = Console.ReadLine();

            if (userSelection != null)
            {

                Product? selectedProduct = inventory.Where(p => p.Id == int.Parse(userSelection)).FirstOrDefault();

                if (selectedProduct != null)
                {
                    Console.WriteLine("Enter the new ID of the cloned product: ");

                    newId = Console.ReadLine();

                    Product? p = selectedProduct.Clone() as Product;

                    if (p != null)
                    {
                        p.Id = int.Parse(newId);
                        inventory.Add(p);
                    }
                }
            }
        }

        private static void ShowCreateNewProduct()
        {

            UnitType unitType = UnitType.PerItem;

            Console.WriteLine("What type of product do you want to create?");
            Console.WriteLine("1. Regular product\n2. Bulk product\n3. Fresk product\n4. Boxed product");
            Console.WriteLine("Your selection: ");


            if (!int.TryParse(Console.ReadLine(), out int productType))
            {

                Console.WriteLine("Invalid selection");
                return;

            }

            if (!IsExistentProductType(productType))
            {

                Console.WriteLine("Invalid Product");
                return;

            }

            Product? newProduct = null;

            Console.Write("Enter the name of the product: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the price of the product: ");
            double price = double.Parse(Console.ReadLine() ?? "0.0");

            ShowAllCurrencies();
            Console.WriteLine("Select the currecy: ");
            Currency currency = (Currency)Enum.Parse(typeof(Currency), Console.ReadLine() ?? "1");

            Console.WriteLine("Enter the description: ");
            string description = Console.ReadLine() ?? string.Empty;

            ShowAllUnitTypes();
            Console.WriteLine("Select the unit type: ");
            unitType = (UnitType)Enum.Parse(typeof(UnitType), Console.ReadLine() ?? "1");

            //if (productType == 1)
            //{
            //    ShowAllUnitTypes();
            //    Console.WriteLine("Select the unit type: ");
            //    unitType = (UnitType)Enum.Parse(typeof(UnitType), Console.ReadLine() ?? "1");
            //}

            Console.WriteLine("Enter the number of items in stock for this product: ");
            int itemInStock = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("Enter the namber as the Max amount for the product: ");
            int maxAmountInStock = int.Parse(Console.ReadLine() ?? "0");

            var newId = 0;

            if (!inventory.IsNullOrEmpty())
            {
                newId = inventory.Max(p => p.Id) + 1;
            }

            switch (productType)
            {
                case 1:
                    newProduct = new RegularProduct(newId++, name, description, new Price() { ItemPrice = price, Currency = currency }, unitType, itemInStock, maxAmountInStock);
                    break;
                case 2:
                    newProduct = new BulkProduct(newId++, name, description, new Price() { ItemPrice = price, Currency = currency }, unitType, itemInStock, maxAmountInStock);
                    break;
                case 3:
                    newProduct = new FreshProduct(newId++, name, description, new Price() { ItemPrice = price, Currency = currency }, unitType, itemInStock, maxAmountInStock);
                    break;
                case 4:
                    Console.WriteLine("Enter the number of items per box: ");
                    int numberInBox = int.Parse(Console.ReadLine() ?? string.Empty);

                    newProduct = new BoxedProduct(newId++, name, description, new Price() { ItemPrice = price, Currency = currency }, itemInStock, numberInBox, maxAmountInStock);
                    break;
            }
            if (newProduct != null)
            {
                productDbRepository.AddProduct(newProduct);
                //inventory.Add(newProduct);
            }
        }

        private static bool IsExistentProductType(int productType)
        {
            if (productType >= 1 && productType <= 4)
                return true;

            return false;
        }

        private static void ShowAllUnitTypes()
        {
            int i = 1;

            foreach (string name in Enum.GetNames(typeof(UnitType)))
            {
                Console.WriteLine($"{i}. {name}");
                i++;
            }
        }

        private static void ShowAllCurrencies()
        {
            int i = 1;

            foreach (string name in Enum.GetNames(typeof(Currency)))
            {
                Console.WriteLine($"{i}. {name}");
                i++;
            }
        }

        private static void ShowProductsLowOnStock()
        {
            List<Product> lowOnStockProducts = inventory.Where(p => p.IsBelowStockThreshold).ToList();

            if (lowOnStockProducts.Count > 0)
            {
                Console.WriteLine("The following items are low on stock, order more soon!");

                foreach (var product in lowOnStockProducts)
                {
                    Console.WriteLine(product.DisplayDetailsShort());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No items are currently low on stock!");
            }

            Console.ReadLine();
        }

        private static void ShowFulfilledOrders()
        {
            Console.WriteLine("Checking fulfilled orders.");

            foreach (var order in orders)
            {
                if (!order.Fulfilled && order.OrderFulfillmentDate < DateTime.Now)
                {
                    foreach (var orderItem in order.OrderItems)
                    {
                        Product? selectedProduct = inventory.Where(p => p.Id == orderItem.ProductId).FirstOrDefault();

                        selectedProduct?.IncreaseStock(orderItem.AmountOrdered);
                    }

                    order.Fulfilled = true;
                }
            }

            orders.RemoveAll(o => o.Fulfilled);

            Console.WriteLine("Fulfilled orders checked");
        }

        private static void ShowDetailsAndUseProduct()
        {
            Console.WriteLine("Enter de ID of the product");

            if (int.TryParse(Console.ReadLine(), out int selectedProductId))
            {

                Product? selectedProduct = productDbRepository.GetProductById(selectedProductId);
                // inventory.Where(p => p.Id == int.Parse(selectedProductId)).FirstOrDefault();

                if (selectedProduct != null)
                {
                    Console.Clear();

                    Console.WriteLine(selectedProduct.DisplayDetailsFull());

                    Console.WriteLine("\nWhat do you want to do?");
                    Console.WriteLine("1: Use product");
                    Console.WriteLine("0: Back to inventory overview");

                    Console.Write("Your selection: ");
                    string? userSelection = Console.ReadLine();

                    if (userSelection == "1")
                    {
                        Console.WriteLine("How many products do you want to use?");
                        int amount = int.Parse(Console.ReadLine() ?? "0");

                        selectedProduct.UseProduct(amount);

                        productDbRepository.UpdateProduct(selectedProduct);

                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Product not found.");
                    Console.WriteLine("Press enter to continue.");
                    Console.ReadLine();
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Non-existing product selected. Please try again.");
            }

        }
        private static void ShowAllProductsOverview()
        {
            inventory.Clear();

            inventory = productDbRepository.GetAllProducts();

            foreach (var product in inventory)
            {
                Console.WriteLine(product.DisplayDetailsShort());
                Console.WriteLine();
            }
        }

        private static IConfiguration ConfigurationBuilder()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            string? rootPath = Directory.GetParent(currentPath)?.Parent?.Parent?.Parent?.FullName;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(rootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
