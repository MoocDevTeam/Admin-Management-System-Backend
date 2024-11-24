using System.Drawing;

namespace Mooc.Core.Caching;

public interface IMoocCache
{
    Task<T> GetAsync<T>(string key);
    T Get<T>(string key);
    Task SetAsync<T>(string key, T value, int second, bool isAbsoluteExpiration = false);
    void Set<T>(string key, T value, int second, bool isAbsoluteExpiration = false);
    Task RemoveAsync(string key);
    void Remove(string key);

}
