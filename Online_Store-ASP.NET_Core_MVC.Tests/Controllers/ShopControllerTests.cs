using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Online_Store_ASP.NET_Core_MVC.Controllers;
using Online_Store_ASP.NET_Core_MVC.Models;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class ShopControllerTests : IDisposable
    {
        private readonly DbContextProject _context;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<ILogger<Online_Store_ASP.NET_Core_MVC.Controllers.ProductsController>> _loggerMock;
        private readonly ShopController _controller;

        public ShopControllerTests()
        {
            var options = new DbContextOptionsBuilder<DbContextProject>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DbContextProject(options);

            _cacheMock = new Mock<IDistributedCache>();
            _loggerMock = new Mock<ILogger<Online_Store_ASP.NET_Core_MVC.Controllers.ProductsController>>();

            _controller = new ShopController(_context, _cacheMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region CreateProduct Tests

        [Fact]
        public void CreateProduct_WhenValid_AddsProductAndReturnsOk()
        {
            System.IO.Directory.CreateDirectory("/tmp/wwwroot/Uploads");

            var product = new Product
            {
                Name = "TestProduct",
                Price = 200,
                Quantity = 5
            };
            var mockFile = CreateMockFormFile("product-image.png");
            product.UploadFile = mockFile;

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns("/tmp/wwwroot");

            var result = _controller.CreateProduct(product, envMock.Object);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Add Product", okResult.Value);
            Assert.Equal("product-image.png", product.pic_1);
            Assert.Single(_context.Product);

            // Cleanup
            if (System.IO.File.Exists("/tmp/wwwroot/Uploads/product-image.png"))
                System.IO.File.Delete("/tmp/wwwroot/Uploads/product-image.png");
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_WhenProductExists_RemovesAndReturnsOk()
        {
            var product = new Product { Id = 1, Name = "ToDelete", Price = 50, Quantity = 1 };
            _context.Product.Add(product);
            _context.SaveChanges();

            var result = _controller.Delete(1, new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Delete Product", okResult.Value);
            Assert.Empty(_context.Product);
        }

        #endregion

        #region Show Tests

        [Fact]
        public void Show_WhenProductExists_ReturnsProductInfo()
        {
            var product = new Product { Id = 10, Name = "Laptop", Price = 1500, Quantity = 3 };
            _context.Product.Add(product);
            _context.SaveChanges();

            var result = _controller.Show(10);

            Assert.Contains("Laptop", result);
            Assert.Contains("1500", result);
        }

        #endregion

        #region ShowWithoutRedis Tests

        [Fact]
        public async Task ShowWithoutRedis_WhenProductExists_ReturnsProductJson()
        {
            var product = new Product { Id = 20, Name = "Phone", Price = 800, Quantity = 10 };
            _context.Product.Add(product);
            _context.SaveChanges();

            var result = await _controller.ShowWithoutRedis(20);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.Contains("Phone", okResult.Value.ToString()!);
        }

        #endregion

        #region ShowWithRedis Tests

        [Fact]
        public async Task ShowWithRedis_WhenCacheHit_ReturnsProductFromCache()
        {
            var product = new Product { Id = 30, Name = "Tablet", Price = 500, Quantity = 7 };
            _context.Product.Add(product);
            _context.SaveChanges();

            _cacheMock.Setup(c => c.GetAsync("product:30", It.IsAny<CancellationToken>()))
                .ReturnsAsync(System.Text.Encoding.UTF8.GetBytes("\"cached-product-json\""));

            var result = await _controller.Get(30);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task ShowWithRedis_WhenCacheMiss_ReturnsProductAndCaches()
        {
            var product = new Product { Id = 40, Name = "Monitor", Price = 300, Quantity = 15 };
            _context.Product.Add(product);
            _context.SaveChanges();

            _cacheMock.Setup(c => c.GetAsync("product:40", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]?)null);

            var result = await _controller.Get(40);

            Assert.IsType<OkResult>(result);
            _cacheMock.Verify(c => c.SetAsync(
                "product:40",
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        private static IFormFile CreateMockFormFile(string fileName)
        {
            var content = "fake image content"u8.ToArray();
            var stream = new MemoryStream(content);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(content.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.CopyTo(It.IsAny<Stream>()))
                .Callback<Stream>(s => stream.CopyTo(s));
            return fileMock.Object;
        }
    }


}
