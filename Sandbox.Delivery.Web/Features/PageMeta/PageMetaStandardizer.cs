using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagContents;
using Sandbox.Delivery.Web.Infrastructure.Contexts;

namespace Sandbox.Delivery.Web.Features.PageMeta
{
    public interface IPageMetaStandardizer<TSiteMeta> where TSiteMeta : IPageMeta
    {
        TSiteMeta Standardize(TSiteMeta pageMeta);
    }

    public class PageMetaStandardizer : IPageMetaStandardizer<PageMeta>
    {
        private readonly IQueryDispatcher dispatcher;
        private readonly IDocumentContext context;

        public PageMetaStandardizer(IQueryDispatcher dispatcher, IDocumentContext context)
        {
            Guard.Against.Null(dispatcher, nameof(dispatcher));
            Guard.Against.Null(context, nameof(context));

            this.dispatcher = dispatcher;
            this.context = context;
        }

        public PageMeta Standardize(PageMeta pageMeta)
        {
            if (pageMeta is null)
            {
                pageMeta = new PageMeta(context.DocumentPageTitle);
                pageMeta.SetDescription(context.DocumentPageDescription);
            }

            var result = dispatcher.Dispatch(new MarketingTagsContentQuery());

            if (result.IsFailure)
            {
                return pageMeta;
            }

            var response = result.Value;

            var defaultMeta = new Dictionary<string, string>
            {
                { PageMeta.OpenGraphImageUrl, response.DefaultOpenGraphImageUrl },
                { PageMeta.Description, response.DefaultPageDescription },
                { PageMeta.TwitterSite, response.DefaultTwitterSite }
            };

            if (pageMeta is null)
            {
                return new PageMeta(response.PageTitleSuffix, defaultMeta);
            }

            foreach (var meta in defaultMeta)
            {
                if (!pageMeta.Metas.ContainsKey(meta.Key))
                {
                    pageMeta.AddMeta(meta.Key, meta.Value);
                }
            }

            string title = string.IsNullOrWhiteSpace(pageMeta.Title)
                ? response.PageTitleSuffix
                : $"{pageMeta.Title} - {response.PageTitleSuffix}";

            var standardizedMetas = pageMeta.Metas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var newPageMeta = new PageMeta(title, standardizedMetas);

            if (pageMeta.CanonicalUrl is object)
            {
                newPageMeta.SetCanonicalUrl(pageMeta.CanonicalUrl);
            }

            return newPageMeta;
        }
    }
}
