using ProjName.DBManager;
using Microsoft.EntityFrameworkCore;

var dbInitializer = new DbInitializer().CreateDbContext(args);

try
{
    // Undo the following line to do the migration.
    dbInitializer.Database.Migrate();
}
catch(Exception ex)
{
    Console.WriteLine("Message: \r\n" + ex.Message + "\r\n\r\nStackTrace:\r\n" + ex.StackTrace);
}

// Console.ReadLine();
