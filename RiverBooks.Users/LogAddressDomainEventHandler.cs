using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Users;

internal class LogAddressDomainEventHandler(ILogger<LogAddressDomainEventHandler> logger) 
    : INotificationHandler<AddressAddedDomainEvent>
{
    public Task Handle(AddressAddedDomainEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Address: {addressId} added for user: {userId}",
            notification.Address.Id, notification.Address.UserId);
        
        return Task.CompletedTask;
    }
}