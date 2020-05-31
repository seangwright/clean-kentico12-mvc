using Ardalis.GuardClauses;

namespace Sandbox.Delivery.Web.Features.PageMetas
{
    public interface IPageMetaService<TPageMeta> where TPageMeta : IPageMeta
    {
        TPageMeta Get();
        void Set(TPageMeta siteMeta);
    }

    /// <summary>
    /// Default implementation of <see cref="IPageMetaService{TPageMeta}"/> which uses a <see cref="IPageMetaStandardizer{TPageMeta}"/>
    /// to ensure all <see cref="TPageMeta"/> returned have standard values
    /// </summary>
    /// <typeparam name="TPageMeta"></typeparam>
    public class PageMetaService<TPageMeta> : IPageMetaService<TPageMeta> where TPageMeta : class, IPageMeta
    {
        private readonly IPageMetaStandardizer<TPageMeta> siteMetaStandardizer;

        private TPageMeta currentPageMeta;

        /// <summary>
        /// Instantiates a new instance, requiring a <see cref="IPageMetaStandardizer{TPageMeta}"/>
        /// </summary>
        /// <param name="siteMetaStandardizer">Type which can standardize the <see cref="TPageMeta"/> returned by the service</param>
        public PageMetaService(IPageMetaStandardizer<TPageMeta> siteMetaStandardizer)
        {
            Guard.Against.Null(siteMetaStandardizer, nameof(siteMetaStandardizer));

            this.siteMetaStandardizer = siteMetaStandardizer;
        }

        public TPageMeta Get() => siteMetaStandardizer.Standardize(currentPageMeta)
;
        public void Set(TPageMeta siteMeta)
        {
            Guard.Against.Null(siteMeta, nameof(siteMeta));

            currentPageMeta = siteMeta;
        }
    }
}
