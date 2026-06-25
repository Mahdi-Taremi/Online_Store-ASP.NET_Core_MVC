using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Online_Store_ASP.NET_Core_MVC.Middleware;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Middleware
{
    public class RequestLoggingMiddlewareTests
    {
        private readonly Mock<ILogger<RequestLoggingMiddleware>> _loggerMock;

        public RequestLoggingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
        }

        [Fact]
        public async Task InvokeAsync_LogsStartAndEndOfRequest()
        {
            var nextCalled = false;
            RequestDelegate next = (HttpContext ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/test";

            await middleware.InvokeAsync(context);

            Assert.True(nextCalled);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("START")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("END")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_LogsStatusCode()
        {
            RequestDelegate next = (HttpContext ctx) =>
            {
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Method = "POST";
            context.Request.Path = "/api/products";

            await middleware.InvokeAsync(context);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("STATUS")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_LogsDuration()
        {
            RequestDelegate next = (HttpContext ctx) => Task.CompletedTask;

            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/test";

            await middleware.InvokeAsync(context);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("DURATION")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_LogsErrorAndRethrows()
        {
            var expectedException = new InvalidOperationException("Test error");
            RequestDelegate next = (HttpContext ctx) => throw expectedException;

            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/api/failing";

            var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(
                () => middleware.InvokeAsync(context));

            Assert.Same(expectedException, thrownException);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("ERROR")),
                    expectedException,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_CallsNextMiddleware()
        {
            var callOrder = new List<string>();
            RequestDelegate next = (HttpContext ctx) =>
            {
                callOrder.Add("next");
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next, _loggerMock.Object);
            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/";

            await middleware.InvokeAsync(context);

            Assert.Contains("next", callOrder);
        }
    }
}
