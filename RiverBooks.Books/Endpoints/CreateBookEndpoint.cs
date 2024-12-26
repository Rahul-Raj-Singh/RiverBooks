using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints;

internal class CreateBookEndpoint(IBookService bookService) : Endpoint<CreateBookRequest, BookDto>
{
    public override void Configure()
    {
        Post("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        var newBookDto = new BookDto { Title = req.Title, Author = req.Author, Price = req.Price };
        
        var newBook = await bookService.CreateBookAsync(newBookDto);
        
        await SendAsync(newBook, statusCode: (int)HttpStatusCode.Created);
    }
}

internal class CreateBookRequest
{
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
}

internal class CreateBookRequestValidator : Validator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Author).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}