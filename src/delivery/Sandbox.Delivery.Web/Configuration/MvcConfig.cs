using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection AddAppMvc(this IServiceCollection services) => services
            .AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResources));
            })
            .ReturnServiceCollection(services);

        private static IServiceCollection ReturnServiceCollection(this IMvcBuilder builder, IServiceCollection services) => services;
    }

    public class SharedResources { }
}
