using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Online_Store_ASP.NET_Core_MVC.Controllers;
using Online_Store_ASP.NET_Core_MVC.Models;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class BasketControllerTests : IDisposable
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly DbContextProject _context;
        private readonly BasketController _controller;

        public BasketControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var options = new DbContextOptionsBuilder<DbContextProject>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new DbContextProject(options);

            _controller = new BasketController(_userManagerMock.Object, _context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SetupUserContext(string userId, string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task AddToBasket_WhenProductNotFound_ReturnsNotFound()
        {
            var user = new IdentityUser { Id = "user-1", UserName = "testuser", Email = "test@test.com" };
            SetupUserContext("user-1", "testuser");

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var result = await _controller.AddToBasket(999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product Not Found ! 404 ", notFoundResult.Value);
        }

        [Fact]
        public async Task AddToBasket_WhenProductExistsAndNoBasket_CreatesNewBasket()
        {
            var user = new IdentityUser { Id = "user-1", UserName = "testuser", Email = "test@test.com" };
            SetupUserContext("user-1", "testuser");

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                Price = 100,
                Quantity = 5,
                IdBasket = null!
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            var result = await _controller.AddToBasket(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfull", okResult.Value);
            Assert.Single(_context.Basket);

            var basket = await _context.Basket.FirstAsync();
            Assert.Equal(1, basket.BasketId);
            Assert.Equal("user-1", basket.UserId);
            Assert.Equal(1, basket.Counter);
        }

        [Fact]
        public async Task AddToBasket_WhenProductExistsAndBasketExists_IncrementsCounter()
        {
            var user = new IdentityUser { Id = "user-1", UserName = "testuser", Email = "test@test.com" };
            SetupUserContext("user-1", "testuser");

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var existingBasket = new Basket { BasketId = 1, UserId = "user-1", Counter = 2 };
            _context.Basket.Add(existingBasket);

            var product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                Price = 100,
                Quantity = 5,
                IdBasket = new List<Basket> { existingBasket }
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            var result = await _controller.AddToBasket(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfull", okResult.Value);

            var basket = await _context.Basket.FirstAsync(b => b.BasketId == 1 && b.UserId == "user-1");
            Assert.Equal(3, basket.Counter);
        }
    }
}
