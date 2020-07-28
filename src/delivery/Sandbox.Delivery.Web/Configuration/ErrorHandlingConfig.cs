using System;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class ErrorHandlingConfig
    {
        public static IServiceCollection AddErrorHandling(this IServiceCollection services, IWebHostEnvironment env) =>
            services.AddProblemDetails(options =>
            {
                // Only include exception details in a development environment. There's really no nee
                // to set this as it's the default behavior. It's just included here for completeness :)
                options.IncludeExceptionDetails = (ctx, ex) => env.IsDevelopment();

                // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
                // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
                options.Rethrow<NotSupportedException>();

                // This will map NotImplementedException to the 501 Not Implemented status code.
                options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

                //// This will map HttpRequestException to the 503 Service Unavailable status code.
                //options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            });

        public static IApplicationBuilder UseAppErrorHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();

            return (env.IsDevelopment()
                 ? app.UseDeveloperExceptionPage()
                 : app
                     .UseExceptionHandler("/error")
                     .UseStatusCodePages())
             .UseStatusCodePagesWithReExecute("/error/{0}");
        }
    }
}
