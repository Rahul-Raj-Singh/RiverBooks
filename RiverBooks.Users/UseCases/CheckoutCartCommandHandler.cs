using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.OrderProcessing.Contracts;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.UseCases;

internal class CartCheckoutCommand : IRequest<Result<Guid>>
{
    public string Email { get; set; }
    public Guid BillingAddressId { get; set; }
    public Guid ShippingAddressId { get; set; }
}


internal class CartCheckoutCommandHandler(IApplicationUserRepository repository, IMediator mediator) 
    : IRequestHandler<CartCheckoutCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CartCheckoutCommand req, CancellationToken ct)
    {
        var existingUser = await repository.GetUserWithCartByEmailAsync(req.Email);

        if (existingUser is null) return Result.Unauthorized();

        // use mediatr to send order create command to order processing module
        var cmd = new CreateOrderCommand
        {
            UserId = Guid.Parse(existingUser.Id),
            ShippingAddressId = req.ShippingAddressId,
            BillingAddressId = req.BillingAddressId,
            OrderItems = existingUser.CartItems.Select(x => new OrderItemDetail 
            {
                BookId = x.BookId, Description = x.Description, Quantity = x.Quantity, UnitPrice = x.UnitPrice
            }).ToList()
        };
        var result = await mediator.Send(cmd, ct);

        if (!result.IsSuccess)
        {
            return Result.Error("Something went wrong placing order!");
        }

        // clear cart
        repository.DeleteCartItems(existingUser.CartItems);
        existingUser.ClearCart();

        await repository.SaveChangesAsync();

        return result.Value.OrderId;
    }
}