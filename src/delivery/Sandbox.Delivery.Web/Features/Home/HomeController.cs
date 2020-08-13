using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Delivery.Web.Infrastructure.Requests;

namespace Sandbox.Delivery.Web.Features.Home
{
    [DynamicOutputCache]
    [Route("/")]
    public class HomeController : BaseController
    {
        public HomeController(IMediator mediator) : base(mediator) { }

        [HttpHead]
        [HttpGet]
        public Task<IActionResult> Home(string requestPath = "/") =>
            Process<HomePageRequest, HomePageViewModel>(new HomePageRequest(requestPath));
    }
}
