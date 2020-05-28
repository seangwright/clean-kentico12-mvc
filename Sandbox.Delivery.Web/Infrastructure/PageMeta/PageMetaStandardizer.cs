using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Delivery.Web.Infrastructure.PageMeta
{
    public interface IPageMetaStandardizer<TSiteMeta> where TSiteMeta : IPageMeta
    {
        TSiteMeta Standardize(TSiteMeta siteMeta);
    }

    public class PageMetaStandardizer : IPageMetaStandardizer<PageMeta>
    {
        private readonly IQueryDispatcher dispatcher;

        public PageMetaStandardizer(IQueryDispatcher dispatcher)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));

            this.dispatcher = dispatcher;
        }

        public PageMeta Standardize(PageMeta siteMeta)
        {
            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return siteMeta;
            }

            var response = result.Value;

            var defaultMeta = new Dictionary<string, string>
            {
                { PageMeta.OpenGraphImageUrl, response.DefaultOpenGraphImageUrl },
                { PageMeta.Description, response.DefaultPageDescription },
                { PageMeta.TwitterSite, response.DefaultTwitterSite }
            };

            if (siteMeta is null)
            {
                return new PageMeta(response.PageTitleSuffix, defaultMeta);
            }

            foreach (var meta in defaultMeta)
            {
                if (!siteMeta.Metas.ContainsKey(meta.Key))
                {
                    siteMeta.AddMeta(meta.Key, meta.Value);
                }
            }

            string title = string.IsNullOrWhiteSpace(siteMeta.Title)
                ? response.PageTitleSuffix
                : $"{siteMeta.Title} - {response.PageTitleSuffix}";

            var standardizedMetas = siteMeta.Metas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var newPageMeta = new PageMeta(title, standardizedMetas);

            if (siteMeta.CanonicalUrl is object)
            {
                newPageMeta.SetCanonicalUrl(siteMeta.CanonicalUrl);
            }

            return newPageMeta;
        }
    }
}
