using System.Text.Json;

namespace LibraryManagement.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unexpected error occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "An unexpected error occurred",
                    statusCode = 500
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}