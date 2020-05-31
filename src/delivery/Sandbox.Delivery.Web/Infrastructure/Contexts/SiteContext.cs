
using static CMS.SiteProvider.SiteContext;

namespace Sandbox.Delivery.Web.Infrastructure.Contexts
{
    public interface ISiteContext
    {
        int SiteId { get; }
        string SiteName { get; }
    }

    public class SiteContext : ISiteContext
    {
        public int SiteId => CurrentSiteID;

        public string SiteName => CurrentSiteName;
    }
}
