using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users;

internal class ApplicationUser : IdentityUser
{
    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public void AddItemToCart(CartItem cartItem)
    {
        var existingItem = _cartItems.SingleOrDefault(x => x.BookId == cartItem.BookId);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(cartItem.Quantity + existingItem.Quantity);
            existingItem.UpdateDescription(cartItem.Description);
            existingItem.UpdateUnitPrice(cartItem.UnitPrice);
            return;
        }
        
        _cartItems.Add(cartItem);
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }
}