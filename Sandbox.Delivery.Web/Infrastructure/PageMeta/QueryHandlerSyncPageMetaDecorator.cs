using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Infrastructure.Documents;
using Sandbox.Delivery.Web.Infrastructure.Contexts;

namespace Sandbox.Delivery.Web.Infrastructure.PageMeta
{
    public class QueryHandlerSyncPageMetaDecorator<T, TResponse> : IQueryHandlerSync<T, TResponse>
        where T : IQuerySync<TResponse>
    {
        private readonly IPageMetaService<PageMeta> service;
        private readonly IDocumentContext context;
        private readonly IQueryHandlerSync<T, TResponse> handler;

        public QueryHandlerSyncPageMetaDecorator(
            IPageMetaService<PageMeta> service,
            IDocumentContext context,
            IQueryHandlerSync<T, TResponse> handler)
        {
            Guard.Against.Null(service, nameof(service));
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(handler, nameof(handler));

            this.service = service;
            this.context = context;
            this.handler = handler;
        }

        public Result<TResponse> Execute(T query)
        {
            var result = handler.Execute(query);

            if (result.IsFailure)
            {
                SetMetaFromContext(context, service);

                return result;
            }

            var response = result.Value;

            var meta = InitializeMeta(context, response);

            service.Set(meta);

            return result;
        }

        public static void SetMetaFromContext(IDocumentContext context, IPageMetaService<PageMeta> service)
        {
            var meta = new PageMeta(context.DocumentPageTitle);

            meta.SetDescription(context.DocumentPageDescription);

            service.Set(meta);
        }

        public static PageMeta InitializeMeta(IDocumentContext context, TResponse response)
        {
            if (!(response is IPageMetaSource metaSource))
            {
                var meta = new PageMeta(context.DocumentPageTitle);
                meta.SetDescription(context.DocumentPageDescription);

                return meta;
            }
            else
            {
                var meta = new PageMeta(metaSource.PageMetaTitle);
                meta.SetDescription(metaSource.PageMetaDescription);

                return meta;
            }
        }
    }
}
