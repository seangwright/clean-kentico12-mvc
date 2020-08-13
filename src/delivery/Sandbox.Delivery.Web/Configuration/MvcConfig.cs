using Microsoft.AspNetCore.Mvc;
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
            .AddMvcOptions(options =>
            {
                options.CacheProfiles.Add("Default", new CacheProfile()
                {
                    Duration = 60,
                    Location = ResponseCacheLocation.Client
                });
                options.CacheProfiles.Add("Disabled", new CacheProfile()
                {
                    Duration = 0,
                    Location = ResponseCacheLocation.None
                });
            })
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
                options.ViewLocationFormats.Insert(0, "/Features/{1}/{0}.cshtml");
            })
            .ReturnServiceCollection(services);

        private static IServiceCollection ReturnServiceCollection(this IMvcBuilder builder, IServiceCollection services) => services;
    }

    public class SharedResources { }
}
