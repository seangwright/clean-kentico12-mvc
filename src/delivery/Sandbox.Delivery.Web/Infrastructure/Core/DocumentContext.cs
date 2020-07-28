using System;
using Ardalis.GuardClauses;
using CMS.DocumentEngine;
using Kentico.Content.Web.Mvc;

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
        bool IsContextInitialized { get; }
    }

    public class DocumentContext : IDocumentContext
    {
        private readonly IPageDataContextRetriever retriever;

        public Guid NodeGuid => Page.NodeGUID;
        public int NodeId => Page.NodeID;
        public string NodeAliasPath => Page.NodeAliasPath;
        public int DocumentId => Page.DocumentID;
        public string DocumentName => Page.DocumentName;
        public string DocumentClassName => Page.NodeClassName;
        public string DocumentPageTitle => Metadata.Title;
        public string DocumentPageDescription => Metadata.Description;
        public bool IsContextInitialized => Page is object;

        private TreeNode Page => retriever.Retrieve<TreeNode>()?.Page;
        private PageMetadata Metadata => retriever.Retrieve<TreeNode>()?.Metadata;

        public DocumentContext(IPageDataContextRetriever retriever)
        {
            Guard.Against.Null(retriever, nameof(retriever));

            this.retriever = retriever;
        }
    }
}
