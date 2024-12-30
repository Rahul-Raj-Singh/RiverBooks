using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.OrderProcessing.Data;

namespace RiverBooks.OrderProcessing;

public static class OrderProcessingServiceExtensions
{
    public static IServiceCollection AddOrderProcessingServices(this IServiceCollection services, IConfiguration configuration,
        List<Assembly> mediatorAssemblies)
    {
        services.AddDbContext<OrderProcessingDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("SqlDbConnection")));
        
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        
        services.AddScoped<RedisOrderAddressCache>();
        services.AddScoped<IOrderAddressCache, ReadThroughOrderAddressCache>();
        
        mediatorAssemblies.Add(typeof(OrderProcessingServiceExtensions).Assembly);
        
        return services;
    }
}

