using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;

namespace RiverBooks.Books.Endpoints;

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<List<BookDto>>
{
    public override void Configure()
    {
        Get("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var books = await bookService.GetAllBooksAsync();
        
        await SendAsync(books);
    }
}