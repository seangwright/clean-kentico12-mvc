using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Sandbox.Core.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<Result<TResponse>> Execute(TQuery query, CancellationToken token);
    }

    public interface IQueryHandlerSync<in TQuery, TResponse> where TQuery : IQuerySync<TResponse>
    {
        Result<TResponse> Execute(TQuery query);
    }
}
