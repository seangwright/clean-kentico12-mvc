using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Infrastructure.Routing
{
    /// <summary>
    /// See http://www.strathweb.com/2015/10/global-route-prefixes-with-attribute-routing-in-asp-net-web-api/
    /// </summary>
    public class ApiBaseDirectRouteProvider : DefaultDirectRouteProvider
    {
        private readonly string basePrefix;

        /// <summary>
        /// Set the base prefix in the controller based on application, namespace and [RoutePrefix] conventions. All routes have a prefix of `/<see cref="basePrefix"/>`.
        /// </summary>
        /// <param name="basePrefix"></param>
        public ApiBaseDirectRouteProvider(string basePrefix)
        {
            Guard.Against.NullOrWhiteSpace(basePrefix, nameof(basePrefix));

            this.basePrefix = basePrefix;
        }

        /// <summary>
        /// Generate a route using the existing route and the base prefix
        /// </summary>
        /// <param name="controllerDescriptor"></param>
        /// <returns></returns>
        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            string controllerPrefix = base.GetRoutePrefix(controllerDescriptor);

            return controllerPrefix is null
                ? basePrefix
                : string.Format("{0}/{1}", basePrefix, controllerPrefix);
        }
    }
}
