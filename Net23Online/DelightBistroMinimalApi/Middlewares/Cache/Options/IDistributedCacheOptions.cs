namespace DelightBistroMinimalApi.Middlewares.Cache.Options
{
    public interface IDistributedCacheOptions
    {
        int AbsoluteMinutes { get; set; }
        int SlidingMinutes { get; set; }
    }
}