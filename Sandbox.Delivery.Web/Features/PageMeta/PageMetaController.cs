using System.Web.Mvc;
using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Features.PageMeta
{
    public class PageMetaController : Controller
    {
        private readonly IPageMetaService<PageMeta> metaService;

        public PageMetaController(IPageMetaService<PageMeta> metaService)
        {
            Guard.Against.Null(metaService, nameof(metaService));

            this.metaService = metaService;
        }

        [ChildActionOnly]
        public ActionResult PageMeta() =>
            PartialView(metaService.Get());
    }
}
