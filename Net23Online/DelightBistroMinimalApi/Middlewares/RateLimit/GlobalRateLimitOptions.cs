using DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces;
using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.RateLimit
{
    public class GlobalRateLimitOptions : IRateLimitOptions
    {
        public const string SectionName = "GlobalRateLimitingOptions";

        public int PermitLimit { get; set; } = 100;
        public int WindowSeconds { get; set; } = 30;
        public int SegmentsPerWindow { get; set; } = 3;
        public int QueueLimit { get; set; } = 5;
        public QueueProcessingOrder QueueOrder { get; set; }= QueueProcessingOrder.OldestFirst;
    }
}
