using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.Documents;

using static Sandbox.Data.Kentico.Infrastructure.Queries.ContextCacheKeysCreator;

namespace Sandbox.Delivery.Data.Features.Documents
{
    public class DocumentByNodeAliasPathQueryHandler :
        IQueryHandler<DocumentByNodeAliasPathQuery, DocumentQueryResponse>,
        IQueryCacheKeysCreator<DocumentByNodeAliasPathQuery, DocumentQueryResponse>
    {
        private readonly IDocumentQueryContext context;

        public DocumentByNodeAliasPathQueryHandler(IDocumentQueryContext context)
        {
            Guard.Against.Null(context, nameof(context));

            this.context = context;
        }

        public async Task<Result<DocumentQueryResponse>> Execute(DocumentByNodeAliasPathQuery query, CancellationToken token)
        {
            var response = (await DocumentHelper.GetDocuments()
                .LatestVersion(context.IsPreviewEnabled)
                .Published(!context.IsPreviewEnabled)
                .OnSite(context.SiteName)
                .CombineWithDefaultCulture()
                .WhereEquals(nameof(TreeNode.NodeAliasPath), query.NodeAliasPath)
                .TopN(1)
                .Columns(
                    nameof(TreeNode.NodeID),
                    nameof(TreeNode.NodeGUID),
                    nameof(TreeNode.NodeAliasPath),
                    nameof(TreeNode.DocumentID),
                    nameof(TreeNode.DocumentName),
                    nameof(TreeNode.ClassName),
                    nameof(TreeNode.DocumentPageTitle),
                    nameof(TreeNode.DocumentPageDescription)
                )
                .GetEnumerableTypedResultAsync(cancellationToken: token))
                .Select(n => new DocumentQueryResponse(
                    n.NodeGUID,
                    n.NodeID,
                    n.NodeAliasPath,
                    n.DocumentID,
                    n.DocumentName,
                    n.ClassName,
                    n.DocumentPageTitle,
                    n.DocumentPageDescription))
                .FirstOrDefault();

            if (response is null)
            {
                return Result.Failure<DocumentQueryResponse>($"Could not find [{nameof(TreeNode)}] with NodeAliasPath [{query.NodeAliasPath}]");
            }

            return Result.Success(response);
        }

        public string[] DependencyKeys(DocumentByNodeAliasPathQuery query, Result<DocumentQueryResponse> result) =>
            new[] { FluentCacheKey.ForPage().OfSite(context.SiteName).WithAliasPath(query.NodeAliasPath) };

        public object[] ItemNameParts(DocumentByNodeAliasPathQuery query) =>
            NamePartsFromQuery(context, nameof(DocumentByNodeAliasPathQuery), query.NodeAliasPath);
    }
}
