using System;
using MediatR;

namespace RiverBooks.Users.Contracts;

public record AddressAddedIntegrationEvent(AddressDto Address) : INotification;

public record AddressDto(
    Guid Id, 
    string Street1, 
    string Street2, 
    string City, 
    string State, 
    string PostalCode, 
    string Country);
