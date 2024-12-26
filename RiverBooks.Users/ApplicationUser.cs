using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

internal class ApplicationUser : IdentityUser
{
    private readonly List<CartItem> _cartItems = [];
    public List<CartItem> CartItems => _cartItems;

    public void AddItemToCart(CartItem cartItem)
    {
        var existingItem = _cartItems.SingleOrDefault(x => x.BookId == cartItem.BookId);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(cartItem.Quantity);
            return;
        }
        
        _cartItems.Add(cartItem);
    }
}