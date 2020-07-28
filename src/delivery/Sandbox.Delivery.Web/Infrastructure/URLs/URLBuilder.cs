using System;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUrlHelper urlHelper;

        public URLBuilder(IQueryDispatcher queryDispatcher, IUrlHelper urlHelper)
        {
            Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));
            Guard.Against.Null(urlHelper, nameof(urlHelper));

            this.queryDispatcher = queryDispatcher;
            this.urlHelper = urlHelper;
        }

        public string BuildMVCURL<T>(Func<T, string> nameofAction, object parameters = null) where T : Controller
        {
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
