using StackExchange.Redis;

namespace RedisExample.Web.Helpers;

public static class RedisHelper
{
    public static IConnectionMultiplexer GetConnection(IConfiguration configuration)
    {
        var redisOptions = ConfigurationOptions.Parse($"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}");
        redisOptions.Password = configuration["Redis:Password"];
        redisOptions.AbortOnConnectFail = false;
        var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
        return multiplexer;
    }
}