using Ardalis.GuardClauses;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;

namespace Sandbox.Delivery.Web.Features.MarketingTags
{
    public class MarketingTagsHeaderRequestHandler : RequestHandler<MarketingTagsHeaderRequest, MarketingTagsHeaderViewModel>
    {
        private readonly IQueryDispatcher dispatcher;

        public MarketingTagsHeaderRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        protected override MarketingTagsHeaderViewModel Handle(MarketingTagsHeaderRequest request)
        {
            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return MarketingTagsHeaderViewModel.Empty;
            }

            return new MarketingTagsHeaderViewModel(result.Value.Header);
        }
    }

    public class MarketingTagsHeaderRequest : IRequest<MarketingTagsHeaderViewModel>
    {

    }

    public class MarketingTagsHeaderViewModel
    {
        public static MarketingTagsHeaderViewModel Empty { get; } = new MarketingTagsHeaderViewModel();

        public string Tags { get; }

        public MarketingTagsHeaderViewModel(string tags) => Tags = tags ?? "";

        protected MarketingTagsHeaderViewModel() => Tags = "";
    }
}
