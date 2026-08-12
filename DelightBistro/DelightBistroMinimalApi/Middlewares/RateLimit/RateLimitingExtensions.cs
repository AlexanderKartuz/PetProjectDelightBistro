using DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces;
using DelightBistroMinimalApi.Middlewares.RateLimit.Options;
using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.RateLimit
{
    public static class RateLimitingExtensions
    {
        private const string PARTITIONED_LIMITING_KEY = "global";
        public static WebApplicationBuilder AddCustomRateLimiter(this WebApplicationBuilder builder)
        {
            var globalOptions = new GlobalRateLimitOptions();
            builder.Configuration.GetSection(GlobalRateLimitOptions.SectionName).Bind(globalOptions);

            var ipOptions = new IpRateLimitOptions();
            builder.Configuration.GetSection(IpRateLimitOptions.SectionName).Bind(ipOptions);

            builder.Services.AddRateLimiter(opt =>
            {
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                opt.GlobalLimiter = PartitionedRateLimiter.CreateChained(

                    CreateIpPartionedLimiter(ipOptions),
                    CreatePartitionedLimiter(globalOptions, PARTITIONED_LIMITING_KEY)
                );
            });

            return builder;
        }

        private static PartitionedRateLimiter<HttpContext> CreateIpPartionedLimiter(IRateLimitOptions options)
        {
            return PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetSlidingWindowLimiter(partitionKey: ip,
                    factory: _ => CreateSlidingOptions(options));
            });
        }

        private static PartitionedRateLimiter<HttpContext> CreatePartitionedLimiter(IRateLimitOptions options, string partitionKey)
        {
            return PartitionedRateLimiter.Create<HttpContext, string>(_ =>
            {
                return RateLimitPartition.GetSlidingWindowLimiter(partitionKey: partitionKey,
                    factory: _ => CreateSlidingOptions(options));
            });
        }

        private static SlidingWindowRateLimiterOptions CreateSlidingOptions(IRateLimitOptions options)
        {
            return new SlidingWindowRateLimiterOptions
            {
                PermitLimit = options.PermitLimit,
                Window = TimeSpan.FromSeconds(options.WindowSeconds),
                SegmentsPerWindow = options.SegmentsPerWindow,
                QueueProcessingOrder = options.QueueOrder,
                QueueLimit = options.QueueLimit,
            };

        }
    }
}

