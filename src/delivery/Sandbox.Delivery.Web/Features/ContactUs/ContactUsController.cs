using System.Threading.Tasks;
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
        public Task<IActionResult> ContactUs(string requestPath) =>
            Process<ContactUsPageRequest, ContactUsPageViewModel>(new ContactUsPageRequest(requestPath));
    }
}
