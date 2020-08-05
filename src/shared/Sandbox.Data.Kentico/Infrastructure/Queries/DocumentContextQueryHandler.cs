using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Infrastructure.Operations.Queries;
using static Sandbox.Data.Kentico.Infrastructure.Queries.ContextCacheKeysCreator;

namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    public abstract class DocumentContextQueryHandler<TDocument>
        where TDocument : TreeNode, new()
    {
        protected readonly IDocumentQueryContext Context;

        public DocumentContextQueryHandler(IDocumentQueryContext context)
        {
            Guard.Against.Null(context, nameof(context));

            Context = context;
        }

        /// <summary>
        /// This functionality could mostly be achieved via the Provider.Get--Page() method, which 
        /// allows selection by nodeAliasPath, but this implementation allows for further refinement if needed.
        /// </summary>
        /// <param name="documentQuery"></param>
        /// <param name="nodeAliasPath"></param>
        /// <returns></returns>
        protected async Task<Result<TDocument>> GetFirstPageWithNodeAliasPath(DocumentQuery<TDocument> documentQuery, NodeAliasPathQuery query, CancellationToken token)
        {
            var page = await documentQuery
                .GetLatestSiteDocuments(Context)
                .Path(query.NodeAliasPath, PathTypeEnum.Explicit)
                .TopN(1)
                .FirstOrDefault(token);

            if (page is null)
            {
                return Result.Failure<TDocument>($"Could not find {documentQuery.ClassName} page at [{query.NodeAliasPath}]");
            }

            return Result.Success(page);
        }

        protected object[] ItemNameParts(string queryClassName) => NamePartsFromQuery(Context, queryClassName);

        protected object[] ItemNameParts(string queryClassName, object queryValue) => NamePartsFromQuery(Context, queryClassName, queryValue);
    }
}
