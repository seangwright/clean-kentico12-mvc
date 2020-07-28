using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.Delivery.Web.Configuration;

namespace Sandbox.Delivery.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) => services
            .AddAppKentico()
            .AddResponseCaching()
            .AddAppRoutes()
            .AddAppAuth(LocalizationConfig.DEFAULT_CULTURE)
            .AddAppMvc()
            .AddAppLocalization();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) => app
            .UseAppErrorHandling(env)
            .UseAppKentico()
            .UseStaticFiles()
            .UseRouting()
            .UseResponseCaching()
            .UseAppLocalization()
            .UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseAppRoutes(LocalizationConfig.DEFAULT_CULTURE);
    }
}
