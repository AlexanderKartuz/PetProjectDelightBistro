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

            var startTime = DateTime.UtcNow;

            context.Response.Headers["X-Request-Id"] = requestId;
            context.Response.Headers.XContentTypeOptions = "nosniff";

            await _next(context);

            var duration = DateTime.UtcNow - startTime;
            var statusCode = context.Response.StatusCode;



            _logger.LogInformation("RequestId={RequestId} | Duration={Duration}ms | Status={Status}", requestId, duration, statusCode);

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

