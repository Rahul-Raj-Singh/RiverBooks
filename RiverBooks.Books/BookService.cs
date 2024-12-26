using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RiverBooks.Books.Data;

namespace RiverBooks.Books;

internal interface IBookService
{
    Task<BookDto> GetBookByIdAsync(Guid id);
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto> CreateBookAsync(BookDto bookDto);
    Task DeleteBookAsync(Guid id);
    Task UpdateBookPriceAsync(BookDto bookDto);
}
internal class BookService(IBookRepository repository, ILogger<BookService> logger) : IBookService
{
    public async Task<BookDto> GetBookByIdAsync(Guid id)
    {
        var existingBook = await repository.GetByIdAsync(id);

        return existingBook is null 
            ? null 
            : new BookDto {Id = existingBook.Id, Title = existingBook.Title, Author = existingBook.Author, Price = existingBook.Price};
    }
    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        var books= (await repository.GetAllAsync())
            .Select(x => new BookDto { Id = x.Id, Title = x.Title, Author = x.Author, Price = x.Price })
            .ToList();
        
        return books;
    }

    public async Task<BookDto> CreateBookAsync(BookDto bookDto)
    {
        var newBook = new Book(Guid.NewGuid(), bookDto.Title, bookDto.Author, bookDto.Price);
        await repository.AddAsync(newBook);
        await repository.SaveChangesAsync();
        
        bookDto.Id = newBook.Id;
        return bookDto;
    }

    public async Task UpdateBookPriceAsync(BookDto bookDto)
    {
        var existingBook = await repository.GetByIdAsync(bookDto.Id);

        if (existingBook is null)
        {
            logger.LogWarning("Cannot find book with id: {id}", bookDto.Id);
            return;
        }
        
        existingBook.UpdateBookPrice(bookDto.Price);
        await repository.SaveChangesAsync();
    }
    
    public async Task DeleteBookAsync(Guid id)
    {
        var existingBook = await repository.GetByIdAsync(id);

        if (existingBook is null)
        {
            logger.LogWarning("Cannot find book with id: {id}", id);
            return;
        }

        await repository.DeleteAsync(existingBook);
        await repository.SaveChangesAsync();
    }

}