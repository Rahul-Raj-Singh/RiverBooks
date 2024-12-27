using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.Endpoints;

internal class AddItemToCartEndpoint(IMediator mediator) : Endpoint<AddItemToCartRequest>
{
    public override void Configure()
    {
        Post("/cart");
        Claims("Email");
    }

    public override async Task HandleAsync(AddItemToCartRequest req, CancellationToken ct)
    {
        var email = User.FindFirstValue("Email");

        var command = new AddItemToCartCommand {Email = email, BookId = req.BookId, Quantity = req.Quantity};
        
        var result = await mediator.Send(command, ct);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(ct);
        else if (result.Status == ResultStatus.NotFound)
            await SendAsync(result.Errors, (int)HttpStatusCode.NotFound, ct);
        else
            await SendOkAsync(ct);
    }
}

internal class AddItemToCartRequest
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}

internal class AddItemToCartRequestValidator : Validator<AddItemToCartRequest>
{
    public AddItemToCartRequestValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}