using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.Endpoints;

internal class ListCartItemEndpoint(IMediator mediator) : EndpointWithoutRequest<List<CartItemDto>>
{
    public override void Configure()
    {
        Get("/cart");
        Claims("Email");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var email = User.FindFirstValue("Email");

        var query = new ListCartItemQuery {Email = email};
        
        var result = await mediator.Send(query, ct);

        await SendOkAsync(result.Value, ct);
    }
}