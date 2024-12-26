using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Books.Data;

namespace RiverBooks.Books;

public static class BookServiceExtensions
{
    public static IServiceCollection AddBookServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("SqlDbConnection")));
        
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookRepository, EfBookRepository>();
        return services;
    }
}