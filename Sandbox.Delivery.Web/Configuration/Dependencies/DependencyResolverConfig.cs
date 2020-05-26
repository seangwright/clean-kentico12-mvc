using Autofac;

namespace Sandbox.Delivery.Web.Configuration.Dependencies
{
    /// <summary>
    /// Registers required implementations to the Autofac container and set the container as ASP.NET MVC dependency resolver
    /// </summary>
    public static class DependencyResolverConfig
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            var container = builder
                .RegisterApplicationTypes()
                .RegisterMvcTypes()
                .RegisterCmsTypes()
                .RegisterApiTypes()
                .RegisterAuthTypes()
                .RegisterEcommerceTypes()
                .Build();

            return container;
        }
    }
}
