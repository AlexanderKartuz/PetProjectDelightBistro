using DelightBistro.Services.Logging;

namespace DelightBistroMinimalApi.Middlewares
{
    public class CustomRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAppLogging<CustomRequestLoggingMiddleware> appLogging)
        {
            var startTime = DateTime.UtcNow;
            appLogging.LogAppInformation(
                "{Method} {Path}. Start, time: {StartTime}",
                new object?[]
                {
                    context.Request.Method,
                    context.Request.Path.ToString(),
                    startTime
                });

            await _next(context);

            var durationTime = DateTime.UtcNow - startTime;
            appLogging.LogAppInformation(
                "{Method} {Path}. End, duration time: {DurationTime} ms, status: {Status}",
                new object?[]
                {
                    context.Request.Method,
                    context.Request.Path.ToString(),
                    durationTime.TotalMilliseconds,
                    context.Response.StatusCode
                });
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
