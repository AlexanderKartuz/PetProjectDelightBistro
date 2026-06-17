namespace DelightBistroMinimalApi.Middlewares
{
    public class ResponseHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseHeaderMiddleware> _logger;

        public ResponseHeaderMiddleware(RequestDelegate next, ILogger<ResponseHeaderMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString("N")[..12];
            context.Items["RequestId"] = requestId;

            context.Response.Headers["X-Request-Id"] = requestId;
            context.Response.Headers.XContentTypeOptions = "nosniff";

            
            context.Response.OnStarting(() =>
            {
                //endpoint уже отработал
                if (context.Request.Method == "GET" && context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    context.Response.Headers["Cache-Control"] = "public, max-age=10"; //10 sec
                }

                return Task.CompletedTask;
            });

            await _next(context);

            _logger.LogInformation("RequestId={RequestId} | Status={Status}", requestId, context.Response.StatusCode);

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

