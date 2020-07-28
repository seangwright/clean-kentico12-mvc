using Ardalis.GuardClauses;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Http;

namespace Sandbox.Delivery.Web.Infrastructure.Core
{
    public interface IPreviewContext
    {
        bool IsPreviewEnabled { get; }
    }

    public class PreviewContext : IPreviewContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public PreviewContext(IHttpContextAccessor httpContextAccessor)
        {
            Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));

            this.httpContextAccessor = httpContextAccessor;
        }

        public bool IsPreviewEnabled => httpContextAccessor.HttpContext.Kentico().Preview().Enabled;
    }
}
