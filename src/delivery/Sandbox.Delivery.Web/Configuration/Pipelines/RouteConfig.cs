using System.Web.Mvc;
using System.Web.Routing;
using Kentico.Web.Mvc;
using Sandbox.Delivery.Web.Infrastructure.Routing;

namespace Sandbox.Delivery.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Content/{*pathInfo}");

            routes.Kentico().MapRoutes();

            routes.MapRoute(
                name: "NodeAliasPath",
                url: "{*nodeAliasPath}",
                defaults: null,
                constraints: new
                {
                    requestPath = new NodeAliasPathRouteConstraint(DependencyResolver.Current)
                });

            routes.MapRoute("StandardConvention", "{controller}/{action}/{id}", new
            {
                controller = "NotFound",
                action = "Index",
                id = UrlParameter.Optional
            });

            routes.AppendTrailingSlash = false;
            routes.LowercaseUrls = true;
        }
    }
}
