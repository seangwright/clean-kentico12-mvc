using System.Threading;
using System.Threading.Tasks;
using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.ContactUsPages;

namespace Sandbox.Delivery.Data.Features.ContactUsPages
{
    public class ContactUsPageQueryHandler : DocumentContextQueryHandler,
        IQueryHandler<ContactUsPageQuery, ContactUsPageQueryResponse>,
        IQueryCacheKeysCreator<ContactUsPageQuery, ContactUsPageQueryResponse>
    {
        public ContactUsPageQueryHandler(IDocumentQueryContext context) : base(context) { }

        public async Task<Result<ContactUsPageQueryResponse>> Execute(ContactUsPageQuery query, CancellationToken token)
        {
            var result = await GetFirstPageWithNodeAliasPath(ContactUsPageProvider.GetContactUsPages(), query, token);

            if (result.IsFailure)
            {
                return Result.Failure<ContactUsPageQueryResponse>(result.Error);
            }

            var node = result.Value;

            return Result.Success(new ContactUsPageQueryResponse("test"));
        }

        public string[] DependencyKeys(ContactUsPageQuery query, Result<ContactUsPageQueryResponse> result) =>
            new[] { FluentCacheKey.ForPage().OfSite(Context.SiteName).WithAliasPath(query.NodeAliasPath) };

        public object[] ItemNameParts(ContactUsPageQuery query) => ItemNameParts(nameof(ContactUsPageQuery), query.NodeAliasPath);
    }
}
