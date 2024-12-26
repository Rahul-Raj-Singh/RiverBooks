using System;
using Ardalis.GuardClauses;

namespace RiverBooks.Books;

internal class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }

    public Book(Guid id, string title, string author, decimal price)
    {
        Id = Guard.Against.Default(id);
        Title = Guard.Against.NullOrWhiteSpace(title);
        Author = Guard.Against.NullOrWhiteSpace(author);
        Price = Guard.Against.Negative(price);
    }

    public void UpdateBookPrice(decimal price)
    {
        Price = Guard.Against.Negative(price);
    }
}