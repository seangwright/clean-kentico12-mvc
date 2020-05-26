namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    public interface IDocumentQueryContext
    {
        string SiteName { get; }
        bool IsPreviewEnabled { get; }
    }
}
