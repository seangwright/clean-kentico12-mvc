﻿using CMS.DocumentEngine.Types.Sandbox;
using CSharpFunctionalExtensions;
using FluentCacheKeys;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;
using Sandbox.Data.Kentico.Infrastructure.Queries;
using Sandbox.Delivery.Core.Features.ContactUsPages;

namespace Sandbox.Delivery.Data.Features.ContactUsPages
{
    public class ContactUsPageQueryHandler : DocumentContextQueryHandler<ContactUsPage>,
        IQueryHandlerSync<ContactUsPageQuery, ContactUsPageQueryResponse>,
        IQuerySyncCacheKeysCreator<ContactUsPageQuery, ContactUsPageQueryResponse>
    {
        private readonly IDocumentQueryContext context;

        public ContactUsPageQueryHandler(IDocumentQueryContext context) : base(context) { }

        public Result<ContactUsPageQueryResponse> Execute(ContactUsPageQuery query)
        {
            var result = GetFirstPageWithNodeAliasPath(ContactUsPageProvider.GetContactUsPages(), query);

            if (result.IsFailure)
            {
                return Result.Failure<ContactUsPageQueryResponse>(result.Error);
            }

            var node = result.Value;

            return Result.Success(new ContactUsPageQueryResponse("test"));
        }

        public string[] DependencyKeys(ContactUsPageQuery query, Result<ContactUsPageQueryResponse> result) =>
            new[] { FluentCacheKey.ForPage().OfSite(context.SiteName).WithAliasPath(query.NodeAliasPath) };

        public object[] ItemNameParts(ContactUsPageQuery query) => ItemNameParts(nameof(ContactUsPageQuery), query.NodeAliasPath);
    }
}
