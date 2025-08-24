using DemoCICD.Contract.Shared;
using MediatR;

namespace DemoCICD.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
