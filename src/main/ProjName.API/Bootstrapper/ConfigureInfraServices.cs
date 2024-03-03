using Microsoft.Data.SqlClient;
using ProjName.Application.Interfaces;
using ProjName.Application.Shared.Interfaces;
using ProjName.Infrastructure.Persistence;
using ProjName.Infrastructure.Shared.Persistence;
using ProjName.Infrastructure.Shared.Services;
using ProjName.Infrastructure.Shared.Sys;
using Serilog;
using System.Data;

namespace ProjName.API.Bootstrapper;

public static class ConfigureInfraServices
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, WebApplicationBuilder builder)
    {

        services.AddScoped<IAppSettings, AppSettings>();

        services.AddMemoryCache();
        services.AddScoped<ICachingService, MemoryCachingService>();

        services.AddScoped<IDateTimeService, DateTimeService>();

        builder.Host.UseSerilog((ctxt, logger) =>
        {
            logger.ReadFrom.Configuration(ctxt.Configuration); // Add required sink configuration in app settings file and also add required lib.
        });

        services.AddHttpClient();
        services.AddScoped<IHttpService, HttpService>();

        services.AddScoped<AppDbContext>();
        services.AddScoped<ICoreDbContext>(x => x.GetRequiredService<AppDbContext>());
        services.AddScoped<IAppDbContext>(x => x.GetRequiredService<AppDbContext>());
        services.AddScoped<EntitySaveChangesInterceptor>();
        services.AddScoped<IDbConnection>(_ => new SqlConnection(builder.Configuration.GetConnectionString(SettingsConstants.ConnString)));

        services.AddScoped<IFileStorageService, DiskFileStorageService>();
        services.AddScoped<IQueueService, MemoryQueueService>();

        return services;
    }
}
