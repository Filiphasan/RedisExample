namespace RedisExample.Web.Services.Interfaces;

public interface ICacheService
{
    Task PingAsync();
    Task SetAsync<TModel>(string key, TModel value, TimeSpan expiration)
        where TModel : class, new();
    Task SetAsync<TModel>(string key, TModel value, DateTimeOffset expiration)
        where TModel : class, new();
    Task<TModel?> GetAsync<TModel>(string key)
        where TModel : class, new();
    Task<bool> ExistsAsync(string key);
    Task RemoveAsync(string key);
}