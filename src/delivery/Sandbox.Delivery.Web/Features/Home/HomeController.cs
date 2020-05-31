using System.Threading.Tasks;
using System.Web.Mvc;
using CMS.DocumentEngine.Types.Sandbox;
using MediatR;
using Sandbox.Delivery.Web.Infrastructure.Requests;

namespace Sandbox.Delivery.Web.Features.Home
{
    [DynamicOutputCache]
    public class HomeController : BaseController
    {
        public HomeController(IMediator mediator) : base(mediator) { }

        [AcceptVerbs(HttpVerbs.Head | HttpVerbs.Get)]
        [PageTypeRoute("CMS.Root", HomePage.CLASS_NAME)]
        public Task<ActionResult> Home(string requestPath) =>
            Process<HomePageRequest, HomePageViewModel>(new HomePageRequest(requestPath));
    }
}
