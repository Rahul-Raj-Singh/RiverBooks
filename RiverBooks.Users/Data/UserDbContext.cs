using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiverBooks.Common;

namespace RiverBooks.Users.Data;

internal class UserDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IDomainEventDispatcher _dispatcher;

    public UserDbContext(DbContextOptions<UserDbContext> options, IDomainEventDispatcher dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

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
        
        builder.Entity<UserAddress>(x =>
        {
            x.ToTable("UserAddress");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
            x.HasOne<ApplicationUser>()
                .WithMany(y => y.Addresses)
                .HasForeignKey(y => y.UserId);
        });
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        if (_dispatcher is null) return result;

        var entitiesWithDomainEvents = ChangeTracker
            .Entries<IHaveDomainEvents>()
            .Select(x => x.Entity)
            .ToArray();
        
        await _dispatcher.DispatchAndClearDomainEvents(entitiesWithDomainEvents);

        return result;
    }
}