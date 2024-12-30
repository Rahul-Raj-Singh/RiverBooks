using System;
using System.Collections.Generic;

namespace RiverBooks.OrderProcessing;

internal class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Address ShippingAddress { get; private set; }
    public Address BillingAddress { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public static Order Create(Guid id, Guid userId, Address shippingAddress, Address billingAddress, List<OrderItem> orderItems)
    {
        var newOrder = new Order();
        newOrder.Id = id;
        newOrder.UserId = userId;
        newOrder.ShippingAddress = shippingAddress;
        newOrder.BillingAddress = billingAddress;
        orderItems.ForEach(oi => newOrder.AddOrderItem(oi));
        
        return newOrder;
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        _orderItems.Add(orderItem);
    }
    

}

internal record Address(
    string Street1, 
    string Street2, 
    string City, 
    string State, 
    string Country, 
    string PostalCode);