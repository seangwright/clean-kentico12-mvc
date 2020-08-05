using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.DocumentEngine;

namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    public static class DocumentQueryExtensions
    {
        /// <summary>
        /// Returns the <see cref="DocumentQuery"/> filtered by the latest publish version on the current site
        /// combined with the default culture, unless the request is in preview mode, which will return the latest document
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="query"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DocumentQuery<TNode> GetLatestSiteDocuments<TNode>(this DocumentQuery<TNode> query, IDocumentQueryContext context)
            where TNode : TreeNode, new()
            => query.LatestVersion(context.IsPreviewEnabled)
                .Published(!context.IsPreviewEnabled)
                .OnSite(context.SiteName)
                .CombineWithDefaultCulture();

        public static MultiDocumentQuery GetLatestSiteDocuments(this MultiDocumentQuery query, IDocumentQueryContext context)
            => query.LatestVersion(context.IsPreviewEnabled)
                .Published(!context.IsPreviewEnabled)
                .OnSite(context.SiteName)
                .CombineWithDefaultCulture();

        public static async Task<TDocument> FirstOrDefault<TDocument>(this DocumentQuery<TDocument> query, CancellationToken token)
            where TDocument : TreeNode, new()
            => (await query.GetEnumerableTypedResultAsync(cancellationToken: token)).FirstOrDefault();

        public static async Task<TreeNode> FirstOrDefault(this MultiDocumentQuery query, CancellationToken token)
            => (await query.GetEnumerableTypedResultAsync(cancellationToken: token)).FirstOrDefault();

        public static async Task<TDocument> Single<TDocument>(this DocumentQuery<TDocument> query, CancellationToken token)
            where TDocument : TreeNode, new()
            => (await query.GetEnumerableTypedResultAsync(cancellationToken: token)).Single();

        public static async Task<IEnumerable<TResult>> Select<TDocument, TResult>(this DocumentQuery<TDocument> query, Func<TDocument, TResult> selector, CancellationToken token)
            where TDocument : TreeNode, new()
            => (await query.GetEnumerableTypedResultAsync(cancellationToken: token)).Select(selector);

        public static async Task<TResult> SelectSingle<TDocument, TResult>(this DocumentQuery<TDocument> query, Func<TDocument, TResult> selector, CancellationToken token)
            where TDocument : TreeNode, new()
            => (await query.GetEnumerableTypedResultAsync(cancellationToken: token)).Select(selector).FirstOrDefault();

    }
}
