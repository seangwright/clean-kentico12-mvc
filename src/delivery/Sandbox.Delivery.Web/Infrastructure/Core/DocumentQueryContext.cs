using Ardalis.GuardClauses;
using Sandbox.Data.Kentico.Infrastructure.Queries;

namespace Sandbox.Delivery.Web.Infrastructure.Core
{
    public class DocumentQueryContext : IDocumentQueryContext
    {
        private readonly ISiteContext siteContext;
        private readonly IPreviewContext previewContext;

        public DocumentQueryContext(ISiteContext siteContext, IPreviewContext previewContext)
        {
            Guard.Against.Null(siteContext, nameof(siteContext));
            Guard.Against.Null(previewContext, nameof(previewContext));

            this.siteContext = siteContext;
            this.previewContext = previewContext;
        }

        public string SiteName => siteContext.SiteName;

        public bool IsPreviewEnabled => previewContext.IsPreviewEnabled;
    }
}
