using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.HomePages;

namespace Sandbox.Delivery.Web.Features.Home
{
    public class HomePageRequestHandler : IRequestHandler<HomePageRequest, Maybe<HomePageViewModel>>
    {
        private readonly IQueryDispatcher dispatcher;

        public HomePageRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        public async Task<Maybe<HomePageViewModel>> Handle(HomePageRequest request, CancellationToken cancellationToken)
        {
            var result = await dispatcher.Dispatch(new HomePageQuery(request.RequestPath), cancellationToken);

            if (result.IsFailure)
            {
                return Maybe<HomePageViewModel>.None;
            }

            var response = result.Value;

            return new HomePageViewModel(response.HeaderText);
        }
    }

    public class HomePageRequest : IRequest<Maybe<HomePageViewModel>>
    {
        public HomePageRequest(string requestPath) => RequestPath = requestPath ?? "";

        public string RequestPath { get; }
    }

    public class HomePageViewModel
    {
        public HomePageViewModel(string title) => Title = title ?? "";

        public string Title { get; }
    }
}
