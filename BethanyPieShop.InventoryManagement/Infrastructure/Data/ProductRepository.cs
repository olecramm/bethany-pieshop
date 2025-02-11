using BethanyPieShop.InventoryManagement.Domain.Contracts;
using BethanyPieShop.InventoryManagement.Domain.ProductManagement;
using BethanyPieShop.InventoryManagement.Domain.General;
using System.Text;

namespace BethanyPieShop.InventoryManagement.Infrastructure.Data
{
    internal class ProductRepository
    {
        private readonly string directory = @"C:\iecourses\";
        private readonly string productFileName = "products.txt";

        private void CheckForExistingProductFile()
        {
            string path = $"{directory}{productFileName}";

            bool existingFileFound = File.Exists(path);

            if (!existingFileFound)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(directory);

                using FileStream fs = File.Create(path);
            }
        }

        public List<Product> LoadProductFromFile(Product? product)
        {

            List<Product> products = [];

            string path = Path.Combine(directory, productFileName);

            try
            {
                CheckForExistingProductFile();

                string[] producAsString = File.ReadAllLines(path);

                for (int i = 0; i < producAsString.Length; i++)
                {
                    string[] productSplits = producAsString[i].Split(";");

                    bool success = int.TryParse(productSplits[0], out int productId);
                    if (!success)
                    {
                        productId = 0;
                    }

                    string name = productSplits[1];
                    string descreption = productSplits[2];

                    success = int.TryParse(productSplits[3], out int amountInStock);

                    if (!success)
                    {
                        amountInStock = 100;
                    }

                    success = int.TryParse(productSplits[4], out int itemPrice);

                    if (!success)
                    {
                        itemPrice = 0;
                    }

                    success = Enum.TryParse(productSplits[5], out Currency currency);

                    if (!success)
                    {
                        currency = Currency.Dollar;
                    }

                    success = Enum.TryParse(productSplits[6], out UnitType unitType);

                    if (!success)
                    {
                        unitType = UnitType.PerItem;
                    }

                    string productType = productSplits[7];

                    Product? currentProduct = null;

                    switch (productType)
                    {
                        case "1":
                            success = int.TryParse(productSplits[8], out int amountPerBox);
                            if (!success)
                            {
                                amountPerBox = 1;
                            }

                            currentProduct = new BoxedProduct(productId, name, descreption, new Price() { ItemPrice = itemPrice, Currency = currency }, amountInStock, amountPerBox, 0);
                            break;

                        case "2":
                            currentProduct = new FreshProduct(productId, name, descreption, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, amountInStock, 0);
                            break;

                        case "3":
                            currentProduct = new BulkProduct(productId, name, descreption, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, amountInStock, 0);
                            break;

                        case "4":
                            currentProduct = new RegularProduct(productId, name, descreption, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, amountInStock, 0);
                            break;

                    }

                    //Product product = new Product(productId, name, descreption, new Price() { ItemPrice = itemPrice, Currency = currency }, unitType, maxItemInStock);

                    products.Add(currentProduct);
                }
            }
            catch (IndexOutOfRangeException iex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong parsinfg the file, please check the data!");
                Console.WriteLine(iex.Message);
            }
            catch (FileNotFoundException fnfex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The file couldn't be found!");
                Console.WriteLine(fnfex.Message);
                Console.WriteLine(fnfex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong while loading the file!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            return products;
        }

        public void SaveToFile(List<ISavable> savables)
        {
            StringBuilder sb = new();
            string path = $"{directory}{productFileName}";

            foreach (var item in savables)
            {
                sb.Append(item.ConvertToStringForSaving());
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(path, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Save items successfully");
            Console.ResetColor();
        }
    }
}
