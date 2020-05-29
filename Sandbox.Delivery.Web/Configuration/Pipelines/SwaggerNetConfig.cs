using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Ardalis.GuardClauses;
using Autofac;
using Swagger.Net.Application;

namespace Sandbox.Delivery.Web.Configuration.Pipelines
{
    public static class SwaggerNetConfig
    {
        /// <summary>
        /// Initializes Swagger and SwaggerUI in the application
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static void ConfigureSwaggerNet(IContainer container)
        {
            var httpConfiguration = container.Resolve<HttpConfiguration>();

            httpConfiguration
                .EnableSwagger(c => ConfigureSwagger(c))
                .EnableSwaggerUi(c => ConfigureSwaggerUI(c));
        }

        /// <summary>
        /// Configures Swagger and how it integrates into the Http Server
        /// </summary>
        /// <param name="docsConfig"></param>
        private static void ConfigureSwagger(SwaggerDocsConfig docsConfig)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            docsConfig.SingleApiVersion(assemblyName.Version.ToString().Replace('.', '_'), $"{assemblyName.Name}.API");
            docsConfig.IncludeXmlComments(HostingEnvironment.MapPath($"~/bin/{assemblyName.Name}.xml"));
            docsConfig.UseFullTypeNameInSchemaIds();

            // This line is specifically for running this api in a virtual directory
            // See https://github.com/domaindrivendev/Swashbuckle/issues/305
            docsConfig.RootUrl(req => req.RequestUri.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/").TrimEnd('/'));
        }

        /// <summary>
        /// Configures the UI elements / front end functionality of swagger
        /// </summary>
        /// <param name="uiConfig"></param>
        private static void ConfigureSwaggerUI(SwaggerUiConfig uiConfig)
        {
            uiConfig.DocExpansion(DocExpansion.None);
            uiConfig.DisableValidator();
            uiConfig.EnableDiscoveryUrlSelector();
        }
    }

    /// <summary>
    /// Filters out Kentico's ApiControllers
    /// </summary>
    public class FilteredApiExplorer : IApiExplorer
    {
        private readonly IApiExplorer apiExplorer;

        public Collection<ApiDescription> ApiDescriptions
        {
            get
            {
                string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

                return new Collection<ApiDescription>(
                    apiExplorer
                        .ApiDescriptions
                        .Where(d => d.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace.StartsWith(assemblyName))
                        .ToList());
            }
        }

        public FilteredApiExplorer(HttpConfiguration httpConfiguration)
        {
            Guard.Against.Null(httpConfiguration, nameof(httpConfiguration));

            apiExplorer = httpConfiguration.Services.GetApiExplorer();
        }
    }
}
