namespace DelightBistroMinimalApi.Services.Cache.Options
{
    /// <summary>
    /// Настройки application-кэша (Memory или Redis).
    /// </summary>
    public class CachingOptions
    {
        public const string SectionName = "Caching";

        public const string ProviderMemory = "Memory";
        public const string ProviderRedis = "Redis";

        /// <summary>
        /// Провайдер: Memory или Redis.
        /// </summary>
        public string Provider { get; set; } = ProviderMemory;

        /// <summary>
        /// Префикс ключей в Redis, чтобы не пересекаться с другими приложениями.
        /// </summary>
        public string InstanceName { get; set; } = "DelightBistro_";

        public bool UseRedis =>
            string.Equals(Provider, ProviderRedis, StringComparison.OrdinalIgnoreCase);
    }
}
