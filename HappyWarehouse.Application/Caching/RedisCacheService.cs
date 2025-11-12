using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace HappyWarehouse.Application.Caching;

public class RedisCacheService(IDistributedCache cache): IRedisCacheService
{
    public T? GetData<T>(string key)
    {
        var data = cache.GetString(key);
        
        if (data == null) return default;

        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T value)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        
        cache.SetString(key, JsonSerializer.Serialize(value), options);
    }
}