using System;
using System.Web.Mvc;
using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Infrastructure.PageMeta
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PageMetaActionFilterAttribute : ActionFilterAttribute
    {
        public IPageMetaService<PageMeta> SiteMetaService { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Guard.Against.Null(SiteMetaService, nameof(SiteMetaService));

            filterContext.Controller.ViewBag.SiteMeta = SiteMetaService.Get();

            base.OnActionExecuted(filterContext);
        }
    }
}
