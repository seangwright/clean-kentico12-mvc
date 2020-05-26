using Ardalis.GuardClauses;

namespace System.Web.Mvc
{
    /// <summary>
    /// Marks the given <see cref="System.Web.Mvc.Controller"/> method as a valid
    /// action method to handle HTTP requests for the specified <see cref="ClassNames" />
    /// matching <see cref="CMS.DocumentEngine.TreeNode.ClassName"/> for custom Page Types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PageTypeRouteAttribute : Attribute
    {
        public PageTypeRouteAttribute(params string[] classNames)
        {
            Guard.Against.NullOrEmpty(classNames, nameof(classNames));

            foreach (string className in classNames)
            {
                Guard.Against.NullOrWhiteSpace(className, nameof(className));
            }

            ClassNames = classNames;
        }

        /// <summary>
        /// <see cref="CMS.DocumentEngine.TreeNode.ClassName"/> values.
        /// </summary>
        public string[] ClassNames { get; }
    }
}
