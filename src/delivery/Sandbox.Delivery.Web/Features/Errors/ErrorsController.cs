using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Features.Errors
{
    [ApiController]
    [Route("error")]
    public class ApiErrorController : ControllerBase
    {
        [Route("")]
        public IActionResult Error() => Problem();
    }

    [AllowAnonymous]
    [Route("error")]
    public class ErrorsController : Controller
    {
        [Route("401")]
        public IActionResult NotAuthenticated() => View(BuildModel());
        [Route("403")]
        public IActionResult NotAuthorized() => View(BuildModel());
        [Route("404")]
        public IActionResult PageNotFound() => View(BuildModel());
        [Route("500")]
        public IActionResult ServerError() => View(BuildModel());


        private ErrorViewModel BuildModel() => new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            RequestPath = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Path ?? ""
        };
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
    }
}
