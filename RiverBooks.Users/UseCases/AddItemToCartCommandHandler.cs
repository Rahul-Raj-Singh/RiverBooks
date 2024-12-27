using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.UseCases;

internal class AddItemToCartCommand : IRequest<Result>
{
    public string Email { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}

internal class AddItemToCartCommandHandler(IApplicationUserRepository repository, IMediator mediator) 
    : IRequestHandler<AddItemToCartCommand, Result>
{
    public async Task<Result> Handle(AddItemToCartCommand req, CancellationToken ct)
    {
        var existingUser = await repository.GetUserWithCartByEmailAsync(req.Email);

        if (existingUser is null) return Result.Unauthorized();

        // use mediatr to fetch book details from books module
        var query = new GetBookDetailsQuery { BookId = req.BookId };
        var result = await mediator.Send(query, ct);

        if (result.Status == ResultStatus.NotFound) 
            return Result.NotFound("Book not found with id: " + req.BookId);
        
        var newCartItem = new CartItem(
            Guid.NewGuid(), 
            req.BookId, 
            existingUser.Id, 
            result.Value.Title + " by " + result.Value.Author, 
            req.Quantity, 
            result.Value.Price);
        existingUser.AddItemToCart(newCartItem);

        await repository.SaveChangesAsync();

        return Result.Success();
    }
}