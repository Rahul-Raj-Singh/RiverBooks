using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RiverBooks.OrderProcessing.Data;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Integrations;

internal class AddressAddedIntegrationEventHandler(
    IOrderAddressCache addressCache, ILogger<AddressAddedIntegrationEventHandler> logger) 
    : INotificationHandler<AddressAddedIntegrationEvent>
{
    public async Task Handle(AddressAddedIntegrationEvent notification, CancellationToken ct)
    {
        var address = new Address(
            notification.Address.Street1, 
            notification.Address.Street2, 
            notification.Address.City, 
            notification.Address.State, 
            notification.Address.Country, 
            notification.Address.PostalCode);
        
        await addressCache.SetAddressByIdAsync(notification.Address.Id, address);
        
        logger.LogInformation($"[OrderProcessing Module] Added address with Id: {notification.Address.Id} in redis cache.");
    }
}