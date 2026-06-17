using DelightBistroMinimalApi.ModelsDto;

namespace DelightBistroMinimalApi.Middlewares
{
    public class CustomExeptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExeptionHandlingMiddleware> _logger;

        public CustomExeptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<CustomExeptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Exception in {Method} {Path}:{Message} ",
                    context.Request.Method,
                    context.Request.Path,
                    ex.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ApiErrorResponse("Внутренняя ошибка сервера", 500, ex.Message);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    public static class CustomExeptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExeptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExeptionHandlingMiddleware>();
        }
    }
}
