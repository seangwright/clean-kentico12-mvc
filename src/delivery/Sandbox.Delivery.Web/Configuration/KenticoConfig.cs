using System;
using System.Collections.Generic;
using System.Linq;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox.Delivery.Web.Configuration
{
    public static class KenticoConfig
    {
        public static IServiceCollection AddAppKentico(this IServiceCollection services) => services
            .AddKentico()
            .AddPageTemplateFilters();

        private static IServiceCollection AddPageTemplateFilters(this IServiceCollection services)
        {
            PageBuilderFilters.PageTemplates.Add(new ArticlePageTemplatesFilter());

            return services;
        }

        public static IApplicationBuilder UseAppKentico(this IApplicationBuilder app) => app
            .UseKentico(features =>
            {
                features.UsePreview();
                features.UsePageBuilder();
            });

        public class ArticlePageTemplatesFilter : IPageTemplateFilter
        {
            /// <summary>
            /// Applies filtering on the given <paramref name="pageTemplates" /> collection based on the given <paramref name="context" />.
            /// </summary>
            /// <returns>
            /// Returns only those page templates that are allowed for article pages if the given context matches the article page type.
            /// </returns>
            public IEnumerable<PageTemplateDefinition> Filter(IEnumerable<PageTemplateDefinition> pageTemplates, PageTemplateFilterContext context) =>
                context.PageType.Equals("SandboxCore.Article", StringComparison.InvariantCultureIgnoreCase)
                    ? pageTemplates.Where(t => GetArticlePageTemplates().Contains(t.Identifier))
                    : pageTemplates.Where(t => !GetArticlePageTemplates().Contains(t.Identifier));

            /// <summary>
            /// Gets all the page templates that are allowed for the article page type.
            /// </summary>
            public IEnumerable<string> GetArticlePageTemplates() =>
                new string[] { "Sandbox.Article", "Sandbox.ArticleWithSidebar" };
        }
    }
}
