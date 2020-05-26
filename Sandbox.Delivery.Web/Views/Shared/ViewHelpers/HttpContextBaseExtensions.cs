using System.Web;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Views.Shared.ViewHelpers
{
    public static class HttpContextBaseExtensions
    {
        /// <summary>
        /// True if the current HTTP request was made from the CMS to enable page builder mode for a page
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsEditMode(this HttpContextBase context) => context.Kentico().PageBuilder().EditMode;
    }
}
