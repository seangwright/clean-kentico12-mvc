using System.Linq;
using System.Web.Mvc;
using Autofac;
using Newtonsoft.Json;
using Sandbox.Delivery.Web.Infrastructure.Serialization;

namespace Sandbox.Delivery.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(
            ValueProviderFactoryCollection factories,
            IContainer container)
        {
            // Remove Default Json Value Provider Factory (JavaScriptSerializer)
            var defaultJsonValueProvider = factories
                .OfType<JsonValueProviderFactory>()
                .FirstOrDefault();

            if (defaultJsonValueProvider is object)
            {
                factories.Remove(defaultJsonValueProvider);
            }

            var serializerSettings = container.Resolve<JsonSerializerSettings>();

            // Add Custom Json Value Provider Factory (Json.Net)
            factories.Add(new JsonDotNetValueProviderFactory(defaultJsonValueProvider, serializerSettings));
        }
    }
}
