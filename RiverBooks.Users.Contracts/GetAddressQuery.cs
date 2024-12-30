using System;
using MediatR;

namespace RiverBooks.Users.Contracts;

public record GetAddressQuery(Guid AddressId) : IRequest<AddressDto>;