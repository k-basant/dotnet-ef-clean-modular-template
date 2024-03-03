using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProjName.Infrastructure.Persistence;

namespace ProjName.Application.UnitTests.Setup;

[Collection("Shared")]
public abstract class TestBase : IDisposable
{
    private IServiceScope? _overidedScope;
    private readonly StartupFixture _fixture;
    private IMediator _mediatR = null!;
    private IDbContextTransaction _transaction = null!;

    /// <summary>
    /// This should be referred when DI version of the Service is required.
    /// </summary>
    protected IMediator MediatR
    {
        get { return _mediatR; }
        set { _mediatR = value; }
    }
    protected AppDbContext DbContext { get; private set; } = null!;
    protected ServiceCollection RegisteredServices { get { return _fixture.Services; } }
    public TestBase(StartupFixture fixture)
    {
        _fixture = fixture;
        Init();
    }
    protected void Init()
    {
        // Fetch DbContext
        this.DbContext = (AppDbContext)GetService<IAppDbContext>();
        _mediatR = GetService<IMediator>();

        if (_transaction != null) { _transaction.Dispose(); }

        _transaction = this.DbContext.Database.BeginTransaction();
    }

    /// <summary>
    /// Returns the <typeparamref name="TService"/> instance resolved via DI Service Provider.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    protected TService GetService<TService>() where TService: notnull
    {
        if (_overidedScope == null)
            return _fixture.Scope.ServiceProvider.GetRequiredService<TService>()!;
        else
            return _overidedScope.ServiceProvider.GetRequiredService<TService>()!;
    }
    /// <summary>
    /// This allows using the overrided DI servies for the currently running test and is suggested to be called before performing any other actions.
    /// </summary>
    /// <param name="services"></param>
    protected void UseOveridedDIServices()
    {
        _overidedScope = _fixture.Services.BuildServiceProvider().CreateScope();

        Init();
    }
    public void Dispose()
    {
        _transaction.Dispose();
    }
}

[CollectionDefinition("Shared")]
public class TestBaseCollection : ICollectionFixture<StartupFixture>
{

}
