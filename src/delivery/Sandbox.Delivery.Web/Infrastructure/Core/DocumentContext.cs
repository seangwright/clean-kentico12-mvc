using System;

namespace Sandbox.Delivery.Web.Infrastructure.Core
{
    public interface IDocumentContext
    {
        Guid NodeGuid { get; }
        int NodeId { get; }
        string NodeAliasPath { get; }
        int DocumentId { get; }
        string DocumentName { get; }
        string DocumentClassName { get; }
        string DocumentPageTitle { get; }
        string DocumentPageDescription { get; }
    }

    public class DocumentContext : IDocumentContext
    {
        public Guid NodeGuid { get; private set; }
        public int NodeId { get; private set; }
        public string NodeAliasPath { get; private set; }
        public int DocumentId { get; private set; }
        public string DocumentName { get; private set; }
        public string DocumentClassName { get; private set; }
        public string DocumentPageTitle { get; private set; }
        public string DocumentPageDescription { get; private set; }

        public DocumentContext()
        {
            NodeAliasPath = "";
            DocumentName = "";
            DocumentClassName = "";
            DocumentPageTitle = "";
            DocumentPageDescription = "";
        }

        public void SetContext(
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
            NodeAliasPath = nodeAliasPath ?? "";
            DocumentId = documentId;
            DocumentName = documentName ?? "";
            DocumentClassName = documentClassName ?? "";
            DocumentPageTitle = documentPageTitle;
            DocumentPageDescription = documentPageDescription;
        }
    }
}
