using System.Threading.Tasks;
using System.Web.Mvc;
using Ardalis.GuardClauses;
using CSharpFunctionalExtensions;
using MediatR;

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

        protected async Task<ActionResult> Process<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<Maybe<TResponse>> =>
            (await Mediator.Send(request))
            .Match<ActionResult, TResponse>(
                vm => View(vm),
                () => HttpNotFound());
    }
}
