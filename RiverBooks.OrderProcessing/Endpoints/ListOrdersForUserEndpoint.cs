using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FastEndpoints;
using MediatR;
using RiverBooks.OrderProcessing.Data;

namespace RiverBooks.OrderProcessing.Endpoints;

internal class ListOrdersForUserEndpoint(IMediator mediator) : EndpointWithoutRequest<List<OrderSummary>>
{
    public override void Configure()
    {
        Get("/orders");
        Claims("Email");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var email = User.FindFirstValue("Email");

        var query = new ListOrdersForUserQuery {Email = email};

        var result = await mediator.Send(query, ct);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(ct);
        else
            await SendOkAsync(result.Value, ct);
    }
}

internal class ListOrdersForUserQuery : IRequest<Result<List<OrderSummary>>>
{
    public string Email { get; set; }
}

internal class ListOrdersForUserQueryHandler(IOrderRepository repository) 
    : IRequestHandler<ListOrdersForUserQuery, Result<List<OrderSummary>>>
{
    public async Task<Result<List<OrderSummary>>> Handle(ListOrdersForUserQuery req, CancellationToken ct)
    {
        // TODO: filter for user using email
        var orders = await repository.GetAllOrdersAsync();

        return orders.Select(x => new OrderSummary
        {
            UserId = default,
            OrderId = x.Id,
            CreatedAtUtc = x.CreatedAtUtc,
            Total = x.OrderItems.Sum(y => y.UnitPrice * y.Quantity),
        }).ToList();
    }
}

internal class OrderSummary
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public decimal Total { get; set; }
}