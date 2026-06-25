using Microsoft.AspNetCore.Mvc;
using Online_Store_ASP.NET_Core_MVC.Controllers;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class TestControllerTests
    {
        private readonly TestController _controller;

        public TestControllerTests()
        {
            _controller = new TestController();
        }

        [Fact]
        public async Task GetNumberAsync_Returns42()
        {
            var result = await _controller.GetNumberAsync();

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task Getinfo_ReturnsOkWithHello()
        {
            var result = await _controller.Getinfo();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Hello", okResult.Value);
        }

        [Fact]
        public void GetSerilog_ReturnsOk()
        {
            var result = _controller.GetSerilog();

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetSerilogWarning_ReturnsOk()
        {
            var result = _controller.GetSerilogWarning();

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetSerilogError_ReturnsOk()
        {
            var result = _controller.GetSerilogError();

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Get_ReturnsStringArray()
        {
            var result = _controller.Get();

            var values = result.ToList();
            Assert.Equal(2, values.Count);
            Assert.Equal("value MT 1 ", values[0]);
            Assert.Equal("value MT 2", values[1]);
        }

        [Fact]
        public void GetUser_ReturnsUserValues()
        {
            var result = _controller.GetUser();

            var values = result.ToList();
            Assert.Equal(2, values.Count);
            Assert.Equal("value USER 1 ", values[0]);
            Assert.Equal("value USER 2", values[1]);
        }

        [Fact]
        public void GetAdmin_ReturnsAdminValues()
        {
            var result = _controller.GetAdmin();

            var values = result.ToList();
            Assert.Equal(2, values.Count);
            Assert.Equal("value ADMIN 3 ", values[0]);
            Assert.Equal("value ADMIN 4", values[1]);
        }

        [Fact]
        public void GetOwner_ReturnsOwnerValues()
        {
            var result = _controller.GetOwner();

            var values = result.ToList();
            Assert.Equal(2, values.Count);
            Assert.Equal("value OWNER 5 ", values[0]);
            Assert.Equal("value OWNER 6", values[1]);
        }
    }
}
