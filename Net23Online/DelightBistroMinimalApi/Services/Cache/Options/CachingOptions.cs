namespace DelightBistroMinimalApi.Services.Cache.Options
{

    public class CachingOptions
    {
        public const string SectionName = "Caching";

        public const string ProviderMemory = "Memory";
        public const string ProviderRedis = "Redis";

        public string Provider { get; set; } = ProviderMemory;
        public string InstanceName { get; set; } = "DelightBistro_";

        public bool UseRedis =>
            string.Equals(Provider, ProviderRedis, StringComparison.OrdinalIgnoreCase);
    }
}
