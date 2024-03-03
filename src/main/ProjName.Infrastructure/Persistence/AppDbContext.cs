using ProjName.Application.Interfaces;
using ProjName.Infrastructure.Shared.Persistence;

namespace ProjName.Infrastructure.Persistence;

#nullable disable
public class AppDbContext : AppDbContextBase, IAppDbContext, ICoreDbContext
{
    public override string Schema { get; } = string.Empty;

    public AppDbContext(IAppSettings appSettings, EntitySaveChangesInterceptor saveChangesInterceptor): base(appSettings, saveChangesInterceptor)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}