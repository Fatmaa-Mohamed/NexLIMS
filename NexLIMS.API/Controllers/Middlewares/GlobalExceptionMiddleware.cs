using System.Net;
using System.Text.Json;

namespace NexLIMS.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next   = next;
            _logger = logger;
            _env    = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await WriteErrorResponse(context, ex);
            }
        }

        private async Task WriteErrorResponse(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)HttpStatusCode.InternalServerError;

            var message = _env.IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred. Please try again later.";

            var body = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(body);
        }
    }
}
