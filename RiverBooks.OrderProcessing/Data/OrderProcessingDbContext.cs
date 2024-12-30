using Microsoft.EntityFrameworkCore;

namespace RiverBooks.OrderProcessing.Data;

internal class OrderProcessingDbContext : DbContext
{
    public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options) : base(options)
    {
    }
    
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("OrderProcessing");

        modelBuilder.Entity<Order>(x =>
        {
            x.ToTable("Order");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
            x.ComplexProperty(y => y.BillingAddress).IsRequired();
            x.ComplexProperty(y => y.ShippingAddress).IsRequired();
        });
        
        modelBuilder.Entity<OrderItem>(x =>
        {
            x.ToTable("OrderItem");
            x.HasKey(y => y.Id);
            x.Property(y => y.Id).ValueGeneratedNever();
        });
        
        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>().WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
        
        
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
    }
}