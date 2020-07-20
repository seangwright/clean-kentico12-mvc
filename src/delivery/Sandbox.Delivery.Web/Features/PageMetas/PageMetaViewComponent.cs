using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Features.PageMetas
{
    public class PageMetaViewComponent : ViewComponent
    {
        private readonly IPageMetaService<PageMeta> metaService;

        public PageMetaViewComponent(IPageMetaService<PageMeta> metaService)
        {
            Guard.Against.Null(metaService, nameof(metaService));

            this.metaService = metaService;
        }

        public IViewComponentResult Invoke() => View(metaService.Get());
    }
}
