using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sandbox.Delivery.Web.Configuration.Pipelines;
using Sandbox.Delivery.Web.Infrastructure.Caching;
using Sandbox.Delivery.Web.Infrastructure.ErrorHandling;
using Sandbox.Delivery.Web.Infrastructure.Validation;

namespace Sandbox.Delivery.Web.Configuration.Dependencies
{
    public static class APIRegistration
    {
        public static ContainerBuilder RegisterAPITypes(this ContainerBuilder builder)
        {
            builder
                .RegisterType<GlobalApiExceptionHandler>()
                .SingleInstance();

            builder
                .RegisterType<FilteredApiExplorer>()
                .SingleInstance();

            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            builder
                .Register(c => GlobalConfiguration.Configuration)
                .SingleInstance();

            builder
                .RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            builder
                .RegisterType<APICacheControlFilter>()
                .AsWebApiActionFilterForAllControllers();

            builder.Register(c => new JsonSerializerSettings
            {
                Formatting = Formatting.None,

                // UTC Date serialization configuration
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,

                // Use 7 digits of precision (fffffff) to match data store datetimeoffset(7)
                DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK",

                ContractResolver = new CamelCasePropertyNamesContractResolver()
            })
            .SingleInstance();

            builder.Register(c => new JsonMediaTypeFormatter
            {
                SerializerSettings = c.Resolve<JsonSerializerSettings>(),
            })
            .SingleInstance();

            builder
                .RegisterAssemblyTypes(typeof(APIRegistration).Assembly)
                .Where(t => t.Name.EndsWith("Validator", StringComparison.OrdinalIgnoreCase))
                .AsClosedTypesOf(typeof(IValidator<>));

            builder
                .RegisterType<ValidationAPIActionFilter>()
                .AsWebApiActionFilterForAllControllers()
                .InstancePerRequest();

            return builder;
        }
    }
}
