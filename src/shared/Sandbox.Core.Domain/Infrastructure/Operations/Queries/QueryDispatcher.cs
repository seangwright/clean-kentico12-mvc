using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Intrastructure.Operations.Factories;

/// <summary>
/// From https://github.com/jbogard/MediatR
/// </summary>
namespace Sandbox.Core.Domain.Intrastructure.Operations.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly ServiceProviderDelegate serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> queryHandlers = new ConcurrentDictionary<Type, object>();

        public QueryDispatcher(ServiceProviderDelegate serviceProvider)
        {
            Guard.Against.Null(serviceProvider, nameof(serviceProvider));

            this.serviceProvider = serviceProvider;
        }

        public Task<Result<TResponse>> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default)
        {
            Guard.Against.Null(query, nameof(query));

            var queryType = query.GetType();

            var handler = (QueryHandlerWrapper<TResponse>)queryHandlers
                .GetOrAdd(
                    queryType,
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));

            return handler.Dispatch(query, serviceProvider, token);
        }

        public Result<TResponse> Dispatch<TResponse>(IQuerySync<TResponse> query)
        {
            Guard.Against.Null(query, nameof(query));

            var queryType = query.GetType();

            var handler = (QueryHandlerSyncWrapper<TResponse>)queryHandlers
                .GetOrAdd(
                    queryType,
                    t => Activator
                        .CreateInstance(typeof(QueryHandlerSyncWrapperImpl<,>)
                        .MakeGenericType(queryType, typeof(TResponse))));

            return handler.Dispatch(query, serviceProvider);
        }
    }

    internal abstract class QueryHandlerWrapper<TResponse> : HandlerBase
    {
        public abstract Task<Result<TResponse>> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token);
    }

    internal class QueryHandlerWrapperImpl<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
        where TQuery : IQuery<TResponse>
    {
        public override Task<Result<TResponse>> Dispatch(IQuery<TResponse> query, ServiceProviderDelegate serviceProvider, CancellationToken token) =>
            GetHandler<IQueryHandler<TQuery, TResponse>>(serviceProvider).Execute((TQuery)query, token);
    }

    internal abstract class QueryHandlerSyncWrapper<TResponse> : HandlerBase
    {
        public abstract Result<TResponse> Dispatch(IQuerySync<TResponse> query, ServiceProviderDelegate serviceProvider);
    }

    internal class QueryHandlerSyncWrapperImpl<TQuery, TResponse> : QueryHandlerSyncWrapper<TResponse>
        where TQuery : IQuerySync<TResponse>
    {
        public override Result<TResponse> Dispatch(IQuerySync<TResponse> query, ServiceProviderDelegate serviceProvider) =>
            GetHandler<IQueryHandlerSync<TQuery, TResponse>>(serviceProvider).Execute((TQuery)query);
    }
}
