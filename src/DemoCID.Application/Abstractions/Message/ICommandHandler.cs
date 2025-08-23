using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoCICD.Domain.Shared;
using MediatR;

namespace DemoCID.Application.Abstractions.Message;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand
{

}

public interface ICommandHandler<TCommand, TResponse> 
    : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{

}
