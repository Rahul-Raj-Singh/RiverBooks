using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using RiverBooks.Common;

namespace RiverBooks.Users;

internal class ApplicationUser : IdentityUser, IHaveDomainEvents
{
    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();
    
    private readonly List<UserAddress> _addresses = [];
    public IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();
    
    private readonly List<DomainEvent> _domainEvents = [];
    [NotMapped]
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

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
    public void AddAddress(UserAddress address)
    {
        var existingAddress = _addresses.SingleOrDefault(x =>
            x.Street1 == address.Street1 &&
            x.Street2 == address.Street2 &&
            x.PostalCode == address.PostalCode);

        // address already exists
        if (existingAddress is not null) return;
        
        _addresses.Add(address);
        
        // register domain event
        AddDomainEvent(new AddressAddedDomainEvent(address));
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }

    public void AddDomainEvent(DomainEvent @event) => _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
}