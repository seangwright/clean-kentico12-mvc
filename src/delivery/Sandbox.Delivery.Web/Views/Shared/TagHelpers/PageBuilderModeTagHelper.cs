using Ardalis.GuardClauses;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Sandbox.Delivery.Web.Views.Shared.TagHelpers
{
    public class PageBuilderModeTagHelper : TagHelper
    {
        private readonly IPageBuilderDataContext pageBuilderDataContext;

        public PageBuilderModeTagHelper(IPageBuilderDataContext pageBuilderDataContext)
        {
            Guard.Against.Null(pageBuilderDataContext, nameof(pageBuilderDataContext));

            this.pageBuilderDataContext = pageBuilderDataContext;
        }

        public PageBuilderMode Mode { get; set; }

        /// <inheritdoc />
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(output, nameof(output));

            output.TagName = null;

            var currentMode = pageBuilderDataContext.EditMode
                ? PageBuilderMode.Edit
                : PageBuilderMode.Live;

            if (Mode != currentMode)
            {
                output.SuppressOutput();

                return;
            }
        }
    }
}
