using DelightBistro.Services.Logging;

namespace DelightBistroMinimalApi.Middlewares
{
    public class ResponseHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAppLogging<ResponseHeaderMiddleware> appLogging)
        {
            var requestId = Guid.NewGuid().ToString("N")[..12];
            context.Items["RequestId"] = requestId;

            context.Response.Headers["X-Request-Id"] = requestId;
            context.Response.Headers.XContentTypeOptions = "nosniff";

            context.Response.OnStarting(() =>
            {
                if (context.Request.Method == "GET" && context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    context.Response.Headers["Cache-Control"] = "public, max-age=10";
                }

                return Task.CompletedTask;
            });

            await _next(context);

            appLogging.LogAppInformation(
                "RequestId={RequestId} | Status={Status}",
                new object?[] { requestId, context.Response.StatusCode });
        }
    }

    public static class ResponseHeaderMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseHeaderMiddleware>();
        }
    }
}

