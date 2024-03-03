using ProjName.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

namespace ProjName.DBManager.Databases;

internal class AppDbContextExt : AppDbContext
{
    /// <summary>
    /// This should be used only by the DbManager and should never be used by the application.
    /// </summary>
    /// <param name="connectionString"></param>
    public AppDbContextExt(string connectionString) : base(null, null)
    {
        ConnectionString = connectionString;
    }
}
