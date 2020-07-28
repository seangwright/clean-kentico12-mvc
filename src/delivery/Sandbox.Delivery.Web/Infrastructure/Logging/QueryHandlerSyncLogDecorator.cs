﻿using Ardalis.GuardClauses;
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

        public Result<TResponse> Execute(TQuery query) =>
            handler
                .Execute(query)
                .OnFailure(error => EventLogProvider.LogEvent(new EventLogInfo
                {
                    EventCode = "Content Delivery Query Error",
                    EventDescription = error,
                    EventType = query.GetType().Name,
                }));
    }
}
