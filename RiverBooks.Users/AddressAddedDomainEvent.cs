using RiverBooks.Common;

namespace RiverBooks.Users;

internal class AddressAddedDomainEvent : DomainEvent
{
    public UserAddress Address { get; set; }
    public AddressAddedDomainEvent(UserAddress address)
    {
        Address = address;
    }
}