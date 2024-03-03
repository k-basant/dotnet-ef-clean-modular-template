using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ProjName.Infrastructure.Shared.Sys;

public class AppSettings: IAppSettings
{
    private readonly IConfiguration _configuration;
    public AppSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ConnectionString => _configuration.GetConnectionString(SettingsConstants.ConnString)!;

    public Task<T> GetSetting<T>(string key)
    {
        var setting = _configuration[key];
        if (setting == null)
        {
            return Task.FromResult(default(T))!;
        }

        if (typeof(T) == typeof(string))
        {
            return Task.FromResult((T)Convert.ChangeType(setting, typeof(T)));
        }

        T typedSetting = JsonConvert.DeserializeObject<T>(setting!)!;
        return Task.FromResult(typedSetting);
    }

    public Task SetSettingAsync<T>(string key, T value)
    {
        throw new NotSupportedException("Setting values is not supported with IConfiguration");
    }
}
