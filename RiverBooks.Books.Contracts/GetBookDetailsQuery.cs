using System;
using Ardalis.Result;
using MediatR;

namespace RiverBooks.Books.Contracts;

public class GetBookDetailsQuery : IRequest<Result<GetBookDetailsResponse>>
{
    public Guid BookId { get; set; }
}