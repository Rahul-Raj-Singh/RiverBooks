using System;
using Ardalis.GuardClauses;

namespace RiverBooks.Users;

internal class CartItem
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public string UserId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public CartItem(Guid id, Guid bookId, string userId, string description, int quantity, decimal unitPrice)
    {
        Id = Guard.Against.Default(id);
        BookId = Guard.Against.Default(bookId);
        UserId = Guard.Against.Default(userId);
        Description = Guard.Against.NullOrWhiteSpace(description);
        Quantity = Guard.Against.Negative(quantity);
        UnitPrice = Guard.Against.Negative(unitPrice);
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = Guard.Against.Negative(quantity);
    }
    public void UpdateDescription(string description)
    {
        Description = Guard.Against.NullOrWhiteSpace(description);
    }
    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = Guard.Against.Negative(unitPrice);
    }
}