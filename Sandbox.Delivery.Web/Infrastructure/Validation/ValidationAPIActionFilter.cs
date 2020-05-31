using System;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ardalis.GuardClauses;
using Autofac.Integration.WebApi;
using Serilog;

namespace Sandbox.Delivery.Web.Infrastructure.Validation
{
    public class ValidationAPIActionFilter : IAutofacActionFilter
    {
        private readonly JsonMediaTypeFormatter mediaTypeFormatter;
        private readonly ILogger logger;

        public ValidationAPIActionFilter(JsonMediaTypeFormatter mediaTypeFormatter, ILogger logger)
        {
            Guard.Against.Null(mediaTypeFormatter, nameof(mediaTypeFormatter));
            Guard.Against.Null(logger, nameof(logger));

            this.mediaTypeFormatter = mediaTypeFormatter;
            this.logger = logger;
        }

        public async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            /*
             * Kentico has its own API endpoints that have models which can be invalid
             * (PageBuilder MediaFilesSelector)
             * We don't want to handle those requests
             */
            if (actionContext.ControllerContext.Controller.GetType().FullName.Contains("Kentico"))
            {
                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                var errorId = Guid.NewGuid();

                var result = new ValidationErrorResult(actionContext.Request, actionContext.ModelState, mediaTypeFormatter, errorId);

                logger.Error("API Bad Request with {@result}", result);

                actionContext.Response = await result.ExecuteAsync(cancellationToken);
            }
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
