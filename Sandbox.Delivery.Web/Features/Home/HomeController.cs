using System.Threading.Tasks;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;

namespace Sandbox.Delivery.Web.Features.Home
{
    [DynamicOutputCache]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            Guard.Against.Null(mediator, nameof(mediator));

            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Head | HttpVerbs.Get)]
        [PageTypeRoute("CMS.Root", HomePage.CLASS_NAME)]
        public async Task<ActionResult> Home(string requestPath)
        {
            var option = await mediator.Send(new HomePageRequest(requestPath));

            if (option.HasNoValue)
            {
                return HttpNotFound();
            }

            return View(option.Value);
        }
    }
}
