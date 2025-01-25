using Microsoft.AspNetCore.Diagnostics;

namespace WebMarket.ServerExceptions
{
    public class GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, $"[{DateTime.UtcNow} UTC]: {exception.Message}");
            var webException = new WebExceptionInfo(exception, httpContext, env.IsDevelopment());
            const string contentType = "application/problem+json";
            httpContext.Response.ContentType = contentType;
            await httpContext.Response.WriteAsync(webException.ToJson(), cancellationToken);
            return true;
        }
    }
}
