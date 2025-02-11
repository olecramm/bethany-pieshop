using Microsoft.Extensions.Configuration;

namespace BethanyPieShop.InventoryManagement
{
    public static class Configuration
    {
        public static IConfiguration Build()
        {

            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            string? rootPath = Directory.GetParent(currentPath)?.Parent?.Parent?.Parent?.FullName;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(rootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return configuration.Build();
        }
    }
}
