using System.Web;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.PageBuilders
{
    public interface IPreviewContext
    {
        bool IsPreviewEnabled { get; }
    }

    public class KenticoPreviewContext : IPreviewContext
    {
        public bool IsPreviewEnabled => HttpContext.Current.Kentico().Preview().Enabled;
    }
}
