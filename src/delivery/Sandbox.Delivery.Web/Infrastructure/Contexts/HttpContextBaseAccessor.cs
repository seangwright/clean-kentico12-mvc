using System.Web;

namespace Sandbox.Delivery.Web.Infrastructure.Contexts
{
    public interface IHttpContextBaseAccessor
    {
        HttpContextBase HttpContextBase { get; }
    }

    public class HttpContextBaseAccessor : IHttpContextBaseAccessor
    {
        public HttpContextBase HttpContextBase => new HttpContextWrapper(HttpContext.Current);
    }
}
