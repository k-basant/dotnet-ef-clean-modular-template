using Microsoft.Extensions.Caching.Memory;

namespace ProjName.Infrastructure.Shared.Services;

public class MemoryCachingService : ICachingService
{
    private readonly IMemoryCache _memoryCache;
    public MemoryCachingService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public Task<T> GetOrCreateAbsoluteAsync<T>(string key, DateTimeOffset expTime, Func<Task<T>> valueProvider)
    {
        return _memoryCache.GetOrCreateAsync(key, async (c) =>
        {
            c.AbsoluteExpiration = expTime;
            return await valueProvider.Invoke();
        })!;
    }

    public Task<T> GetOrCreateSlidingAsync<T>(string key, int minutes, Func<Task<T>> valueProvider)
    {
        return _memoryCache.GetOrCreateAsync(key, async (c) =>
        {
            c.SlidingExpiration = TimeSpan.FromMinutes(minutes);
            return await valueProvider.Invoke();
        })!;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}
