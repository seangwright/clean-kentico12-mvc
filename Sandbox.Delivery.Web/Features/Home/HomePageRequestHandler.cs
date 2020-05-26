using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.HomePages;
using Sandbox.Delivery.Web.Infrastructure.PageBuilders;

namespace Sandbox.Delivery.Web.Features.Home
{
    public class HomePageRequestHandler : RequestHandler<HomePageRequest, Maybe<HomePageViewModel>>
    {
        private readonly IQueryDispatcher dispatcher;

        public HomePageRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        protected override Maybe<HomePageViewModel> Handle(HomePageRequest request)
        {
            var result = dispatcher.Dispatch(new HomePageQuery(request.RequestPath));

            if (result.IsFailure)
            {
                return Maybe<HomePageViewModel>.None;
            }

            var response = result.Value;

            return new HomePageViewModel(response.DocumentId, response.HeaderText);
        }
    }

    public class HomePageRequest : IRequest<Maybe<HomePageViewModel>>
    {
        public HomePageRequest(string requestPath) => RequestPath = requestPath ?? "";

        public string RequestPath { get; }
    }

    public class HomePageViewModel : IPageBuilderViewModel
    {
        public HomePageViewModel(int documentId, string title)
        {
            DocumentId = documentId;
            Title = title ?? "";
        }

        public int DocumentId { get; }
        public string Title { get; }
    }
}
