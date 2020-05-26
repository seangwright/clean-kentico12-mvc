using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;

namespace Sandbox.Delivery.Web.Infrastructure.PageBuilders
{
    public class RequestHandlerPageBuilderDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        private readonly IPageBuilderInitializer initializer;

        public RequestHandlerPageBuilderDecorator(
            IPageBuilderInitializer initializer)
        {
            Guard.Against.Null(initializer, nameof(initializer));

            this.initializer = initializer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (response.IsFailure)
            {
                return response;
            }

            var resultType = typeof(TResponse);

            if (!resultType.IsGenericType)
            {
                return response;
            }

            var property = resultType.GetProperty(nameof(Result<object>.Value));

            object resultValue = property.GetValue(response);

            if (!(resultValue is IPageBuilderViewModel viewModel))
            {
                return response;
            }

            initializer.Initialize(viewModel.DocumentId);

            return response;
        }
    }
}
