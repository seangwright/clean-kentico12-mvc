using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ardalis.GuardClauses;
using Sandbox.Delivery.Core.Features.Nodes;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Web.Infrastructure.Routing
{
    /// <summary>
    /// Restricts a route match to requests where the request path is a <see cref="CMS.DocumentEngine.TreeNode.NodeAliasPath"/>
    /// and stores the <see cref="CMS.DocumentEngine.TreeNode.NodeClassName"/> in the <see cref="NODE_CLASS_NAME_VALUE_KEY"/> key
    /// of the <see cref="RouteValueDictionary"/>
    /// </summary>
    public class NodeAliasPathRouteConstraint : IRouteConstraint
    {
        public const string NODE_CLASS_NAME_VALUE_KEY = nameof(NODE_CLASS_NAME_VALUE_KEY);

        private readonly IDependencyResolver dependencyResolver;

        private IQueryDispatcher QueryDispatcher
        {
            get
            {
                var queryDispatcher = dependencyResolver.GetService<IQueryDispatcher>();

                Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));

                return queryDispatcher;
            }
        }

        public IControllerActionMatchProvider ControllerActionMatchProvider
        {
            get
            {
                var provider = dependencyResolver.GetService<IControllerActionMatchProvider>();

                Guard.Against.Null(provider, nameof(provider));

                return provider;
            }
        }


        public NodeAliasPathRouteConstraint(IDependencyResolver dependencyResolver)
        {
            Guard.Against.Null(dependencyResolver, nameof(dependencyResolver));

            this.dependencyResolver = dependencyResolver;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(parameterName, out object requestPathObj))
            {
                return false;
            }

            string requestPath = requestPathObj is null || Equals(requestPathObj, "/")
                ? $"/Home"
                : $"/{requestPathObj}";

            var result = QueryDispatcher.Dispatch(new NodeClassNameByNodeAliasPathQuery(requestPath));

            if (result.IsFailure)
            {
                return false;
            }

            if (!ControllerActionMatchProvider.TryFindMatch(result.Value, out var match))
            {
                return false;
            }

            values["action"] = match.ActionName;
            values["controller"] = match.ControllerName;
            values[parameterName] = requestPath;
            values[NODE_CLASS_NAME_VALUE_KEY] = result.Value;

            return true;
        }
    }
}
