using System.Threading.Tasks;
using System.Web.Mvc;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Delivery.Web.Infrastructure.Requests;

namespace Sandbox.Delivery.Web.Features.ContactUs
{
    [DynamicOutputCache]
    public class ContactUsController : BaseController
    {
        public ContactUsController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [HttpHead]
        [PageTypeRoute(ContactUsPage.CLASS_NAME)]
        public Task<IActionResult> ContactUs(string requestPath) =>
            Process<ContactUsPageRequest, ContactUsPageViewModel>(new ContactUsPageRequest(requestPath));
    }
}
