using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BethanyPieShop.InventoryManagement.db
{
    public interface IRepository<T>
    {
        void AddProduct(T entity);
        List<T> GetAllProducts();
        T GetProductById(int id);

    }
}
