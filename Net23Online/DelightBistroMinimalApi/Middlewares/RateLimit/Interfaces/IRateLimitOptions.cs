using System.Threading.RateLimiting;

namespace DelightBistroMinimalApi.Middlewares.RateLimit.Interfaces
{
    public interface IRateLimitOptions
    {
        int PermitLimit { get; set; }
        int QueueLimit { get; set; }
        int SegmentsPerWindow { get; set; }
        int WindowSeconds { get; set; }
        QueueProcessingOrder QueueOrder { get; set; }
    }
}