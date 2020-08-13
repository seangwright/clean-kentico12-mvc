using System;
using System.Threading;
using System.Threading.Tasks;
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
        Task<string> BuildNodeAliasPathURL(Guid nodeGuid, CancellationToken token);
        Task<string> BuildNodeAliasPathURL(TreeNode node, CancellationToken token);
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

        public Task<string> BuildNodeAliasPathURL(Guid nodeGuid, CancellationToken token) => GetNodeAliasPathForNodeGuid(nodeGuid, token);

        public Task<string> BuildNodeAliasPathURL(TreeNode node, CancellationToken token) => GetNodeAliasPathForNodeGuid(node.NodeGUID, token);

        private async Task<string> GetNodeAliasPathForNodeGuid(Guid nodeGuid, CancellationToken token)
        {
            var result = (await queryDispatcher.Dispatch(new NodeAliasPathByNodeGuidQuery(nodeGuid), token));

            return result.IsFailure
                ? ""
                : result.Value.ToLowerInvariant();
        }
    }
}
