namespace LeaderAnalytics.Observer.Fred.Services;

public class DatabaseInitializer : IDatabaseInitializer
{
    private Db db;


    public DatabaseInitializer(Db db)
    {
        this.db = db;
    }

    public async Task Seed(string migrationName)
    {

    }
}
