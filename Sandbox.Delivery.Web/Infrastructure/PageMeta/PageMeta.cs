using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Infrastructure.PageMeta
{
    public interface IPageMeta
    {
        string Title { get; }
        IReadOnlyDictionary<string, string> Metas { get; }

        void AddMeta(string key, string value);
    }

    public class PageMeta : IPageMeta
    {
        public const string OpenGraphImageUrl = "og:image";
        public const string Description = "description";
        public const string TwitterSite = "twitter:site";

        private readonly Dictionary<string, string> metas;

        public string Title { get; } = "";
        public string CanonicalUrl { get; protected set; } = "";

        public IReadOnlyDictionary<string, string> Metas => metas;

        public PageMeta(
            string title,
            Dictionary<string, string> metas = null)
        {
            Title = title ?? "";

            this.metas = metas is null
                ? new Dictionary<string, string>()
                : metas;
        }

        public PageMeta SetCanonicalUrl(string value)
        {
            CanonicalUrl = value ?? "";

            return this;
        }

        public PageMeta SetOpenGraphImageUrl(string value)
        {
            metas[OpenGraphImageUrl] = value ?? "";

            return this;
        }

        public PageMeta SetDescription(string value)
        {
            metas[Description] = value ?? "";

            return this;
        }

        public void AddMeta(string key, string value)
        {
            Guard.Against.NullOrWhiteSpace(key, nameof(key));

            metas[key] = value ?? "";
        }
    }
}
