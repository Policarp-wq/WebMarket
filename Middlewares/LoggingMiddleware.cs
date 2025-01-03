using WebMarket.Controllers;
using WebMarket.Models;

namespace WebMarket.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyController<DbEntry>> _logger;

        public LoggingMiddleware(ILogger<MyController<DbEntry>> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"[{DateTime.UtcNow}]: Handling request {context.Request.Method} {context.Request.Path}");
            await _next(context);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Responsed with code {context.Response.StatusCode}");
        }
    }
}
