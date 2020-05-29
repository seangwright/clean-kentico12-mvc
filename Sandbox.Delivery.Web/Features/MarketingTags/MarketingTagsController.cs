using System.Threading.Tasks;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using MediatR;

namespace Sandbox.Delivery.Web.Features.MarketingTags
{
    public class MarketingTagsController : Controller
    {
        private readonly IMediator mediator;

        public MarketingTagsController(IMediator mediator)
        {
            Guard.Against.Null(mediator, nameof(mediator));

            this.mediator = mediator;
        }

        [ChildActionOnly]
        public async Task<ActionResult> Header() =>
            PartialView(await mediator.Send(new MarketingTagsHeaderRequest()));

        [ChildActionOnly]
        public async Task<ActionResult> AfterBodyStart() =>
            PartialView(await mediator.Send(new MarketingTagsAfterBodyStartRequest()));

        [ChildActionOnly]
        public async Task<ActionResult> BeforeBodyEnd() =>
            PartialView(await mediator.Send(new MarketingTagsBeforeBodyEndRequest()));
    }
}
