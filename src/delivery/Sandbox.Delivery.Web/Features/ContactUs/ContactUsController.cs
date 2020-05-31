using System.Threading.Tasks;
using System.Web.Mvc;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;
using Sandbox.Delivery.Web.Infrastructure.Requests;

namespace Sandbox.Delivery.Web.Features.ContactUs
{
    [DynamicOutputCache]
    public class ContactUsController : BaseController
    {
        public ContactUsController(IMediator mediator) : base(mediator) { }

        [AcceptVerbs(HttpVerbs.Head | HttpVerbs.Get)]
        [PageTypeRoute(ContactUsPage.CLASS_NAME)]
        public Task<ActionResult> ContactUs(string requestPath) =>
            Process<ContactUsPageRequest, ContactUsPageViewModel>(new ContactUsPageRequest(requestPath));
    }
}
