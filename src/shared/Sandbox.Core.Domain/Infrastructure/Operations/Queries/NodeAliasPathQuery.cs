namespace Sandbox.Core.Domain.Infrastructure.Operations.Queries
{
    public abstract class NodeAliasPathQuery
    {
        public NodeAliasPathQuery(string nodeAliasPath) => NodeAliasPath = nodeAliasPath ?? "";

        public string NodeAliasPath { get; }
    }
}
