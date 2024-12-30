using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.OrderProcessing.Data;

namespace RiverBooks.OrderProcessing.Integrations;

internal class CreateOrderCommandHandler(IOrderRepository repository, IOrderAddressCache addressCache) 
    : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    public async Task<Result<CreateOrderResponse>> Handle(CreateOrderCommand req, CancellationToken ct)
    {
        var orderItems = req.OrderItems.Select(x => new OrderItem(
            Guid.NewGuid(), 
            x.BookId, 
            x.Quantity, 
            x.Description, 
            x.UnitPrice)).ToList();
        
        var billingAddress = await addressCache.GetAddressByIdAsync(req.BillingAddressId);
        var shippingAddress = await addressCache.GetAddressByIdAsync(req.ShippingAddressId);
        
        var newOrder = Order.Create(Guid.NewGuid(), req.UserId, shippingAddress, billingAddress, orderItems);
        
        repository.AddOrder(newOrder);

        await repository.SaveChangesAsync();

        return new CreateOrderResponse {OrderId = newOrder.Id};
    }
}