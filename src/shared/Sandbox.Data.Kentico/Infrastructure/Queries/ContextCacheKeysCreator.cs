namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    public static class ContextCacheKeysCreator
    {
        public static object[] NamePartsFromQuery(IDocumentQueryContext context, string queryClassName) =>
            new object[]
            {
                queryClassName,
                context.SiteName,
                $"preview:{context.IsPreviewEnabled}"
            };

        public static object[] NamePartsFromQuery(IDocumentQueryContext context, string queryClassName, object queryValue) =>
            new object[]
            {
                queryClassName,
                context.SiteName,
                $"preview:{context.IsPreviewEnabled}",
                queryValue
            };
    }
}
