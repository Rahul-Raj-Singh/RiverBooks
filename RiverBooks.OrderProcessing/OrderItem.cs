using System;
using Ardalis.GuardClauses;

namespace RiverBooks.OrderProcessing;

internal class OrderItem
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid OrderId { get; private set; }
    public int Quantity { get; private set; }
    public string Description { get; private set; }
    public decimal UnitPrice { get; private set; }

    public OrderItem(Guid id, Guid bookId, int quantity, string description, decimal unitPrice)
    {
        Id = Guard.Against.Default(id);
        BookId = Guard.Against.Default(bookId);
        Quantity = Guard.Against.NegativeOrZero(quantity);
        Description = Guard.Against.NullOrWhiteSpace(description);
        UnitPrice = Guard.Against.Negative(unitPrice);
    }
}