using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books.Data;

internal class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }
    
    internal DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("Books");

        modelBuilder.Entity<Book>(x =>
        {
            x.ToTable("Book");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
            x.Property(y => y.Title).HasMaxLength(200);
            x.Property(y => y.Author).HasMaxLength(200);
        });
        
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}