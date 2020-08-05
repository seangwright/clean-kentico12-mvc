using CSharpFunctionalExtensions;
using Sandbox.Core.Domain.Intrastructure.Operations.Queries;

namespace Sandbox.Data.Kentico.Infrastructure.Queries
{
    /// <summary>
    /// A contract specifying how to generate both a cache item name and cache dependency keys for a specific
    /// <see cref="TQuery"/>, using both the <see cref="TQuery"/> instance and the <see cref="Result{TResponse}" />
    /// of executing the query
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQueryCacheKeysCreator<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Returns an array of valid cache dependency keys for the given query and result set.
        /// See https://docs.kentico.com/k13/configuring-kentico/configuring-caching/setting-cache-dependencies for dependency key patterns.
        /// </summary>
        /// <example>
        /// <code>
        /// string[] DependencyKeys(ExampleQuery query, Result<Guid> result)
        /// {
        ///     var keys = new List<string>()
        ///     {
        ///         $"documentid|{query.Id}",
        ///     };
        ///     
        ///     if (result.IsFailure)
        ///     {
        ///         return keys.ToArray();
        ///     }
        ///     
        ///     keys.Add($"attachment|{result.Value}");
        ///     
        ///     return keys.ToArray();
        /// }   
        /// </code>
        /// </example>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string[] DependencyKeys(TQuery query, Result<TResponse> result);

        /// <summary>
        /// Returns an array of all parts needed to uniquely identify the cache item generated for
        /// the result of this query
        /// </summary>
        /// <example>
        /// <code>
        /// object[] ItemNameParts(ExampleQuery query) =>
        ///     new object[]
        ///     {
        ///         nameof(ExampleQuery),
        ///         query.Id,
        ///         query.Count
        ///     };
        /// </code>
        /// </example>
        /// <param name="query"></param>
        /// <returns></returns>
        object[] ItemNameParts(TQuery query);
    }

    /// <summary>
    /// A contract specifying how to generate both a cache item name and cache dependency keys for a specific
    /// <see cref="TQuery"/>, using both the <see cref="TQuery"/> instance and the <see cref="Result{TResponse}" />
    /// of executing the query
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQuerySyncCacheKeysCreator<TQuery, TResponse> where TQuery : IQuerySync<TResponse>
    {
        /// <summary>
        /// Returns an array of valid cache dependency keys for the given query and result set.
        /// See https://docs.kentico.com/k13/configuring-kentico/configuring-caching/setting-cache-dependencies for dependency key patterns.
        /// </summary>
        /// <example>
        /// <code>
        /// string[] DependencyKeys(ExampleQuery query, Result<Guid> result)
        /// {
        ///     var keys = new List<string>()
        ///     {
        ///         $"documentid|{query.Id}",
        ///     };
        ///     
        ///     if (result.IsFailure)
        ///     {
        ///         return keys.ToArray();
        ///     }
        ///     
        ///     keys.Add($"attachment|{result.Value}");
        ///     
        ///     return keys.ToArray();
        /// }   
        /// </code>
        /// </example>
        /// <param name="query"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string[] DependencyKeys(TQuery query, Result<TResponse> result);

        /// <summary>
        /// Returns an array of all parts needed to uniquely identify the cache item generated for
        /// the result of this query
        /// </summary>
        /// <example>
        /// <code>
        /// object[] ItemNameParts(ExampleQuery query) =>
        ///     new object[]
        ///     {
        ///         nameof(ExampleQuery),
        ///         query.Id,
        ///         query.Count
        ///     };
        /// </code>
        /// </example>
        /// <param name="query"></param>
        /// <returns></returns>
        object[] ItemNameParts(TQuery query);
    }
}
