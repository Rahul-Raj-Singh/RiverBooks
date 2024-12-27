using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.UseCases;

internal class ListCartItemQuery : IRequest<Result<List<CartItemDto>>>
{
    public string Email { get; set; }
}


internal class ListCartItemQueryHandler(IApplicationUserRepository repository) 
    : IRequestHandler<ListCartItemQuery ,Result<List<CartItemDto>>>
{
    public async Task<Result<List<CartItemDto>>> Handle(ListCartItemQuery req, CancellationToken ct)
    {
        var cartItems = await repository.GetUserWithCartByEmailAsync(req.Email);

        return cartItems.CartItems.Select(x => new CartItemDto
        {
            Id = x.Id,
            BookId = x.BookId,
            UserId = x.UserId,
            Description = x.Description,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice
        }).ToList();
    }
}

internal class CartItemDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string UserId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}