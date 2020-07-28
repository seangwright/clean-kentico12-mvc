using System;
using System.Net;
using System.Text.RegularExpressions;
using CMS.Core;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class RouteConfig
    {
        /// <summary>
        /// Configures and adds routing to the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppRoutes(this IServiceCollection services) =>
            services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                options.AppendTrailingSlash = false;
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

        /// <summary>
        /// Defines application routes and URL rewrite rules
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppRoutes(this IApplicationBuilder app, string defaultCulture) => app
            .UseRewriter(new RewriteOptions()
                .Add(new AdminRedirectRewriteRule()))
            .UseEndpoints(endpoints =>
            {
                endpoints.Kentico().MapRoutes();

                endpoints.MapControllerRoute(
                    name: "articleDetail",
                    pattern: AddCulturePrefix(defaultCulture, "{controller}/{id}"),
                    defaults: new { action = "Detail" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: AddCulturePrefix(defaultCulture, "{controller:slugify=Home}/{action:slugify=Index}/{id?}"));

                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "error/{code}",
                    defaults: new { controller = "HttpErrors", action = "Error" });
            });

        private static string AddCulturePrefix(string defaultCulture, string pattern) =>
            $"{{culture={defaultCulture}}}/{pattern}";
    }

    /// <summary>
    /// Transforms a route value to a dash-case-url
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) =>
            value is null
                ? null
                : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2");
    }

    /// <summary>
    /// Redirects a request to "/admin" to the administration site specified in <c>DancingGoatAdminUrl</c> app setting.
    /// </summary>
    public class AdminRedirectRewriteRule : IRule
    {
        private readonly string adminUrl;

        public AdminRedirectRewriteRule() =>
            adminUrl = Service.Resolve<IAppSettingsService>()["AdminUrl"] ?? string.Empty;

        public void ApplyRule(RewriteContext context)
        {
            if (string.IsNullOrEmpty(adminUrl))
            {
                return;
            }

            var request = context.HttpContext.Request;

            if (request.Path.Value.TrimEnd('/').Equals("/admin", StringComparison.OrdinalIgnoreCase))
            {
                var response = context.HttpContext.Response;

                response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                response.Headers[HeaderNames.Location] = adminUrl;
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}
