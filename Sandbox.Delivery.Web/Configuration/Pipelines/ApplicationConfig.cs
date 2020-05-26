using System.Web.Optimization;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Configuration.Pipelines
{
    public class ApplicationConfig
    {
        public static void RegisterFeatures(IApplicationBuilder builder)
        {
            builder.UsePreview();

            builder.UsePageBuilder(new PageBuilderOptions
            {
                RegisterDefaultSection = true
            });

            builder.UsePageRouting(new PageRoutingOptions
            {
                EnableAlternativeUrls = true
            });

            builder.UseResourceSharingWithAdministration();

            BundleTable.EnableOptimizations = false;
        }
    }
}
