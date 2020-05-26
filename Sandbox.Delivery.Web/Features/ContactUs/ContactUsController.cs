using System.Threading.Tasks;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;

namespace Sandbox.Delivery.Web.Features.ContactUs
{
    [DynamicOutputCache]
    public class ContactUsController : Controller
    {
        private readonly IMediator mediator;

        public ContactUsController(IMediator mediator)
        {
            Guard.Against.Null(mediator, nameof(mediator));

            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Head | HttpVerbs.Get)]
        [PageTypeRoute(ContactUsPage.CLASS_NAME)]
        public async Task<ActionResult> Home(string requestPath)
        {
            var option = await mediator.Send(new ContactUsPageRequest(requestPath));

            if (option.HasNoValue)
            {
                return HttpNotFound();
            }

            return View(option.Value);
        }
    }
}
