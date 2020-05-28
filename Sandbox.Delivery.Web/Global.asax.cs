using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Kentico.Web.Mvc;
using Sandbox.Delivery.Web.Configuration.Dependencies;
using Sandbox.Delivery.Web.Configuration.Pipelines;

namespace Sandbox.Delivery.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            var container = DependencyResolverConfig.BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            WebApiConfig.ConfigureWebApi(container);

            RouteConfig.RegisterRoutes(RouteTable.Routes, container);

            FilterConfig.RegisterGlobalFilters(FilterProviders.Providers, ValueProviderFactories.Factories, container);
        }
    }
}
