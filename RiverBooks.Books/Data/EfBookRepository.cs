using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;

internal interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(Guid id);
    Task AddAsync(Book book);
    Task DeleteAsync(Book book);
    Task SaveChangesAsync();
}

internal class EfBookRepository(BookDbContext dbContext) : IBookRepository
{
    public async Task<List<Book>> GetAllAsync()
    {
        return await dbContext.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        return await dbContext.Books.FindAsync(id);
    }

    public async Task AddAsync(Book book)
    {
        dbContext.Books.Add(book);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Book book)
    {
        dbContext.Books.Remove(book);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}