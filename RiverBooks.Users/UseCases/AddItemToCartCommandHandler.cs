using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.UseCases;

internal class AddItemToCartCommand : IRequest<Result>
{
    public string Email { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}

internal class AddItemToCartCommandHandler(IApplicationUserRepository repository) : IRequestHandler<AddItemToCartCommand, Result>
{
    public async Task<Result> Handle(AddItemToCartCommand req, CancellationToken ct)
    {
        var existingUser = await repository.GetUserWithCartByEmailAsync(req.Email);

        if (existingUser is null) return Result.Unauthorized();
        
        // TODO: how to fetch desc and price from book module
        var newCartItem = new CartItem(
            Guid.NewGuid(), 
            req.BookId, 
            existingUser.Id, 
            "Description", 
            req.Quantity, 
            1);
        existingUser.AddItemToCart(newCartItem);

        await repository.SaveChangesAsync();

        return Result.Success();
    }
}