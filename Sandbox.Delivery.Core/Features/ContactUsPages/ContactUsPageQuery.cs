using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Infrastructure.Documents;

namespace Sandbox.Delivery.Core.Features.ContactUsPages
{
    public class ContactUsPageQuery : IQuerySync<ContactUsPageQueryResponse>
    {
        public ContactUsPageQuery(string nodeAliasPath) => NodeAliasPath = nodeAliasPath ?? "";

        public string NodeAliasPath { get; }
    }

    public class ContactUsPageQueryResponse : IPageMetaSource
    {
        public ContactUsPageQueryResponse(
            string headerText,
            string pageMetaTitle,
            string pageMetaDescription)
        {
            HeaderText = headerText ?? "";
            PageMetaTitle = pageMetaTitle ?? "";
            PageMetaDescription = pageMetaDescription ?? "";
        }

        public int DocumentId { get; }
        public string HeaderText { get; }
        public string PageMetaTitle { get; }
        public string PageMetaDescription { get; }
    }
}
