using Ardalis.GuardClauses;
using CMS.Helpers;
using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Caching;
using Sandbox.Data.Kentico.Infrastructure.Queries;

namespace Sandbox.Delivery.Web.Infrastructure.Caching
{
    public class QueryHandlerSyncCacheDecorator<TQuery, TResponse> : IQueryHandlerSync<TQuery, TResponse>
        where TQuery : IQuerySync<TResponse>
    {
        private readonly IOutputCacheDependenciesStore cacheKeyStore;
        private readonly ICacheHelper cacheHelper;
        private readonly QueryCacheSettings queryCacheSettings;
        private readonly IQueryHandlerSync<TQuery, TResponse> handler;
        private readonly IQuerySyncCacheKeysCreator<TQuery, TResponse> cacheKeysCreator;

        public QueryHandlerSyncCacheDecorator(
            IOutputCacheDependenciesStore cacheKeyStore,
            ICacheHelper cacheHelper,
            QueryCacheSettings queryCacheSettings,
            IQueryHandlerSync<TQuery, TResponse> handler,
            IQuerySyncCacheKeysCreator<TQuery, TResponse> cacheKeysCreator)
        {
            Guard.Against.Null(cacheKeyStore, nameof(cacheKeyStore));
            Guard.Against.Null(queryCacheSettings, nameof(queryCacheSettings));
            Guard.Against.Null(handler, nameof(handler));
            Guard.Against.Null(cacheKeysCreator, nameof(cacheKeysCreator));
            Guard.Against.Null(cacheHelper, nameof(cacheHelper));

            this.cacheKeyStore = cacheKeyStore;
            this.cacheHelper = cacheHelper;
            this.queryCacheSettings = queryCacheSettings;
            this.handler = handler;
            this.cacheKeysCreator = cacheKeysCreator;
        }

        public Result<TResponse> Execute(TQuery query)
        {
            if (!queryCacheSettings.IsEnabled)
            {
                return handler.Execute(query);
            }

            var cacheSettings = new CacheSettings(
                cacheMinutes: queryCacheSettings.CacheItemDuration.Minutes,
                useSlidingExpiration: true,
                cacheItemNameParts: cacheKeysCreator.ItemNameParts(query));

            return cacheHelper.Cache(
                 (cs) =>
                 {
                     var result = handler.Execute(query);

                     if (cs.Cached)
                     {
                         cs.GetCacheDependency = () =>
                         {
                             string[] cacheKeys = cacheKeysCreator.DependencyKeys(query, result);

                             cacheKeyStore.AddDependencyOnKeys(cacheKeys);

                             return cacheHelper.GetCacheDependency(cacheKeys);
                         };
                     }

                     return result;
                 },
                 cacheSettings);
        }
    }
}
