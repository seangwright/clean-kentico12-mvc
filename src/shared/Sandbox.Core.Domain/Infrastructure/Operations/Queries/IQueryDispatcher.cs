using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Sandbox.Core.Domain.Intrastructure.Operations.Queries
{
    public interface IQueryDispatcher
    {
        Task<Result<TResponse>> Dispatch<TResponse>(IQuery<TResponse> query, CancellationToken token = default);
        Result<TResponse> Dispatch<TResponse>(IQuerySync<TResponse> query);
    }
}
