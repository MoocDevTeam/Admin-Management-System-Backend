using Microsoft.Extensions.Caching.Distributed;
using Mooc.Core.DependencyInjection;
using System.Text.Json;

namespace Mooc.Core.Caching;

public class MoocCache : IMoocCache, ISingletonDependency
{
    private readonly IDistributedCache _cache;
    public MoocCache(IDistributedCache cache)
    {
        this._cache = cache;
    }

    public T Get<T>(string key)
    {
        var valueString = this._cache.GetString(key);
        if (string.IsNullOrEmpty(valueString))
            return default(T);

        return JsonSerializer.Deserialize<T>(valueString);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var valueString = await this._cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(valueString))
            return default(T);

        return JsonSerializer.Deserialize<T>(valueString);

    }

    public void Remove(string key)
    {
        this._cache.Remove(key);
    }

    public Task RemoveAsync(string key)
    {
        return this.RemoveAsync(key);
    }

    public void Set<T>(string key, T value, int second, bool isAbsoluteExpiration = false)
    {
        if (value == null)
            return;

        var valueString = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions();
        if (isAbsoluteExpiration)
            options.SetSlidingExpiration(TimeSpan.FromSeconds(second));
        else
            options.SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(second));

         this._cache.SetString(key, valueString, options);
    }

    public Task SetAsync<T>(string key, T value, int second, bool isAbsoluteExpiration = false)
    {
        if (value == null)
            return Task.CompletedTask;

        var valueString = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions();
        if (isAbsoluteExpiration)
            options.SetSlidingExpiration(TimeSpan.FromSeconds(second));
        else
            options.SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(second));

        return this._cache.SetStringAsync(key, valueString, options);
    }
}
