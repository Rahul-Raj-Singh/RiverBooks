using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RiverBooks.OrderProcessing.Data;

internal interface IOrderAddressCache
{
    Task<Address> GetAddressByIdAsync(Guid id);
    Task SetAddressByIdAsync(Guid id, Address address);
}

internal class RedisOrderAddressCache : IOrderAddressCache
{
    private readonly IDatabase _db;

    public RedisOrderAddressCache(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
        _db = redis.GetDatabase();
    }
    public async Task<Address> GetAddressByIdAsync(Guid id)
    {
        string redisValue = await _db.StringGetAsync(nameof(Address) + ":" + id);
        
        if (redisValue is null) return null;
        
        var address = JsonSerializer.Deserialize<Address>(redisValue);

        return address;
    }

    public async Task SetAddressByIdAsync(Guid id, Address address)
    {
        var redisValue = JsonSerializer.Serialize(address);
        
        await _db.StringSetAsync(nameof(Address) + ":" + id, redisValue);
    }
}