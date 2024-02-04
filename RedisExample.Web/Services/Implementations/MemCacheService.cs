using Microsoft.Extensions.Caching.Memory;
using RedisExample.Web.Services.Interfaces;

namespace RedisExample.Web.Services.Implementations;

public sealed class MemCacheService(IMemoryCache memoryCache) : ICacheService
{
    public Task PingAsync()
    {
        memoryCache.Set("ping", "pong", TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    public async Task SetAsync<TModel>(string key, TModel value, TimeSpan expiration) where TModel: class, new()
    {
        await Task.Run(() =>
        {
            memoryCache.Set(key, value, expiration);
        });
    }

    public async Task SetAsync<TModel>(string key, TModel value, DateTimeOffset expiration) where TModel: class, new()
    {
        await Task.Run(() =>
        {
            memoryCache.Set(key, value, expiration);
        });
    }

    public Task<bool> ExistsAsync(string key)
    {
        return Task.FromResult(memoryCache.TryGetValue(key, out object _));
    }

    public Task<TModel?> GetAsync<TModel>(string key) where TModel: class, new()
    {
        return Task.FromResult(memoryCache.Get<TModel>(key));
    }

    public Task RemoveAsync(string key)
    {
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}