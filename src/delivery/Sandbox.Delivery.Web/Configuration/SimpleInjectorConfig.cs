using Kentico.PageBuilder.Web.Mvc;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.Core.Domain.Intrastructure.Operations.Factories;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Data.Features.HomePages;
using Sandbox.Delivery.Web.Features.Home;
using Sandbox.Delivery.Web.Features.PageMetas;
using Sandbox.Delivery.Web.Infrastructure.Core;
using Sandbox.Delivery.Web.Infrastructure.PageBuilders;
using SimpleInjector;
using CMSService = CMS.Core.Service;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class SimpleInjectorConfig
    {
        /// <summary>
        /// See: https://simpleinjector.readthedocs.io/en/latest/aspnetintegration.html
        /// </summary>
        /// <param name="services"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppSimpleInjector(this IServiceCollection services, Container container)
        {
            // Sets up the basic configuration that for integrating Simple Injector with
            // ASP.NET Core by setting the DefaultScopedLifestyle, and setting up auto
            // cross wiring.
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope and
                // allows request-scoped framework services to be resolved.
                options.AddAspNetCore()

                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    // All calls are optional. You can enable what you need. For instance,
                    // ViewComponents, PageModels, and TagHelpers are not needed when you
                    // build a Web API.
                    .AddControllerActivation()
                    .AddViewComponentActivation()
                    .AddPageModelActivation()
                    .AddTagHelperActivation();

                // Optionally, allow application components to depend on the non-generic
                // ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
                // (Microsoft.Extensions.Localization) abstractions.
                options.AddLogging();
                options.AddLocalization();
            });

            InitializeContainer(container);

            return services;
        }

        private static void InitializeContainer(Container container)
        {
            var assemblies = new[]
            {
                typeof(HomePageQueryHandler).Assembly,
                typeof(HomeController).Assembly
            };

            container.Register(typeof(IQueryHandler<,>), assemblies);
            container.Register(typeof(IQueryHandlerSync<,>), assemblies);
            container.Register<IQueryDispatcher, QueryDispatcher>();
            container.Register<ServiceProviderDelegate>(() => t => container.GetInstance(t));
            container.Register<IDocumentQueryContext, DocumentQueryContext>();
            container.Register<ISiteContext, SiteContext>();
            container.Register<IPreviewContext, PreviewContext>();

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
            container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
            {
                typeof(PageBuilderInitializerDecorator<,>)
            });

            container.Register<IPageMetaService<PageMeta>, PageMetaService<PageMeta>>(Lifestyle.Scoped);
            container.Register<IPageMetaStandardizer<PageMeta>, PageMetaStandardizer>(Lifestyle.Scoped);
            container.Register<IDocumentContext, DocumentContext>(Lifestyle.Scoped);

            container.ResolveUnregisteredType += (sender, e) =>
            {
                var serviceType = e.UnregisteredServiceType;

                if (CMSService.ResolveOptional(serviceType) is object service)
                {
                    e.Register(() => service);
                }
                else if (serviceType == typeof(IPageBuilderDataContext))
                {
                    e.Register(()
                        => container.GetInstance<IHttpContextAccessor>()?.HttpContext is object
                            ? container.GetInstance<IPageBuilderDataContextRetriever>()?.Retrieve()
                            : new NullPageBuilderDataContext());
                }
            };
        }

        public static IApplicationBuilder UseAppSimpleInjector(this IApplicationBuilder app, Container container)
            => app.UseSimpleInjector(container);

        public static void UseContainerVerification(this IApplicationBuilder app, Container container)
            => container.Verify();
    }

    internal class NullPageBuilderDataContext : IPageBuilderDataContext
    {
        public PageBuilderConfiguration Configuration => null;

        public bool EditMode => false;

        public PageBuilderOptions Options => null;
    }
}
