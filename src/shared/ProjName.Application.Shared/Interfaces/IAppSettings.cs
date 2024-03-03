namespace ProjName.Application.Shared.Interfaces;

public interface IAppSettings
{
    string ConnectionString { get; }
    Task<T> GetSetting<T>(string key);
    Task SetSettingAsync<T>(string key, T value);
}