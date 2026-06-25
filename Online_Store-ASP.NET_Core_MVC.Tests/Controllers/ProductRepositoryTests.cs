using Microsoft.EntityFrameworkCore;
using Online_Store_ASP.NET_Core_MVC.Models;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly DbContextProject _context;
        private readonly ProductsController _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DbContextProject>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DbContextProject(options);
            _repository = new ProductsController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Add_AddsProductToContext()
        {
            var product = new Product
            {
                Name = "Test Product",
                Price = 100,
                Quantity = 10
            };

            _repository.Add(product);
            await _repository.SaveChangesAsync();

            Assert.Single(_context.Product);
            var savedProduct = await _context.Product.FirstAsync();
            Assert.Equal("Test Product", savedProduct.Name);
            Assert.Equal(100, savedProduct.Price);
            Assert.Equal(10, savedProduct.Quantity);
        }

        [Fact]
        public async Task SaveChangesAsync_PersistsChanges()
        {
            var product = new Product
            {
                Name = "Persisted Product",
                Price = 200,
                Quantity = 5
            };

            _repository.Add(product);
            await _repository.SaveChangesAsync();

            var count = await _context.Product.CountAsync();
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Findable Product",
                Price = 300,
                Quantity = 2
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Findable Product", result!.Name);
            Assert.Equal(300, result.Price);
        }

        [Fact]
        public async Task GetByIdAsync_WhenProductDoesNotExist_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Add_MultipleProducts_AllPersisted()
        {
            var product1 = new Product { Name = "Product 1", Price = 100, Quantity = 1 };
            var product2 = new Product { Name = "Product 2", Price = 200, Quantity = 2 };
            var product3 = new Product { Name = "Product 3", Price = 300, Quantity = 3 };

            _repository.Add(product1);
            _repository.Add(product2);
            _repository.Add(product3);
            await _repository.SaveChangesAsync();

            Assert.Equal(3, await _context.Product.CountAsync());
        }
    }
}
