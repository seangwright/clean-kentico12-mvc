using System;
using CMS.Helpers;

namespace Sandbox.Data.Kentico.Infrastructure.Caching
{
    /// <summary>
    /// A wrapper around <see cref="CacheHelper"/> for testability
    /// </summary>
    public interface ICacheHelper
    {
        void EnsureDummyKey(string key);

        TData Cache<TData>(Func<TData> loadMethod, CacheSettings settings);
        TData Cache<TData>(Func<CacheSettings, TData> loadMethod, CacheSettings settings);
        bool TryGetItem<OutputType>(string key, out OutputType value);

        CMSCacheDependency GetCacheDependency(string[] keys);
    }

    public class KenticoCacheHelper : ICacheHelper
    {
        public TData Cache<TData>(Func<TData> loadMethod, CacheSettings settings) =>
            CacheHelper.Cache(loadMethod, settings);

        public TData Cache<TData>(Func<CacheSettings, TData> loadMethod, CacheSettings settings) =>
            CacheHelper.Cache(loadMethod, settings);

        public bool TryGetItem<OutputType>(string key, out OutputType value) =>
            CacheHelper.TryGetItem(key, out value);

        public void EnsureDummyKey(string key) =>
            CacheHelper.EnsureDummyKey(key);

        public CMSCacheDependency GetCacheDependency(string[] keys) =>
            CacheHelper.GetCacheDependency(keys);
    }
}
