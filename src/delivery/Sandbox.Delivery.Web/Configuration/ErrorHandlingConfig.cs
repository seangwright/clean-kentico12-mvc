using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class ErrorHandlingConfig
    {
        public static IApplicationBuilder UseAppErrorHandling(this IApplicationBuilder app, IWebHostEnvironment env) =>
            (env.IsDevelopment()
                ? app.UseDeveloperExceptionPage()
                : app)
            .UseStatusCodePagesWithReExecute("/error/{0}");
    }
}
