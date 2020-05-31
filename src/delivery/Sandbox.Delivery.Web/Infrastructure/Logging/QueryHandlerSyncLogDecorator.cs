using Ardalis.GuardClauses;
using CMS.EventLog;
using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Web.Infrastructure.Logging
{
    public class QueryHandlerSyncLogDecorator<TQuery, TResponse> : IQueryHandlerSync<TQuery, TResponse>
        where TQuery : IQuerySync<TResponse>
    {
        private readonly IQueryHandlerSync<TQuery, TResponse> handler;

        public QueryHandlerSyncLogDecorator(IQueryHandlerSync<TQuery, TResponse> handler)
        {
            Guard.Against.Null(handler, nameof(handler));

            this.handler = handler;
        }

        public Result<TResponse> Execute(TQuery query)
        {
            var result = handler.Execute(query);

            if (result.IsFailure)
            {
                EventLogProvider.LogEvent(new EventLogInfo
                {
                    EventCode = "Error",
                    EventDescription = result.Error,
                    EventType = query.GetType().Name,
                });
            }

            return result;
        }
    }
}
