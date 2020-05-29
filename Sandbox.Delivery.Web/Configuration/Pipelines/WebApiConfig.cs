using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using FluentValidation.WebApi;
using Sandbox.Delivery.Web.Infrastructure.Routing;

namespace Sandbox.Delivery.Web
{
    public static class WebApiConfig
    {
        public static HttpConfiguration ConfigureWebApi(IContainer container)
        {
            var config = container.Resolve<HttpConfiguration>();

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Remove XML formatter and any others
            config.Formatters.Clear();

            config.Formatters.Add(container.Resolve<JsonMediaTypeFormatter>());

            config.Services.Replace(typeof(IApiExplorer), container.Resolve<FilteredApiExplorer>());
            config.Services.Replace(typeof(IExceptionHandler), container.Resolve<GlobalApiExceptionHandler>());

            config.MapHttpAttributeRoutes(new ApiBaseDirectRouteProvider("api"));

            FluentValidationModelValidatorProvider.Configure(config, p => p.ValidatorFactory = new FluentValidationValidatorFactory(container));

            config.EnsureInitialized();

            return config;
        }
    }

    internal class FluentValidationValidatorFactory : ValidatorFactoryBase
    {
        private readonly IContainer context;

        public FluentValidationValidatorFactory(IContainer context) => this.context = context;

        public override IValidator CreateInstance(Type validatorType)
        {
            if (context.TryResolve(validatorType, out object validator))
            {
                return validator as IValidator;
            }

            return null;
        }
    }
}
