using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Features.Navigation
{
    public class FooterNavViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("~/Features/Navigation/Footer.cshtml", new FooterNavViewModel());
    }

    public class FooterNavViewModel
    {

    }
}
