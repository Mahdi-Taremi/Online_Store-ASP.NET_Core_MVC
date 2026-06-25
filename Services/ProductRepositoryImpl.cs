using Online_Store_ASP.NET_Core_MVC.Models;

namespace Online_Store_ASP.NET_Core_MVC.Services
{
    public class ProductRepositoryImpl : IProductRepository
    {
        private readonly DbContextProject _dbContext;

        public ProductRepositoryImpl(DbContextProject dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Product product)
        {
            _dbContext.Product.Add(product);
        }

        public void Remove(Product product)
        {
            _dbContext.Product.Remove(product);
        }

        public Product? GetById(int id)
        {
            return _dbContext.Product.SingleOrDefault(x => x.Id == id);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.Product.FindAsync(id);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
