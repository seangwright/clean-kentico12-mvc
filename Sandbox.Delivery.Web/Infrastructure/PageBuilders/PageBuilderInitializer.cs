using System.Web;
using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.PageBuilders
{
    public interface IPageBuilderInitializer
    {
        void Initialize(TreeNode treeNode);
        void Initialize(int documentId);
    }

    public class HttpContextPageBuilderInitializer : IPageBuilderInitializer
    {
        public void Initialize(TreeNode treeNode) =>
            HttpContext.Current.Kentico().PageBuilder().Initialize(treeNode.DocumentID);

        public void Initialize(int documentId) =>
            HttpContext.Current.Kentico().PageBuilder().Initialize(documentId);
    }
}
