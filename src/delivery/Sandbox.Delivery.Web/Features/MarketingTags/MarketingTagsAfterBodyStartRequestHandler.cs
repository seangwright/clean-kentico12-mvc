using Ardalis.GuardClauses;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;

namespace Sandbox.Delivery.Web.Features.MarketingTags
{
    public class MarketingTagsAfterBodyStartRequestHandler : RequestHandler<MarketingTagsAfterBodyStartRequest, MarketingTagsAfterBodyStartViewModel>
    {
        private readonly IQueryDispatcher dispatcher;

        public MarketingTagsAfterBodyStartRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        protected override MarketingTagsAfterBodyStartViewModel Handle(MarketingTagsAfterBodyStartRequest request)
        {
            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return MarketingTagsAfterBodyStartViewModel.Empty;
            }

            return new MarketingTagsAfterBodyStartViewModel(result.Value.AfterBodyStart);
        }
    }

    public class MarketingTagsAfterBodyStartRequest : IRequest<MarketingTagsAfterBodyStartViewModel>
    {

    }

    public class MarketingTagsAfterBodyStartViewModel
    {
        public static MarketingTagsAfterBodyStartViewModel Empty { get; } = new MarketingTagsAfterBodyStartViewModel();

        public string Tags { get; }

        public MarketingTagsAfterBodyStartViewModel(string tags) => Tags = tags ?? "";

        protected MarketingTagsAfterBodyStartViewModel() => Tags = "";
    }
}
