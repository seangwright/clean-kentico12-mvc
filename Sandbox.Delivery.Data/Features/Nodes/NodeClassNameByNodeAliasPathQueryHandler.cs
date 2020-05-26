using System.Linq;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.Nodes;

namespace Sandbox.Delivery.Data.Features.Nodes
{
    public class NodeClassNameByNodeAliasPathQueryHandler : IQueryHandlerSync<NodeClassNameByNodeAliasPathQuery, string>
    {
        private readonly IDocumentQueryContext queryContext;

        public NodeClassNameByNodeAliasPathQueryHandler(IDocumentQueryContext queryContext)
        {
            Guard.Against.Null(queryContext, nameof(queryContext));

            this.queryContext = queryContext;
        }

        public Result<string> Execute(NodeClassNameByNodeAliasPathQuery query)
        {
            var node = DocumentHelper.GetDocuments()
                .LatestVersion(queryContext.IsPreviewEnabled)
                .Published(!queryContext.IsPreviewEnabled)
                .OnSite(queryContext.SiteName)
                .CombineWithDefaultCulture()
                .WhereEquals(nameof(TreeNode.NodeAliasPath), query.NodeAliasPath)
                .TopN(1)
                .Column(nameof(TreeNode.ClassName))
                .FirstOrDefault();

            if (node is null)
            {
                return Result.Failure<string>($"Could not find [{nameof(TreeNode)}] with NodeAliasPath [{query.NodeAliasPath}]");
            }

            return Result.Success(node.ClassName);
        }
    }

    public class NodeClassNameByNodeAliasPathQueryCacheKeysCreator : ContextCacheKeysCreator, IQuerySyncCacheKeysCreator<NodeClassNameByNodeAliasPathQuery, string>
    {
        public NodeClassNameByNodeAliasPathQueryCacheKeysCreator(IDocumentQueryContext queryContext) : base(queryContext) { }

        public string[] DependencyKeys(NodeClassNameByNodeAliasPathQuery query, Result<string> result) =>
            new[]
            {
                FluentCacheKey.ForPage().OfSite(QueryContext.SiteName).WithAliasPath(query.NodeAliasPath)
            };

        public object[] ItemNameParts(NodeClassNameByNodeAliasPathQuery query) =>
            NamePartsFromQuery(nameof(NodeClassNameByNodeAliasPathQuery));
    }
}
