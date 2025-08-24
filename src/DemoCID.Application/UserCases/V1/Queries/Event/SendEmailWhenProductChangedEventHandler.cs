
using DemoCICD.Contract.Abstractions.Message;

namespace DemoCICD.Application.UserCases.V1.Queries.Event;

internal class SendEmailWhenProductChangedEventHandler
    : IDomainEventHandler<DomainEvent.ProductCreated>,
    IDomainEventHandler<DomainEvent.ProductDeleted>
{
    public async Task Handle(DomainEvent.ProductCreated notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(100000);
    }

    public async Task Handle(DomainEvent.ProductDeleted notification, CancellationToken cancellationToken)
    {
        SendEmail();
        await Task.Delay(100000);
    }

    private void SendEmail()
    {

    }
}
