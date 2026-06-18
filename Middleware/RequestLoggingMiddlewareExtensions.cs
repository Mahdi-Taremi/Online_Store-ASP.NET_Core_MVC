namespace Online_Store_ASP.NET_Core_MVC.Middleware
{
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder
            UseRequestLogging(
                this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
