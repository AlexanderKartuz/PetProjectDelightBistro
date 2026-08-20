using DelightBistro.Services.Logging;
using DelightBistroMinimalApi.ModelsDto;

namespace DelightBistroMinimalApi.Middlewares
{
    public class CustomExeptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<CustomExeptionHandlingMiddleware> _logger;

        public CustomExeptionHandlingMiddleware(
            RequestDelegate next
            /*ILogger<CustomExeptionHandlingMiddleware> logger*/)
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext context,
            IAppLogging<CustomExeptionHandlingMiddleware> appLogging)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    appLogging.LogAppCritical(ex, "Cannot write error response, already started");
                    throw;
                }

                appLogging.LogAppError(ex, "Exception in {Method} {Path}:{Message} ",
                    new object?[]
                    {
                    context.Request.Method,
                    context.Request.Path,
                    ex.Message
                    });

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
