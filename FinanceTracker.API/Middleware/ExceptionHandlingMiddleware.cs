using System.Net;
using System.Text.Json;

namespace FinanceTracker.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {               
                await _next(context);
            }
            catch (Exception ex)
            {              
                _logger.LogError(ex, "Beklenmeyen hata oluştu: {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new { message = "Sunucuda bir hata oluştu." };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}