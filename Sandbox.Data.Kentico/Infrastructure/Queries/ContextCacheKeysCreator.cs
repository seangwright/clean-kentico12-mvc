using Ardalis.GuardClauses;

namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    public abstract class ContextCacheKeysCreator
    {
        protected readonly IDocumentQueryContext QueryContext;

        public ContextCacheKeysCreator(IDocumentQueryContext queryContext)
        {
            Guard.Against.Null(queryContext, nameof(queryContext));

            QueryContext = queryContext;
        }

        public object[] NamePartsFromQuery(string queryClassName) =>
            new object[]
            {
                queryClassName,
                QueryContext.SiteName,
                $"preview:{QueryContext.IsPreviewEnabled}"
            };

        public object[] NamePartsFromQuery(string queryClassName, string nodeAliasPath) =>
            new object[]
            {
                queryClassName,
                QueryContext.SiteName,
                $"preview:{QueryContext.IsPreviewEnabled}",
                nodeAliasPath
            };
    }
}
