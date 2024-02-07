using RedisExample.Web.Helpers;
using RedisExample.Web.Services.Implementations;
using RedisExample.Web.Services.Interfaces;

namespace RedisExample.Web;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        var multiplexer = RedisHelper.GetConnection(configuration);
        services.AddSingleton(multiplexer);
        // ICacheService kullanılan yerlerde varsayılan olarak Redis'in çalışmasını sağlar
        services.AddSingleton<ICacheService, RedisCacheService>();
        // ICacheService kullanılan yerlerde varsayılan olarak In-Memory Cache'in çalışmasını sağlar
        // services.AddSingleton<ICacheService, MemCacheService>()
        services.AddKeyedSingleton<ICacheService, RedisCacheService>("redis");
        services.AddKeyedSingleton<ICacheService, MemCacheService>("memcache");

        services.AddSingleton<ICategoryService, CategoryService>();
    }
}