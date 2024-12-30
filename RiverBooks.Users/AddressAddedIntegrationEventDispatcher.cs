using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RiverBooks.Users.Contracts;

namespace RiverBooks.Users;

internal class AddressAddedIntegrationEventDispatcher(IMediator mediator)
    : INotificationHandler<AddressAddedDomainEvent>
{
    public async Task Handle(AddressAddedDomainEvent notification, CancellationToken ct)
    {
        var @event = new AddressAddedIntegrationEvent(new AddressDto(
            notification.Address.Id,
            notification.Address.Street1,
            notification.Address.Street2,
            notification.Address.City,
            notification.Address.State,
            notification.Address.PostalCode,
            notification.Address.Country
            ));
        
        await mediator.Publish(@event, ct);
    }
}