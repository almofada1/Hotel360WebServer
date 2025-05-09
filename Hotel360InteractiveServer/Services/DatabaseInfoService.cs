using Microsoft.Data.SqlClient;

public class DatabaseInfoService
{
    public string DefaultDatabaseName { get; }

    public DatabaseInfoService(IConfiguration config)
    {
        var cs = config.GetConnectionString("DefaultConnection");
        DefaultDatabaseName = new SqlConnectionStringBuilder(cs).InitialCatalog;
    }
}
