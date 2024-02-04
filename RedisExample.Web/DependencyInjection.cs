using RedisExample.Web.Helpers;
using RedisExample.Web.Services.Implementations;
using RedisExample.Web.Services.Interfaces;

namespace RedisExample.Web;

internal static class DependencyInjection
{
    internal static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        var multiplexer = RedisHelper.GetConnection(configuration);
        services.AddSingleton(multiplexer);
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddKeyedSingleton<ICacheService, RedisCacheService>("redis");
        services.AddKeyedSingleton<ICacheService, MemCacheService>("memcache");

        services.AddSingleton<ICategoryService, CategoryService>();
    }
}