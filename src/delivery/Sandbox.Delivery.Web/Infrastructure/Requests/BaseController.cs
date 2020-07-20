using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Sandbox.Delivery.Web.Infrastructure.Requests
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;

        public BaseController(IMediator mediator)
        {
            Guard.Against.Null(mediator, nameof(mediator));

            Mediator = mediator;
        }

        protected async Task<IActionResult> Process<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<Maybe<TResponse>> =>
            (await Mediator.Send(request))
            .Match<ActionResult, TResponse>(
                vm => View(vm),
                () => NotFound());
    }
}
