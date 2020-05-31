using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Sandbox.Delivery.Web.Infrastructure.ErrorHandling
{
    public class GlobalApiExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var exception = context.Exception;

            return Task.CompletedTask;
        }
    }
}
