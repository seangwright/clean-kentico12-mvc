using System;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Core.Features.Documents
{
    public class DocumentByNodeAliasPathQuery : IQuerySync<DocumentQueryResponse>
    {
        public DocumentByNodeAliasPathQuery(string nodeAliasPath) => NodeAliasPath = nodeAliasPath ?? "";

        public string NodeAliasPath { get; }
    }

    public class DocumentQueryResponse
    {
        public static DocumentQueryResponse Empty { get; } = new DocumentQueryResponse();

        public DocumentQueryResponse(
            Guid nodeGuid,
            int nodeId,
            string nodeAliasPath,
            int documentId,
            string documentName,
            string documentClassName,
            string documentPageTitle,
            string documentPageDescription)
        {
            NodeGuid = nodeGuid;
            NodeId = nodeId;
            DocumentId = documentId;
            DocumentName = documentName ?? "";
            DocumentClassName = documentClassName ?? "";
            DocumentPageTitle = documentPageTitle ?? "";
            DocumentPageDescription = documentPageDescription ?? "";
            NodeAliasPath = nodeAliasPath ?? "";
        }

        protected DocumentQueryResponse()
        {
            NodeAliasPath = "";
            DocumentName = "";
            DocumentClassName = "";
            DocumentPageTitle = "";
            DocumentPageDescription = "";
        }

        public Guid NodeGuid { get; }
        public int NodeId { get; }
        public int DocumentId { get; }
        public string DocumentName { get; }
        public string DocumentClassName { get; }
        public string DocumentPageTitle { get; }
        public string DocumentPageDescription { get; }
        public string NodeAliasPath { get; }
    }
}
