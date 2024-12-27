using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace RiverBooks.Books.Endpoints;

internal class UpdateBookPriceEndpoint(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
    public override void Configure()
    {
        Put("/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
    {
        var result = await bookService.UpdateBookPriceAsync(new BookDto {Id = req.Id, Price = req.Price});
        
        if (result.IsSuccess) 
            await SendOkAsync(ct);
        else
            await SendNotFoundAsync(ct);
    }
}

internal class UpdateBookPriceRequest
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
}

internal class UpdateBookPriceRequestValidator : Validator<UpdateBookPriceRequest>
{
    public UpdateBookPriceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}