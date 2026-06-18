using System.Diagnostics;

namespace Online_Store_ASP.NET_Core_MVC.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware>
    _logger;

        public RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
       

        public async Task InvokeAsync(
            HttpContext context)
        {
            //Console.WriteLine("Middleware Executed");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                _logger.LogInformation(
                 "START => {Method} {Path}",
                 context.Request.Method,
                 context.Request.Path);

                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "END => {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                _logger.LogInformation(
                    "STATUS => {StatusCode}",
                    context.Response.StatusCode);

                _logger.LogInformation(
                    "DURATION => {Elapsed} ms",
                    stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                ex,
                "ERROR => {Message}",
                ex.Message);

                throw;
            }
            //throw new Exception("Middleware Test");

        }
    }
}
