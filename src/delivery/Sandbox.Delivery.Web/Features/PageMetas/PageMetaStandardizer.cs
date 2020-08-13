using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;
using Sandbox.Delivery.Web.Infrastructure.Core;

namespace Sandbox.Delivery.Web.Features.PageMetas
{
    public interface IPageMetaStandardizer<TSiteMeta> where TSiteMeta : IPageMeta
    {
        Task<TSiteMeta> Standardize(TSiteMeta pageMeta, CancellationToken token);
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

        public async Task<PageMeta> Standardize(PageMeta pageMeta, CancellationToken token)
        {
            if (pageMeta is null)
            {
                pageMeta = new PageMeta(context.DocumentPageTitle);
                pageMeta.SetDescription(context.DocumentPageDescription);
            }

            var result = await dispatcher.Dispatch(new MarketingTagsContentQuery(), token);

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
