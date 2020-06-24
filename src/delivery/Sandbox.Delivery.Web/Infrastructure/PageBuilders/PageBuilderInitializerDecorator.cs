using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Sandbox.Delivery.Web.Infrastructure.Core;

namespace Sandbox.Delivery.Web.Infrastructure.PageBuilders
{
    public class PageBuilderInitializerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        private readonly IPageBuilderInitializer initializer;
        private readonly IDocumentContext documentContext;

        public PageBuilderInitializerDecorator(
            IPageBuilderInitializer initializer,
            IDocumentContext documentContext)
        {
            Guard.Against.Null(initializer, nameof(initializer));
            Guard.Against.Null(documentContext, nameof(documentContext));

            this.initializer = initializer;
            this.documentContext = documentContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (response.IsFailure)
            {
                return response;
            }

            initializer.Initialize(documentContext.DocumentId);

            return response;
        }
    }
}
