using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ardalis.GuardClauses;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.Documents;
using Sandbox.Delivery.Web.Infrastructure.Contexts;

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

        private DocumentContext DocumentContext
        {
            get
            {
                var context = dependencyResolver.GetService<DocumentContext>();

                Guard.Against.Null(context, nameof(context));

                return context;
            }
        }


        public NodeAliasPathRouteConstraint(IDependencyResolver dependencyResolver)
        {
            Guard.Against.Null(dependencyResolver, nameof(dependencyResolver));

            this.dependencyResolver = dependencyResolver;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            /*
             * Child action request, so the context is already set
             */
            if (!string.IsNullOrWhiteSpace(DocumentContext.DocumentClassName))
            {
                return true;
            }

            if (!values.TryGetValue(parameterName, out object requestPathObj))
            {
                return false;
            }

            string requestPath = requestPathObj is null || Equals(requestPathObj, "/")
                ? $"/Home"
                : $"/{requestPathObj}";

            var result = QueryDispatcher.Dispatch(new DocumentByNodeAliasPathQuery(requestPath));

            if (result.IsFailure)
            {
                return false;
            }

            var response = result.Value;

            if (!ControllerActionMatchProvider.TryFindMatch(response.DocumentClassName, out var match))
            {
                return false;
            }

            DocumentContext.SetContext(
                response.NodeGuid,
                response.NodeId,
                response.NodeAliasPath,
                response.DocumentId,
                response.DocumentName,
                response.DocumentClassName,
                response.DocumentPageTitle,
                response.DocumentPageDescription);

            values["action"] = match.ActionName;
            values["controller"] = match.ControllerName;
            values[parameterName] = requestPath;
            values[NODE_CLASS_NAME_VALUE_KEY] = result.Value;

            return true;
        }
    }
}
