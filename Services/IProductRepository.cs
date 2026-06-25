using Online_Store_ASP.NET_Core_MVC.Models;

namespace Online_Store_ASP.NET_Core_MVC.Services
{
    public interface IProductRepository
    {
        void Add(Product product);
        void Remove(Product product);
        Product? GetById(int id);
        Task<Product?> GetByIdAsync(int id);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
