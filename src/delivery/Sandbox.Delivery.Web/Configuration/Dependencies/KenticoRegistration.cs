using System;
using System.Configuration;
using System.Globalization;
using Autofac;
using Sandbox.Data.Kentico.Infrastructure.Caching;
using Sandbox.Delivery.Web.Infrastructure.Caching;
using Sandbox.Delivery.Web.Infrastructure.Contexts;
using Sandbox.Delivery.Web.Infrastructure.PageBuilders;

namespace Sandbox.Delivery.Web.Configuration.Dependencies
{
    public static class KenticoRegistration
    {
        public static ContainerBuilder RegisterKenticoTypes(this ContainerBuilder builder)
        {
            var appSettings = ConfigurationManager.AppSettings;

            builder
                .RegisterType<KenticoCacheHelper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<SiteContext>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<PreviewContext>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DocumentQueryContext>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DocumentContext>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerRequest();

            builder
                .RegisterType<PageBuilderInitializer>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OutputCacheDependenciesStore>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder
                .Register(BuildCacheSettings)
                .SingleInstance();

            builder.RegisterSource(new KenticoRegistrationSource());

            return builder;
        }

        private static QueryCacheSettings BuildCacheSettings(IComponentContext ctx)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string value = appSettings["cache:query-result:duration:seconds"];

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int seconds) && seconds > 0)
            {
                return new QueryCacheSettings(true, TimeSpan.FromSeconds(seconds));
            }

            return new QueryCacheSettings(false, TimeSpan.Zero);
        }
    }
}
