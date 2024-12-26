using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Users.Data;

namespace RiverBooks.Users;

public static class UserModuleExtensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration configuration,
        List<Assembly> mediatorAssemblies)
    {
        services.AddDbContext<UserDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("SqlDbConnection")));

        services.AddIdentityCore<ApplicationUser>().AddEntityFrameworkStores<UserDbContext>();
        
        mediatorAssemblies.Add(typeof(UserModuleExtensions).Assembly);

        services.AddScoped<IApplicationUserRepository, EfApplicationUserRepository>();
        
        return services;
    }
}