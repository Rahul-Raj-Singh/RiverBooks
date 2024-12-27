using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints;

internal class DeleteBookEndpoint(IBookService bookService) : Endpoint<DeleteBookRequest, BookDto>
{
    public override void Configure()
    {
        Delete("/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBookRequest req, CancellationToken ct)
    {
        var result = await bookService.DeleteBookAsync(req.Id);
        
        if (result.IsSuccess) 
            await SendNoContentAsync(ct);
        else
            await SendNotFoundAsync(ct);
    }
}

internal class DeleteBookRequest
{
    public Guid Id { get; set; }
}

internal class DeleteBookRequestValidator : Validator<DeleteBookRequest>
{
    public DeleteBookRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}