namespace BethanyPieShop.InventoryManagement.Infrastructure.Data
{
    public interface IReadOnlyRepository<T>
    {
        List<T> GetAllProducts();
        T GetProductById(int id);
    }

    public interface IWriteRepository<T>
    {
        void AddProduct(T entity);
        void UpdateProduct(T entity);
    }

    public interface IDataRepository<T> : IReadOnlyRepository<T>, IWriteRepository<T>
    {
    }
}
