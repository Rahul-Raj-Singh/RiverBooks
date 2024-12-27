using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Books.Contracts;
using RiverBooks.Books.Data;

namespace RiverBooks.Books.Integrations;

internal class BookDetailsQueryHandler(IBookRepository repository) 
    : IRequestHandler<GetBookDetailsQuery, Result<GetBookDetailsResponse>>
{
    public async Task<Result<GetBookDetailsResponse>> Handle(GetBookDetailsQuery req, CancellationToken ct)
    {
        var existingBook = await repository.GetByIdAsync(req.BookId);

        if (existingBook is null) return Result.NotFound();

        return new GetBookDetailsResponse
        {
            Id = existingBook.Id,
            Title = existingBook.Title,
            Author = existingBook.Author,
            Price = existingBook.Price,
        };
        
    }
}