using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.Nodes;

using static Sandbox.Data.Kentico.Infrastructure.Queries.ContextCacheKeysCreator;

namespace Sandbox.Delivery.Data.Features.Nodes
{
    public class NodeAliasPathByNodeGuidQueryHandler :
        IQueryHandler<NodeAliasPathByNodeGuidQuery, string>,
        IQueryCacheKeysCreator<NodeAliasPathByNodeGuidQuery, string>
    {
        private readonly IDocumentQueryContext context;

        public NodeAliasPathByNodeGuidQueryHandler(IDocumentQueryContext context)
        {
            Guard.Against.Null(context, nameof(context));

            this.context = context;
        }

        public async Task<Result<string>> Execute(NodeAliasPathByNodeGuidQuery query, CancellationToken token)
        {
            var node = await DocumentHelper
                .GetDocuments()
                .GetLatestSiteDocuments(context)
                .WhereEquals(nameof(TreeNode.NodeGUID), query.NodeGuid)
                .TopN(1)
                .FirstOrDefault(token);

            if (node is null)
            {
                return Result.Failure<string>($"Could not find node [{query.NodeGuid}]");
            }

            return Result.Success(node.NodeAliasPath);
        }

        public string[] DependencyKeys(NodeAliasPathByNodeGuidQuery query, Result<string> result) =>
            new[] { FluentCacheKey.ForPage().OfSite(context.SiteName).WithNodeGuid(query.NodeGuid) };

        public object[] ItemNameParts(NodeAliasPathByNodeGuidQuery query) =>
            NamePartsFromQuery(context, nameof(NodeAliasPathByNodeGuidQuery), query.NodeGuid);
    }
}
