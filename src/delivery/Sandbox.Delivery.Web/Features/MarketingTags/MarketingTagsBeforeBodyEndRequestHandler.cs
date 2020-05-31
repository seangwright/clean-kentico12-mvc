using Ardalis.GuardClauses;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;

namespace Sandbox.Delivery.Web.Features.MarketingTags
{
    public class MarketingTagsBeforeBodyEndRequestHandler : RequestHandler<MarketingTagsBeforeBodyEndRequest, MarketingTagsBeforeBodyEndViewModel>
    {
        private readonly IQueryDispatcher dispatcher;

        public MarketingTagsBeforeBodyEndRequestHandler(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        protected override MarketingTagsBeforeBodyEndViewModel Handle(MarketingTagsBeforeBodyEndRequest request)
        {
            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return MarketingTagsBeforeBodyEndViewModel.Empty;
            }

            return new MarketingTagsBeforeBodyEndViewModel(result.Value.BeforeBodyEnd);
        }
    }

    public class MarketingTagsBeforeBodyEndRequest : IRequest<MarketingTagsBeforeBodyEndViewModel>
    {

    }

    public class MarketingTagsBeforeBodyEndViewModel
    {
        public static MarketingTagsBeforeBodyEndViewModel Empty { get; } = new MarketingTagsBeforeBodyEndViewModel();

        public string Tags { get; }

        public MarketingTagsBeforeBodyEndViewModel(string tags) => Tags = tags ?? "";

        protected MarketingTagsBeforeBodyEndViewModel() => Tags = "";
    }
}
