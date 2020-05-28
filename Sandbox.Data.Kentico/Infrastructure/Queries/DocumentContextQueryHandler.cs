using System.Linq;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using CSharpFunctionalExtensions;

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
        protected Result<TDocument> GetFirstPageWithNodeAliasPath(DocumentQuery<TDocument> documentQuery, string nodeAliasPath)
        {
            var page = documentQuery
                .GetLatestSiteDocuments(Context)
                .TopN(1)
                .FirstOrDefault();

            if (page is null)
            {
                return Result.Failure<TDocument>($"Could not find {documentQuery.ClassName} page at [{nodeAliasPath}]");
            }

            return Result.Success(page);
        }

        protected object[] ItemNameParts(string queryClassName) => NamePartsFromQuery(Context, queryClassName);

        protected object[] ItemNameParts(string queryClassName, object queryValue) => NamePartsFromQuery(Context, queryClassName, queryValue);
    }
}
