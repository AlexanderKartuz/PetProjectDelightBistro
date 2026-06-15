using DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces;
using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.RateLimit
{
    public static class RateLimitingExtensions
    {
        private const string PARTITIONEDLIMITINGKEY = "global";
        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services,
            IConfiguration configuration)
        {
            var globalOptions = new GlobalRateLimitOptions();
            configuration.GetSection(GlobalRateLimitOptions.SectionName).Bind(globalOptions);

            var ipOptions = new IpRateLimitOptions();
            configuration.GetSection(IpRateLimitOptions.SectionName).Bind(ipOptions);

            services.AddRateLimiter(opt =>
            {
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                opt.GlobalLimiter = PartitionedRateLimiter.CreateChained(

                    CreateIpPartionedLimiter(ipOptions),
                    CreatePartitionedLimiter(globalOptions, PARTITIONEDLIMITINGKEY)
                );
            });

            return services;
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

