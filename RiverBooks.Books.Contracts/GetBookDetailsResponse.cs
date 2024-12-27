using System;

namespace RiverBooks.Books.Contracts;

public class GetBookDetailsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
}