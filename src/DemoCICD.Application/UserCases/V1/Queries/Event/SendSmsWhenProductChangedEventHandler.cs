
using DemoCICD.Contract.Abstractions.Message;
using DemoCICD.Contract.Services.V1.Product;

namespace DemoCICD.Application.UserCases.V1.Queries.Event;

internal class SendSmsWhenProductChangedEventHandler
    : IDomainEventHandler<DomainEvent.ProductCreated>,
    IDomainEventHandler<DomainEvent.ProductDeleted>
{
    public async Task Handle(DomainEvent.ProductCreated notification, CancellationToken cancellationToken)
    {
        SendSms();
        await Task.Delay(100000);
    }

    public async Task Handle(DomainEvent.ProductDeleted notification, CancellationToken cancellationToken)
    {
        SendSms();
        await Task.Delay(100000);
    }

    private void SendSms()
    {

    }
}

