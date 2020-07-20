using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Features.Navigation
{
    public class HeaderNavViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("~/Features/Navigation/Header.cshtml", new HeaderNavViewModel());
    }

    public class HeaderNavViewModel
    {

    }
}
