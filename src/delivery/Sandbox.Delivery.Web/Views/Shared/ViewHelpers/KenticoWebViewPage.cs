using System.Web;
using System.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

namespace Sandbox.Delivery.Web.Views.Shared.ViewHelpers
{
    public class KenticoWebViewPage<TModel> : WebViewPage<TModel>
    {
        public override void Execute() { }

        /// <summary>
        /// Produces a 'background: url('...')' string that can be applied to a style attribute on an element.
        /// If the imageUrl is empty, an empty string is returned to prevent the browser from
        /// making requests to the root of the site for empty background: url().
        /// Includes rule ending semicolon.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IHtmlString BackgroundImageUrlStyleRule(string imageUrl, SizeConstraint constraint) =>
            string.IsNullOrWhiteSpace(imageUrl)
                ? new HtmlString("")
                : Html.Raw($"background-image: url('{Url.Kentico().ImageUrl(imageUrl, constraint)}');");

        /// <summary>
        /// Wraps .ImageUrl(), returning an empty string if imageUrl is empty instead of throwing an exception
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public string SafeImageUrl(string imageUrl, SizeConstraint constraint) =>
            string.IsNullOrWhiteSpace(imageUrl)
                ? ""
                : Url.Kentico().ImageUrl(imageUrl, constraint);

        /// <summary>
        /// Generates link tags appropriate for links leading to external locations
        /// </summary>
        public string GetExternalLinkTargetAndRelAttributes() =>
            Html.AttributeEncode(@"target=""_blank"" rel=""noopener""");

        /// <summary>
        /// True if the current HTTP request was made from the CMS to enable
        /// preview mode for a page
        /// </summary>
        public bool IsPreviewMode => Context.Kentico().Preview().Enabled;

        /// <summary>
        /// True if the current HTTP request was made from the CMS to enable
        /// Page Builder Edit mode for a page
        /// </summary>
        public bool IsEditMode => Context.Kentico().PageBuilder().EditMode;

        /// <summary>
        /// True if the current request is not for either edit or preview mode
        /// </summary>
        public bool IsLiveMode => !(IsPreviewMode || IsEditMode);
    }
}
