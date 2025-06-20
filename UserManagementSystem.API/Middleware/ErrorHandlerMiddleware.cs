using System.Net;

namespace UserManagementSystem.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (!context.Response.HasStarted &&
                    (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                     context.Response.StatusCode == (int)HttpStatusCode.Forbidden))
                {
                    context.Response.ContentType = "application/json";
                    var responseBody = new
                    {
                        Title = context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ? "Unauthorized" : "Forbidden",
                        Status = context.Response.StatusCode,
                        Detail = context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ?
                                 "Authentication credentials were not provided or are invalid." :
                                 "You do not have the necessary permissions to access this resource."
                    };
                    await context.Response.WriteAsJsonAsync(responseBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var responseBody = new
                {
                    Title = "Internal Server Error",
                    Status = context.Response.StatusCode,
                    Detail = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(responseBody);
            }
        }
    }
}
