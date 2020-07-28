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
        private readonly ICacheHelper cacheHelper;
        private readonly QueryCacheSettings queryCacheSettings;
        private readonly IQueryHandlerSync<TQuery, TResponse> handler;

        public QueryHandlerSyncCacheDecorator(
            ICacheHelper cacheHelper,
            QueryCacheSettings queryCacheSettings,
            IQueryHandlerSync<TQuery, TResponse> handler)
        {
            Guard.Against.Null(queryCacheSettings, nameof(queryCacheSettings));
            Guard.Against.Null(handler, nameof(handler));
            Guard.Against.Null(cacheHelper, nameof(cacheHelper));

            this.cacheHelper = cacheHelper;
            this.queryCacheSettings = queryCacheSettings;
            this.handler = handler;
        }

        public Result<TResponse> Execute(TQuery query)
        {
            if (!queryCacheSettings.IsEnabled)
            {
                return handler.Execute(query);
            }

            if (!(handler is IQuerySyncCacheKeysCreator<TQuery, TResponse> cacheKeysCreator))
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
                            cacheHelper.GetCacheDependency(cacheKeysCreator.DependencyKeys(query, result));
                     }

                     return result;
                 },
                 cacheSettings);
        }
    }
}
