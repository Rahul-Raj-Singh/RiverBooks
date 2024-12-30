using System;
using System.Threading.Tasks;
using MediatR;
using RiverBooks.Users.Contracts;

namespace RiverBooks.OrderProcessing.Data;

internal class ReadThroughOrderAddressCache : IOrderAddressCache
{
    private readonly RedisOrderAddressCache _decorated;
    private readonly IMediator _mediator;

    public ReadThroughOrderAddressCache(RedisOrderAddressCache cache, IMediator mediator)
    {
        _decorated = cache;
        _mediator = mediator;
    }
    public async Task<Address> GetAddressByIdAsync(Guid id)
    {
        var cachedAddress = await _decorated.GetAddressByIdAsync(id);
        
        if (cachedAddress is not null) return cachedAddress;

        var query = new GetAddressQuery(id);
        
        var addressDto = await _mediator.Send(query);

        if (addressDto is null) return null;

        var address = new Address(
            addressDto.Street1, 
            addressDto.Street2, 
            addressDto.City, 
            addressDto.State, 
            addressDto.Country, 
            addressDto.PostalCode);

        await SetAddressByIdAsync(addressDto.Id, address);
        
        return address;
    }

    public async Task SetAddressByIdAsync(Guid id, Address address)
    {
        await _decorated.SetAddressByIdAsync(id, address);
    }
}


