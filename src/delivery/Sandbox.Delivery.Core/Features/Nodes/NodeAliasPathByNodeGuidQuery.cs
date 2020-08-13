﻿using System;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.Nodes
{
    public class NodeAliasPathByNodeGuidQuery : IQuery<string>
    {
        public NodeAliasPathByNodeGuidQuery(Guid nodeGuid) => NodeGuid = nodeGuid;

        public Guid NodeGuid { get; }
    }
}
