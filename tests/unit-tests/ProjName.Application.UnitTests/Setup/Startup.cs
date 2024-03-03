using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjName.API.Bootstrapper;

namespace ProjName.Application.UnitTests.Setup;


public class StartupFixture
{
    private ServiceCollection _services;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;

    #region C'tor
    public StartupFixture()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _services = new ServiceCollection();

        _services.AddLogging(o =>
        {
            o.ClearProviders();
            o.AddDebug();
        });

        _services.AddSingleton<IConfiguration>(x => _configuration);
        _services.AddAppServices(_configuration);
        _services.AddInfraServices(WebApplication.CreateBuilder());

        _serviceProvider = _services.BuildServiceProvider();

        _scope = _serviceProvider.CreateScope();

        // We can perform any one time DB setup here on this scope.
    }
    #endregion

    public IServiceProvider ServiceProvider { get { return _serviceProvider; } }
    public IConfiguration Configuration { get { return _configuration; } }
    public IServiceScope Scope { get { return _scope; } }
    public ServiceCollection Services { get { return _services; } }

    public void Dispose()
    {
        _scope.Dispose();
    }
}