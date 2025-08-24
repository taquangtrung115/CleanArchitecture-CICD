

using MediatR;

namespace DemoCICD.Contract.Abstractions.Message;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }
}
