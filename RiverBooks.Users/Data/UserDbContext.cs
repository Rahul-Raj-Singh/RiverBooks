using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal class UserDbContext : IdentityDbContext<ApplicationUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("Users");
        
        builder.Entity<CartItem>(x =>
        {
            x.ToTable("CartItem");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
            x.HasOne<ApplicationUser>()
                .WithMany(y => y.CartItems)
                .HasForeignKey(y => y.UserId);
        });
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}