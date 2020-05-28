using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Infrastructure.Documents;

namespace Sandbox.Delivery.Core.Features.HomePages
{
    public class HomePageQuery : IQuerySync<HomePageQueryResponse>
    {
        public HomePageQuery(string nodeAliasPath) => NodeAliasPath = nodeAliasPath ?? "";

        public string NodeAliasPath { get; }
    }

    public class HomePageQueryResponse : IPageMetaSource
    {
        public HomePageQueryResponse(
            string headerText,
            string footerTitle,
            string footerText,
            string pageMetaTitle,
            string pageMetaDescription)
        {
            HeaderText = headerText ?? "";
            FooterTitle = footerTitle ?? "";
            FooterText = footerText ?? "";
            PageMetaTitle = pageMetaTitle ?? "";
            PageMetaDescription = pageMetaDescription ?? "";
        }

        public string HeaderText { get; }
        public string FooterTitle { get; }
        public string FooterText { get; }
        public string PageMetaTitle { get; }
        public string PageMetaDescription { get; }
    }
}
