using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.UseCases;

internal record AddAddressForUserCommand(
    string Email,
    string Street1,
    string Street2,
    string City,
    string State,
    string Country,
    string PostalCode) : IRequest<Result>;

internal class AddAddressForUserCommandHandler(IApplicationUserRepository repository) 
    : IRequestHandler<AddAddressForUserCommand, Result>
{
    public async Task<Result> Handle(AddAddressForUserCommand req, CancellationToken ct)
    {
        var existingUser = await repository.GetUserWithAddressesByEmailAsync(req.Email);

        if (existingUser is null) return Result.Unauthorized();

        var newAddress = new UserAddress(Guid.NewGuid(), existingUser.Id, req.Street1, req.Street2, 
            req.City, req.State, req.Country, req.PostalCode);
        
        existingUser.AddAddress(newAddress);

        await repository.SaveChangesAsync();
        
        return Result.Success();
    }
}