using Microsoft.EntityFrameworkCore.Design;
using ProjName.DBManager.Databases;

namespace ProjName.DBManager;

internal class DbInitializer : IDesignTimeDbContextFactory<AppDbContextExt>
{
    public AppDbContextExt CreateDbContext(string[] args)
    {
        if(args.Length == 0)
        {
            return new AppDbContextExt("Server=localhost;Database=app-data;Integrated Security=True;TrustServerCertificate=True;");
        }
        else
        {
            return new AppDbContextExt(args[0]);
        }
    }
}
