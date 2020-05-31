using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Sandbox.Delivery.Web.Infrastructure.Validation
{
    public class ValidationErrorResult : IHttpActionResult
    {
        private static readonly MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/json");

        private readonly HttpRequestMessage requestMessage;
        private readonly JsonMediaTypeFormatter formatter;

        public Guid Id { get; }
        public Dictionary<string, IEnumerable<string>> ValidationErrors { get; }

        public ValidationErrorResult(
            HttpRequestMessage requestMessage,
            ModelStateDictionary modelState,
            JsonMediaTypeFormatter formatter,
            Guid id)
        {
            this.requestMessage = requestMessage;
            this.formatter = formatter;

            Id = id;
            ValidationErrors = modelState.ToDictionary(ms => ms.Key, ms => ms.Value.Errors.Select(e => e.ErrorMessage));
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) =>
            Task.FromResult(requestMessage.CreateResponse(HttpStatusCode.BadRequest, new { Id, ValidationErrors }, formatter, mediaType));
    }
}
