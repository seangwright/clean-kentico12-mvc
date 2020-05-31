using System;
using System.Web;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.Nodes;
using Sandbox.Delivery.Web.Infrastructure.Routing;

namespace Sandbox.Delivery.Web.Infrastructure.URLs
{
    public interface IURLBuilder
    {
        string BuildMVCURL<T>(Func<T, string> nameofAction, object parameters = null) where T : Controller;
        string BuildNodeAliasPathURL(Guid nodeGuid);
        string BuildNodeAliasPathURL(TreeNode node);
    }

    public class URLBuilder : IURLBuilder
    {
        private readonly IQueryDispatcher queryDispatcher;

        public URLBuilder(IQueryDispatcher queryDispatcher)
        {
            Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));

            this.queryDispatcher = queryDispatcher;
        }

        public string BuildMVCURL<T>(Func<T, string> nameofAction, object parameters = null) where T : Controller
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            string controllerName = typeof(T).RemoveControllerSuffix();
            string actionName = nameofAction(default);

            return urlHelper.Action(actionName, controllerName, parameters);
        }

        public string BuildNodeAliasPathURL(Guid nodeGuid) => GetNodeAliasPathForNodeGuid(nodeGuid);

        public string BuildNodeAliasPathURL(TreeNode node) => GetNodeAliasPathForNodeGuid(node.NodeGUID);

        private string GetNodeAliasPathForNodeGuid(Guid nodeGuid)
        {
            var result = queryDispatcher.Dispatch(new NodeAliasPathByNodeGuidQuery(nodeGuid));

            if (result.IsFailure)
            {
                return "";
            }

            return result.Value.ToLowerInvariant();
        }
    }
}
