using System;
using System.Collections.Generic;
using Ardalis.Result;
using MediatR;

namespace RiverBooks.OrderProcessing.Contracts;

public class CreateOrderCommand : IRequest<Result<CreateOrderResponse>>
{
    public Guid UserId { get; set; }
    public Guid ShippingAddressId { get; set; }
    public Guid BillingAddressId { get; set; }
    public List<OrderItemDetail> OrderItems { get; set; }
}

public class OrderItemDetail
{
    public Guid BookId { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}