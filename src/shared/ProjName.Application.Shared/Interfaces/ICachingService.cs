namespace ProjName.Application.Shared.Interfaces;

public interface ICachingService
{
    Task<T> GetOrCreateSlidingAsync<T>(string key, int minutes, Func<Task<T>> valueProvider);
    Task<T> GetOrCreateAbsoluteAsync<T>(string key, DateTimeOffset expTime, Func<Task<T>> valueProvider);
    void Remove(string key);
}
