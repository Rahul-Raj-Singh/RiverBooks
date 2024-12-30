using System;

namespace RiverBooks.Users;

internal class UserAddress
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string Street1 { get; private set; }
    public string Street2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }

    public UserAddress(Guid id, string userId, string street1, string street2, 
        string city, string state, string country, string postalCode)
    {
        Id = id;
        UserId = userId;
        Street1 = street1;
        Street2 = street2;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }
}