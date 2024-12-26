using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints;

internal class GetBookByIdEndpoint(IBookService bookService) : Endpoint<GetBookByIdRequest, BookDto>
{
    public override void Configure()
    {
        Get("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
    {
        var book = await bookService.GetBookByIdAsync(req.Id);

        if (book is null) 
            await SendNotFoundAsync();
        else
            await SendAsync(book);
    }
}

internal class GetBookByIdRequest
{
    public Guid Id { get; set; }
}

internal class GetBookByIdValidator : Validator<GetBookByIdRequest>
{
    public GetBookByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}