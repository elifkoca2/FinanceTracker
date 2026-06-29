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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Yetkisiz Erişim"),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Sunucu da bir hata oluştu.")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Beklenmeyen hata: {Path}", context.Request.Path);
            else 
                _logger.LogWarning("İşlem hatası: ({StatusCode}): {Message}", (int)statusCode, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode= (int)statusCode;

            var errorResponse = new {message};
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
    }
}