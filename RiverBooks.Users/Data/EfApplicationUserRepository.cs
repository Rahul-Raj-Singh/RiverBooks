using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Users.Data;

internal interface IApplicationUserRepository
{
    Task<ApplicationUser> GetUserWithCartByEmailAsync(string email);
    Task<ApplicationUser> GetUserWithAddressesByEmailAsync(string email);
    Task SaveChangesAsync();
    void DeleteCartItems(IEnumerable<CartItem> cartItems);
    Task<UserAddress> GetAddressByIdAsync(Guid id);
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
    public async Task<ApplicationUser> GetUserWithAddressesByEmailAsync(string email)
    {
        var existingUser = await context.Users
            .Include(x => x.Addresses)
            .SingleOrDefaultAsync(x => x.Email == email);

        return existingUser;
    }
    
    public void DeleteCartItems(IEnumerable<CartItem> cartItems)
    {
        context.CartItems.RemoveRange(cartItems);
    }

    public async Task<UserAddress> GetAddressByIdAsync(Guid id)
    {
        return await context.UserAddresses.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}