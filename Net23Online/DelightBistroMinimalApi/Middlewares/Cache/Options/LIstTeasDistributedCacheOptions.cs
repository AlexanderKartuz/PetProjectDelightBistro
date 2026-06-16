namespace DelightBistroMinimalApi.Middlewares.Cache.Options
{
    public class ListTeasDistributedCacheOptions : IDistributedCacheOptions
    {
        public const string SectionName = "ListTeasDistributedCacheOptions";

        public int AbsoluteMinutes { get; set; } = 5;
        public int SlidingMinutes { get; set; } = 2;
    }
}
