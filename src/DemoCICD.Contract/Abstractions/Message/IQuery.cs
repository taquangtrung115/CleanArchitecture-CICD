using DemoCICD.Contract.Shared;
using MediatR;

namespace DemoCICD.Contract.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

