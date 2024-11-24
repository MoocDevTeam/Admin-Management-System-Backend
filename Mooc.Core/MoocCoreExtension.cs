using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mooc.Core;

public static class MoocCoreExtension
{
    public static void AddAppCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorizationCore();
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();

        var redisEnabled = configuration["Caching:UseRedis"];
        if (string.IsNullOrEmpty(redisEnabled) || bool.Parse(redisEnabled))
        {
            var redisConfiguration = configuration["Caching:Configuration"];
            if (!string.IsNullOrEmpty(redisConfiguration))
            {
                var instanceName = configuration["Caching:InstanceName"];
                if (string.IsNullOrEmpty(instanceName))
                    instanceName = "moccdb";

                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = redisConfiguration;
                    option.InstanceName = instanceName;
                });
            }
        }
    }
}
