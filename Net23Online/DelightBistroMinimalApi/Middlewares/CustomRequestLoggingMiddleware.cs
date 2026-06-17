namespace DelightBistroMinimalApi.Middlewares
{
    public class CustomRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomRequestLoggingMiddleware> _logger;

        public CustomRequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<CustomRequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            _logger.LogInformation("{Method} {Path}. Start, time: {StartTime}",
                context.Request.Method,
                context.Request.Path,
                startTime);

            await _next(context);

            var durationTime = DateTime.UtcNow - startTime;
            _logger.LogInformation("{Method} {Path}. End, duration time: {DurationTime} ms, status: {Status}",
                context.Request.Method,
                context.Request.Path,
                durationTime.TotalMilliseconds,
                context.Response.StatusCode);
        }
    }
    public static class CustomRequestLoggingMiddlewareExtansions
    {
        public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomRequestLoggingMiddleware>();
        }
    }
}
