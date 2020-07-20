using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Sandbox.Delivery.Web.Views.Shared
{
    public enum PageBuilderMode
    {
        Live,
        Edit
    }

    public class KenticoRazorPage<TModel> : RazorPage<TModel>
    {
        public override Task ExecuteAsync() => Task.CompletedTask;

        /// <summary>
        /// True if the current HTTP request was made from the CMS to enable
        /// Page Builder Edit mode for a page
        /// </summary>
        public bool IsEditMode => Context.Kentico().PageBuilder().EditMode;

        /// <summary>
        /// True if the current request is not for edit mode
        /// </summary>
        public bool IsLiveMode => !IsEditMode;

        public PageBuilderMode Live => PageBuilderMode.Live;
        public PageBuilderMode Edit => PageBuilderMode.Edit;
    }
}
