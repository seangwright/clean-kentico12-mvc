using System;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sandbox.Delivery.Web.Infrastructure.Serialization
{
    /// <summary>
    /// From https://github.com/garysharp/Disco/commit/1dfa3f4f15fe4fc093e90e4cd490dd06cc30cf07
    /// </summary>
    public class JsonDotNetValueProviderFactory : ValueProviderFactory
    {
        private readonly JsonValueProviderFactory originalFactory;
        private readonly JsonSerializerSettings serializerSettings;

        public JsonDotNetValueProviderFactory(
            JsonValueProviderFactory originalFactory,
            JsonSerializerSettings serializerSettings)
        {
            Guard.Against.Null(serializerSettings, nameof(serializerSettings));
            Guard.Against.Null(originalFactory, nameof(originalFactory));

            this.originalFactory = originalFactory;
            this.serializerSettings = serializerSettings;
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            Guard.Against.Null(controllerContext, nameof(controllerContext));

            /*
             * Kentico uses the JsonValueProviderFactory for its internal widget infrastructure
             * and our custom implementation is incompatible, so we use the original
             * for Kentico-related requests
             */
            if (controllerContext.Controller.GetType().FullName.Contains("Kentico"))
            {
                return originalFactory.GetValueProvider(controllerContext);
            }

            var request = controllerContext.HttpContext.Request;

            if (!request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (request.InputStream.Length == 0)
            {
                return null;
            }

            var jsonSerializer = JsonSerializer.Create(serializerSettings);

            jsonSerializer.Converters.Add(new ExpandoObjectConverter());

            using (var streamReader = new StreamReader(request.InputStream, Encoding.UTF8, true, 0x400, true))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var bodyObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);

                return new DictionaryValueProvider<object>(bodyObject, CultureInfo.CurrentCulture);
            }
        }
    }
}
