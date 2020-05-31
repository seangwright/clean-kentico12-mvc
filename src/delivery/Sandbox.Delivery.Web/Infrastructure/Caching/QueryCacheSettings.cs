using System;

namespace Sandbox.Delivery.Web.Infrastructure.Caching
{
    public class QueryCacheSettings
    {
        public QueryCacheSettings(
            bool isEnabled,
            TimeSpan cacheItemDuration)
        {
            IsEnabled = isEnabled;
            CacheItemDuration = cacheItemDuration;
        }

        public bool IsEnabled { get; }
        public TimeSpan CacheItemDuration { get; }
    }
}
