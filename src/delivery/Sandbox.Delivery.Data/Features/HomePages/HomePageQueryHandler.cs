using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.HomePages;

namespace Sandbox.Delivery.Data.Features.HomePages
{
    public class HomePageQueryHandler : DocumentContextQueryHandler,
        IQueryHandler<HomePageQuery, HomePageQueryResponse>,
        IQueryCacheKeysCreator<HomePageQuery, HomePageQueryResponse>
    {
        public HomePageQueryHandler(IDocumentQueryContext context) : base(context) { }

        public async Task<Result<HomePageQueryResponse>> Execute(HomePageQuery query, CancellationToken token)
        {
            //var result = await GetFirstPageWithNodeAliasPath(HomePageProvider.GetHomePages(), query, token);

            var p = HomePageProvider.GetHomePages()
                .GetLatestSiteDocuments(Context)
                .Path(query.NodeAliasPath, PathTypeEnum.Explicit)
                .TopN(1)
                .FirstOrDefault();

            var result = p is null ? Result.Failure<HomePage>("") : Result.Success(p);

            if (result.IsFailure)
            {
                return Result.Failure<HomePageQueryResponse>(result.Error);
            }

            var node = result.Value;

            return Result.Success(new HomePageQueryResponse(
                node.Fields.HeaderText,
                node.Fields.FooterTitle,
                node.Fields.FooterText));
        }

        public string[] DependencyKeys(HomePageQuery query, Result<HomePageQueryResponse> result) =>
            new[] { FluentCacheKey.ForPage().OfSite(Context.SiteName).WithAliasPath(query.NodeAliasPath) };

        public object[] ItemNameParts(HomePageQuery query) => ItemNameParts(nameof(HomePageQuery), query.NodeAliasPath);
    }
}
