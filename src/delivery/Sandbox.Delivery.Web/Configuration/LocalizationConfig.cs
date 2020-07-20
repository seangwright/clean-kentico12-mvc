using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class LocalizationConfig
    {
        // Default site culture
        public const string DEFAULT_CULTURE = "en-US";

        public static IServiceCollection AddAppLocalization(this IServiceCollection services) => services
            .AddLocalization();

        public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app) => app
            .UseRequestLocalization(options =>
            {
                var supportedCultures = new[]
                {
                    CultureInfo.GetCultureInfo(DEFAULT_CULTURE),
                    CultureInfo.GetCultureInfo("es-ES"),
                };

                options.DefaultRequestCulture = new RequestCulture(DEFAULT_CULTURE);

                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
    }
}
