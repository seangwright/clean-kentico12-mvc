using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Sandbox.Delivery.Web.Features.PageMetas;
using Sandbox.Delivery.Web.Infrastructure.Routing;
using Sandbox.Delivery.Web.Infrastructure.URLs;

namespace Sandbox.Delivery.Web.Configuration.Dependencies
{
    public static class MvcRegistration
    {
        public static ContainerBuilder RegisterMvcTypes(this ContainerBuilder builder)
        {
            var assemblies = new Assembly[]
            {
                typeof(MvcApplication).Assembly
            };

            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterControllers(assemblies);

            builder.RegisterFilterProvider();

            builder
                .Register(ctx => new ControllerActionMatchProvider(typeof(MvcRegistration).Assembly.GetTypes()))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<URLBuilder>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder
                .RegisterType<PageMetaStandardizer>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder
                .RegisterType<PageMetaService<PageMeta>>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            return builder;
        }
    }
}
