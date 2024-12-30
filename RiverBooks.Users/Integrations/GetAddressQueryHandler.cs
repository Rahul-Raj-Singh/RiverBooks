using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RiverBooks.Users.Contracts;
using RiverBooks.Users.Data;

namespace RiverBooks.Users.Integrations;

internal class GetAddressQueryHandler(IApplicationUserRepository repository) 
    : IRequestHandler<GetAddressQuery, AddressDto>
{
    public async Task<AddressDto> Handle(GetAddressQuery req, CancellationToken ct)
    {
        var address = await repository.GetAddressByIdAsync(req.AddressId);
        
        if (address is null) return null;

        return new AddressDto(
            address.Id, address.Street1, address.Street2, address.City, address.State,address.PostalCode,address.Country);
    }
}