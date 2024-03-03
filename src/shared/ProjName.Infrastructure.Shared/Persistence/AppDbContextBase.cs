using Microsoft.Extensions.Options;

namespace ProjName.Infrastructure.Shared.Persistence;

public abstract class AppDbContextBase: DbContext
{
    protected readonly IAppSettings _appSettings;
    protected readonly EntitySaveChangesInterceptor _entitySaveChangesInterceptor;

    public string ConnectionString { get; protected set; } = string.Empty;
    public abstract string Schema { get; }
    public AppDbContextBase(IAppSettings appSettings, EntitySaveChangesInterceptor saveChangesInterceptor)
    {
        _appSettings = appSettings;
        _entitySaveChangesInterceptor = saveChangesInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {        
        if (ConnectionString.IsNullOrEmpty())
        {
            ConnectionString = _appSettings.ConnectionString;
        }
        optionsBuilder.UseSqlServer(ConnectionString);
        optionsBuilder.AddInterceptors(_entitySaveChangesInterceptor);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Schema.IsNotNullOrEmpty())
        {
            modelBuilder.HasDefaultSchema(Schema);
        }
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<T> GetDbSet<T, TPK>() where T : BaseEntity<TPK> where TPK : struct
    {
        return this.Set<T>();
    }
}
