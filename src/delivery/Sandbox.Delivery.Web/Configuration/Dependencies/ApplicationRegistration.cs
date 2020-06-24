using System;
using System.Configuration;
using System.Reflection;
using Autofac;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;
using Sandbox.Core.Domain.Intrastructure.Clocks;
using Sandbox.Core.Domain.Intrastructure.Operations.Factories;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Data.Features.HomePages;
using Sandbox.Delivery.Web.Infrastructure.Caching;
using Sandbox.Delivery.Web.Infrastructure.Core;
using Sandbox.Delivery.Web.Infrastructure.PageBuilders;

namespace Sandbox.Delivery.Web.Configuration.Dependencies
{
    public static class ApplicationRegistration
    {
        public static ContainerBuilder RegisterApplicationTypes(this ContainerBuilder builder)
        {
            var appSettings = ConfigurationManager.AppSettings;

            var assemblies = new Assembly[]
            {
                typeof(MvcApplication).Assembly,
                typeof(HomePage).Assembly,
                typeof(HomePageQueryHandler).Assembly,
                typeof(QueryDispatcher).Assembly,
            };

            builder.RegisterType<DateTimeProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();

            // Register CQRS Types
            builder.Register<ServiceProviderDelegate>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(assemblies)
                .Where(MatchCQRSTypes)
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterGenericDecorator(
                typeof(QueryHandlerSyncCacheDecorator<,>),
                typeof(IQueryHandlerSync<,>));

            // Register Mediatr Types
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(ApplicationRegistration).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder
                .RegisterGeneric(typeof(PageBuilderInitializerDecorator<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerRequest();

            builder
                .RegisterType<HttpContextBaseAccessor>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OutputCacheDependenciesStore>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            return builder;
        }
        private static bool MatchCQRSTypes(Type t)
        {
            if (!t.IsClass || t.IsAbstract)
            {
                return false;
            }

            string name = t.Name;

            return name.EndsWith("QueryHandler", StringComparison.Ordinal) ||
                name.EndsWith("CommandHandler", StringComparison.Ordinal) ||
                name.EndsWith("Dispatcher", StringComparison.Ordinal);
        }
    }
}
