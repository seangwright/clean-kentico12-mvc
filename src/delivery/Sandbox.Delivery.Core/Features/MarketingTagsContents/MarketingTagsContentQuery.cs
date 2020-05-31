using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.MarketingTagsContents
{
    public class MarketingTagsContentQuery : IQuerySync<MarketingTagsQueryResponse>
    {
    }

    public class MarketingTagsQueryResponse
    {
        public MarketingTagsQueryResponse(
            string header,
            string afterBodyStart,
            string beforeBodyEnd,
            string pageTitleSuffix,
            string defaultPageDescription,
            string defaultOpenGraphImageUrl,
            string defaultTwitterSite)
        {
            Header = header ?? "";
            AfterBodyStart = afterBodyStart ?? "";
            BeforeBodyEnd = beforeBodyEnd ?? "";
            PageTitleSuffix = pageTitleSuffix ?? "";
            DefaultPageDescription = defaultPageDescription ?? "";
            DefaultOpenGraphImageUrl = defaultOpenGraphImageUrl ?? "";
            DefaultTwitterSite = defaultTwitterSite ?? "";
        }

        public string Header { get; }
        public string AfterBodyStart { get; }
        public string BeforeBodyEnd { get; }
        public string PageTitleSuffix { get; }
        public string DefaultPageDescription { get; }
        public string DefaultOpenGraphImageUrl { get; }
        public string DefaultTwitterSite { get; }
    }
}
