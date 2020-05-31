using System.Collections.Generic;
using Ardalis.GuardClauses;
using Sandbox.Data.Kentico.Infrastructure.Caching;
using Sandbox.Delivery.Web.Infrastructure.Contexts;

namespace Sandbox.Delivery.Web.Infrastructure.Caching
{
    /// <summary>
    /// Represents a contract for objects that create a minimum set of ASP.NET output cache dependencies for views that contain data from pages or info objects.
    /// </summary>
    public interface IOutputCacheDependenciesStore
    {
        void AddDependencyOnKeys(params string[] cacheKeys);
    }

    /// <summary>
    /// Creates a minimum set of ASP.NET output cache dependencies for views that contain data from pages or info objects.
    /// </summary>
    public class OutputCacheDependenciesStore : IOutputCacheDependenciesStore
    {
        private readonly IHttpContextBaseAccessor httpContextAccessor;
        private readonly ICacheHelper cacheHelper;

        private readonly HashSet<string> dependencyCacheKeys = new HashSet<string>();

        public OutputCacheDependenciesStore(
            IHttpContextBaseAccessor httpContextAccessor,
            ICacheHelper cacheHelper)
        {
            Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            Guard.Against.Null(cacheHelper, nameof(cacheHelper));

            this.httpContextAccessor = httpContextAccessor;
            this.cacheHelper = cacheHelper;
        }

        public void AddDependencyOnKeys(params string[] cacheKeys)
        {
            foreach (string key in cacheKeys)
            {
                string lowerKey = key.ToLowerInvariant();

                if (dependencyCacheKeys.Contains(lowerKey))
                {
                    return;
                }

                dependencyCacheKeys.Add(lowerKey);

                cacheHelper.EnsureDummyKey(lowerKey);

                httpContextAccessor.HttpContextBase.Response.AddCacheItemDependency(lowerKey);
            }
        }
    }
}
