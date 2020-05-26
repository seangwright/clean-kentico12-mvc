using System.Linq;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.HomePages;

namespace Sandbox.Delivery.Data.Features.HomePages
{
    public class HomePageQueryHandler : IQueryHandlerSync<HomePageQuery, HomePageQueryResponse>
    {
        private readonly IDocumentQueryContext queryContext;

        public HomePageQueryHandler(IDocumentQueryContext queryContext)
        {
            Guard.Against.Null(queryContext, nameof(queryContext));

            this.queryContext = queryContext;
        }

        public Result<HomePageQueryResponse> Execute(HomePageQuery query)
        {
            var node = HomePageProvider.GetHomePages()
               .LatestVersion(queryContext.IsPreviewEnabled)
               .Published(!queryContext.IsPreviewEnabled)
               .OnSite(queryContext.SiteName)
               .CombineWithDefaultCulture()
               .WhereEquals(nameof(TreeNode.NodeAliasPath), query.NodeAliasPath)
               .TopN(1)
               .FirstOrDefault();

            if (node is null)
            {
                return Result.Failure<HomePageQueryResponse>($"Could not find [{nameof(HomePage)}] with NodeAliasPath [{query.NodeAliasPath}]");
            }

            return Result.Success(new HomePageQueryResponse(
                node.DocumentID,
                node.Fields.HeaderText,
                node.Fields.FooterTitle,
                node.Fields.FooterText,
                node.DocumentPageTitle,
                node.DocumentPageDescription));
        }
    }

    public class HomePageQueryCacheKeysCreator : ContextCacheKeysCreator, IQuerySyncCacheKeysCreator<HomePageQuery, HomePageQueryResponse>
    {
        public HomePageQueryCacheKeysCreator(IDocumentQueryContext queryContext) : base(queryContext) { }

        public string[] DependencyKeys(HomePageQuery query, Result<HomePageQueryResponse> result) =>
            new[]
            {
                FluentCacheKey.ForPage().OfSite(QueryContext.SiteName).WithAliasPath(query.NodeAliasPath)
            };

        public object[] ItemNameParts(HomePageQuery query) => NamePartsFromQuery(nameof(HomePageQuery), query.NodeAliasPath);
    }
}
