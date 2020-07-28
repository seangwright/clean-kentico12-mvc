using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using Kentico.Content.Web.Mvc;
using MediatR;
using Sandbox.Delivery.Web.Infrastructure.Core;

namespace Sandbox.Delivery.Web.Infrastructure.PageBuilders
{
    public class PageBuilderInitializerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        private readonly IPageDataContextInitializer initializer;
        private readonly IDocumentContext context;

        public PageBuilderInitializerDecorator(
            IPageDataContextInitializer initializer,
            IDocumentContext context)
        {
            Guard.Against.Null(initializer, nameof(initializer));
            Guard.Against.Null(context, nameof(context));

            this.initializer = initializer;
            this.context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (response.IsFailure)
            {
                return response;
            }

            if (context.IsContextInitialized)
            {
                initializer.Initialize(context.DocumentId);
            }

            return response;
        }
    }
}
