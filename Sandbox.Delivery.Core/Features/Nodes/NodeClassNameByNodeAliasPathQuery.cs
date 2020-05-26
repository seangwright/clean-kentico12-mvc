using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.Nodes
{
    public class NodeClassNameByNodeAliasPathQuery : IQuerySync<string>
    {
        public NodeClassNameByNodeAliasPathQuery(string nodeAliasPath) => NodeAliasPath = nodeAliasPath ?? "";

        public string NodeAliasPath { get; }
    }
}
