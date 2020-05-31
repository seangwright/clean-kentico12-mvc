using System.Web;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.Contexts
{
    public interface IPreviewContext
    {
        bool IsPreviewEnabled { get; }
    }

    public class PreviewContext : IPreviewContext
    {
        public bool IsPreviewEnabled => HttpContext.Current.Kentico().Preview().Enabled;
    }
}
