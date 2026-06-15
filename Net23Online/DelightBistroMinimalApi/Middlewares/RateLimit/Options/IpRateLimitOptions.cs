using DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces;
using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.RateLimit.Options
{
    public class IpRateLimitOptions: IRateLimitOptions
    {
        public const string SectionName = "IpRateLimitingOptions";

        public int PermitLimit { get; set; } = 10;
        public int WindowSeconds { get; set; } = 30;
        public int SegmentsPerWindow { get; set; } = 3;
        public int QueueLimit { get; set; } = 0;
        public QueueProcessingOrder QueueOrder { get; set; } = QueueProcessingOrder.OldestFirst;

    }
}
