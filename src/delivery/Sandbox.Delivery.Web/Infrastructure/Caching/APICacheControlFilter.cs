using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;

namespace Sandbox.Delivery.Web.Infrastructure.Caching
{
    /// <summary>
    /// Applies no-cache headers to all API endpoint responses
    /// </summary>
    public class APICacheControlFilter : IAutofacActionFilter
    {
        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken) => Task.CompletedTask;

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Response is null)
            {
                return Task.CompletedTask;
            }

            var headers = actionExecutedContext.Response.Headers;

            headers.CacheControl = new CacheControlHeaderValue { NoCache = true, NoStore = true, MustRevalidate = true };
            headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
            headers.Age = TimeSpan.Zero;

            return Task.CompletedTask;
        }
    }
}
