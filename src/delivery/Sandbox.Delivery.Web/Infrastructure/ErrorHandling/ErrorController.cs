using System.Diagnostics;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.ErrorHandling
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
    public class ErrorController : Controller
    {
        private readonly IExceptionHandlerPathFeature feature;

        public ErrorController(IExceptionHandlerPathFeature feature)
        {
            Guard.Against.Null(feature, nameof(feature));

            this.feature = feature;
        }

        [Route("401")]
        public IActionResult Error401() => View(BuildModel());
        [Route("403")]
        public IActionResult Error403() => View(BuildModel());
        [Route("404")]
        public IActionResult Error404() => View(BuildModel());
        [Route("500")]
        public IActionResult Error500() => View(BuildModel());


        private ErrorViewModel BuildModel() => new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            RequestPath = feature.Path
        };
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
    }
}
