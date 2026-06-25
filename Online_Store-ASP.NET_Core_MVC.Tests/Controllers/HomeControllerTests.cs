using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Online_Store_ASP.NET_Core_MVC.Controllers;
using Online_Store_ASP.NET_Core_MVC.Models;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var result = _controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = _controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
        }

        [Fact]
        public void Error_HasResponseCacheAttribute()
        {
            var method = typeof(HomeController).GetMethod("Error");
            var attribute = method!.GetCustomAttributes(typeof(ResponseCacheAttribute), false)
                .FirstOrDefault() as ResponseCacheAttribute;

            Assert.NotNull(attribute);
            Assert.Equal(0, attribute!.Duration);
            Assert.Equal(ResponseCacheLocation.None, attribute.Location);
            Assert.True(attribute.NoStore);
        }
    }
}
