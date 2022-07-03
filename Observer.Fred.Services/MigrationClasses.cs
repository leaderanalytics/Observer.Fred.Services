using LeaderAnalytics.Observer.Fred.Services.Domain;
using System.Linq;

namespace LeaderAnalytics.Observer.Fred.Services;

public class MigrationConstants
{
    public static readonly IEnumerable<IEndPointConfiguration> endPoints;
    static MigrationConstants()
    {
        endPoints = LeaderAnalytics.AdaptiveClient.EndPointUtilities.LoadEndPoints(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json"), true);
    }
}

public class MSSQLContextFactory : IDesignTimeDbContextFactory<Db_MSSQL>
{
    public Db_MSSQL CreateDbContext(string[] args)
    {
        string connectionString = MigrationConstants.endPoints.First(x => x.API_Name == API_Name.Observer && x.ProviderName == DatabaseProviderName.MSSQL).ConnectionString;
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder();
        dbOptions.UseSqlServer(connectionString);
        Db_MSSQL db = new Db_MSSQL(dbOptions.Options);
        return db;
    }
}

public class MySQLContextFactory : IDesignTimeDbContextFactory<Db_MySQL>
{
    public Db_MySQL CreateDbContext(string[] args)
    {
        string connectionString = MigrationConstants.endPoints.First(x => x.API_Name == API_Name.Observer && x.ProviderName == DatabaseProviderName.MySQL).ConnectionString;
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder();
        dbOptions.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        Db_MySQL db = new Db_MySQL(dbOptions.Options);
        return db;
    }
}
