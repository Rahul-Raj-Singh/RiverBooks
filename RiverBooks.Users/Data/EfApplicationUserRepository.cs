using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task SaveChangesAsync();
}

internal class EfApplicationUserRepository(UserDbContext context) : IApplicationUserRepository
{
    public async Task<ApplicationUser> GetUserWithCartByEmailAsync(string email)
    {
        var existingUser = await context.Users
            .Include(x => x.CartItems)
            .SingleOrDefaultAsync(x => x.Email == email);

        return existingUser;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}