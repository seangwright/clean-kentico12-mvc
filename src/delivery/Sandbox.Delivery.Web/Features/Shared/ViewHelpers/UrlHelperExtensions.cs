using System;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Features.Shared.ViewHelpers
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to the specified content by using the specified content path. Converts a
        /// virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        /// <remarks>From https://stackoverflow.com/a/40649583/939634 </remarks>
        public static string AbsoluteContent(
            this IUrlHelper url,
            string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;

            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
        }

    }
}
