using System.Text.Json;
using RedisExample.Web.Services.Interfaces;
using StackExchange.Redis;

namespace RedisExample.Web.Services.Implementations;

public sealed class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer)
    {
        ArgumentNullException.ThrowIfNull(connectionMultiplexer);
        _database = connectionMultiplexer.GetDatabase(Convert.ToInt32(configuration["Redis:Database"]));
    }

    public async Task PingAsync()
    {
        await _database.PingAsync();
    }

    public async Task SetAsync<TModel>(string key, TModel value, TimeSpan expiration) where TModel: class, new()
    {
        var redisValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, redisValue, expiration);
    }

    public async Task SetAsync<TModel>(string key, TModel value, DateTimeOffset expiration) where TModel: class, new()
    {
        var redisValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, redisValue, TimeSpan.FromTicks(expiration.Ticks));
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    public async Task<TModel?> GetAsync<TModel>(string key) where TModel: class, new()
    {
        var redisValue = await _database.StringGetAsync(key);
        return redisValue.HasValue
            ? JsonSerializer.Deserialize<TModel>(redisValue.ToString())
            : null;
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}