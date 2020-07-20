using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;

namespace Sandbox.Delivery.Web.Features.MarketingTags
{
    public class MarketingTagsViewComponent : ViewComponent
    {
        private readonly IQueryDispatcher dispatcher;

        public MarketingTagsViewComponent(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        public IViewComponentResult Invoke(string area)
        {
            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return View(MarketingTagsViewModel.Empty);
            }

            string tagValue = GetTagValue(result.Value, area);

            return View("~/Features/MarketingTags/MarketingTags.cshtml", new MarketingTagsViewModel(tagValue));
        }

        private string GetTagValue(MarketingTagsQueryResponse response, string area)
        {
            switch (area)
            {
                case "Header":
                    return response.Header;

                case "AfterBodyStart":
                    return response.AfterBodyStart;

                case "BeforeBodyEnd":
                    return response.BeforeBodyEnd;

                default:
                    return "";
            }
        }
    }

    public class MarketingTagsViewModel
    {
        public static MarketingTagsViewModel Empty { get; } = new MarketingTagsViewModel("");

        public MarketingTagsViewModel(string tags) => Tags = tags ?? "";

        public string Tags { get; }
    }
}
