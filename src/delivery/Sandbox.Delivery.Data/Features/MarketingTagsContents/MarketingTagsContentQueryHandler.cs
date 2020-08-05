using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.MarketingTagsContents;

using static Sandbox.Data.Kentico.Infrastructure.Queries.ContextCacheKeysCreator;

namespace Sandbox.Delivery.Data.Features.MarketingTagsContents
{
    public class MarketingTagsContentQueryHandler :
        IQueryHandler<MarketingTagsContentQuery, MarketingTagsQueryResponse>,
        IQueryCacheKeysCreator<MarketingTagsContentQuery, MarketingTagsQueryResponse>
    {
        private readonly IDocumentQueryContext context;

        public MarketingTagsContentQueryHandler(IDocumentQueryContext context)
        {
            Guard.Against.Null(context, nameof(context));

            this.context = context;
        }

        public async Task<Result<MarketingTagsQueryResponse>> Execute(MarketingTagsContentQuery query, CancellationToken token)
        {
            var response = await MarketingTagsContentProvider.GetMarketingTagsContents()
                .LatestVersion(context.IsPreviewEnabled)
                .Published(!context.IsPreviewEnabled)
                .OnSite(context.SiteName)
                .CombineWithDefaultCulture()
                .Columns(
                    nameof(MarketingTagsContent.MarketingTagsContentHeaderTags),
                    nameof(MarketingTagsContent.MarketingTagsContentAfterBodyStartTags),
                    nameof(MarketingTagsContent.MarketingTagsContentBeforeBodyEndTags),
                    nameof(MarketingTagsContent.MarketingTagsContentPageTitleSuffix),
                    nameof(MarketingTagsContent.MarketingTagsContentDefaultPageDescription),
                    nameof(MarketingTagsContent.MarketingTagsContentDefaultOpenGraphImageUrl),
                    nameof(MarketingTagsContent.MarketingTagsContentDefaultTwitterSite)
                )
                .TopN(1)
                .SelectSingle(content => new MarketingTagsQueryResponse(
                    content.Fields.HeaderTags,
                    content.Fields.AfterBodyStartTags,
                    content.Fields.BeforeBodyEndTags,
                    content.Fields.PageTitleSuffix,
                    content.Fields.DefaultPageDescription,
                    content.Fields.DefaultOpenGraphImageUrl,
                    content.Fields.DefaultTwitterSite), token);

            if (response is null)
            {
                return Result.Failure<MarketingTagsQueryResponse>($"Could not find any {nameof(MarketingTagsContent)} documents");
            }

            return Result.Success(response);
        }

        public string[] DependencyKeys(MarketingTagsContentQuery query, Result<MarketingTagsQueryResponse> result) =>
            new[]
            {
                FluentCacheKey.ForPages().OfSite(context.SiteName).OfClassName(MarketingTagsContent.CLASS_NAME)
            };

        public object[] ItemNameParts(MarketingTagsContentQuery query) => NamePartsFromQuery(context, nameof(MarketingTagsContentQuery));
    }
}
