using DemoCICD.Domain.Shared;
using MediatR;

namespace DemoCID.Application.Abstractions.Message;

public interface ICommand : IRequest<Result>
{

}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
