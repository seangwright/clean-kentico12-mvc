using System;
using System.Web;
using System.Web.Mvc;

namespace Sandbox.Delivery.Web.Views.Shared.ViewHelpers
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteUrl(this UrlHelper urlHelper, string relativeContentPath)
        {
            var url = new Uri(HttpContext.Current.Request.Url, urlHelper.Content(relativeContentPath));

            return url.AbsoluteUri;
        }
    }
}
