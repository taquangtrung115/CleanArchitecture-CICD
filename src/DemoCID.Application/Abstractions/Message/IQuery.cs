

using DemoCICD.Domain.Shared;
using MediatR;

namespace DemoCID.Application.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

