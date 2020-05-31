using Sandbox.Core.Domain.Infrastructure.Operations.Queries;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.ContactUsPages
{
    public class ContactUsPageQuery : NodeAliasPathQuery, IQuerySync<ContactUsPageQueryResponse>
    {
        public ContactUsPageQuery(string nodeAliasPath) : base(nodeAliasPath) { }
    }

    public class ContactUsPageQueryResponse
    {
        public ContactUsPageQueryResponse(string headerText) => HeaderText = headerText ?? "";

        public string HeaderText { get; }
    }
}
