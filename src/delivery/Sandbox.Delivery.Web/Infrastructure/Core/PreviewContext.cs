using System.Web;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.Core
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
