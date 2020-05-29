using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Sandbox.Delivery.Web.Infrastructure.PageMeta;
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

            builder
                .RegisterType<PageMetaActionFilterAttribute>()
                .AsActionFilterFor<Controller>()
                .InstancePerRequest()
                .PropertiesAutowired();

            return builder;
        }
    }
}
