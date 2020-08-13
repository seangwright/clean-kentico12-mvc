using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sandbox.Delivery.Web.Configuration;
using SimpleInjector;

namespace Sandbox.Delivery.Web
{
    public class Startup
    {
        private readonly Container container = new Container();

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddResponseCaching()
                .AddAppRoutes()
                .AddAppAuth(LocalizationConfig.DEFAULT_CULTURE)
                .AddProblemDetails()
                .AddAppMvc()
                .AddAppLocalization()
                .AddAppKentico()
                .AddAppSimpleInjector(container);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => app
                .UseAppSimpleInjector(container)
                .UseAppErrorHandling(env)
                .UseAppKentico()
                .UseStaticFiles()
                .UseRouting()
                .UseResponseCaching()
                .UseAppLocalization()
                .UseCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseAppRoutes(LocalizationConfig.DEFAULT_CULTURE)
                .UseContainerVerification(container);
    }
}
