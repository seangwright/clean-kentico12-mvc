using Sandbox.Core.Domain.Infrastructure.Operations.Queries;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.HomePages
{
    public class HomePageQuery : NodeAliasPathQuery, IQuerySync<HomePageQueryResponse>
    {
        public HomePageQuery(string nodeAliasPath) : base(nodeAliasPath) { }
    }

    public class HomePageQueryResponse
    {
        public HomePageQueryResponse(
            string headerText,
            string footerTitle,
            string footerText)
        {
            HeaderText = headerText ?? "";
            FooterTitle = footerTitle ?? "";
            FooterText = footerText ?? "";
        }

        public string HeaderText { get; }
        public string FooterTitle { get; }
        public string FooterText { get; }
    }
}
