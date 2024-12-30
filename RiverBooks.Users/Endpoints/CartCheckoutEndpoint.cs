using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.Endpoints;

internal class CartCheckoutEndpoint(IMediator mediator) : Endpoint<CheckoutRequest, CheckoutResponse>
{
    public override void Configure()
    {
        Post("/cart/checkout");
        Claims("Email");
    }

    public override async Task HandleAsync(CheckoutRequest req, CancellationToken ct)
    {
        var email = User.FindFirstValue("Email");

        var cmd = new CartCheckoutCommand 
            {Email = email, BillingAddressId = req.BillingAddressId, ShippingAddressId = req.ShippingAddressId};
        
        var result = await mediator.Send(cmd, ct);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(ct);
        else
            await SendOkAsync(new CheckoutResponse {NewOrderId = result.Value}, ct);
    }
}

internal class CheckoutRequest
{
    public Guid BillingAddressId { get; set; }
    public Guid ShippingAddressId { get; set; }
}
internal class CheckoutResponse
{
    public Guid NewOrderId { get; set; }
}