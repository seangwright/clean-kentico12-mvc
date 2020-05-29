using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.HomePages;

namespace Sandbox.Delivery.Data.Features.HomePages
{
    public class HomePageQueryHandler : DocumentContextQueryHandler<HomePage>,
        IQueryHandlerSync<HomePageQuery, HomePageQueryResponse>,
        IQuerySyncCacheKeysCreator<HomePageQuery, HomePageQueryResponse>
    {
        public HomePageQueryHandler(IDocumentQueryContext context) : base(context) { }

        public Result<HomePageQueryResponse> Execute(HomePageQuery query)
        {
            var result = GetFirstPageWithNodeAliasPath(HomePageProvider.GetHomePages(), query);

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
