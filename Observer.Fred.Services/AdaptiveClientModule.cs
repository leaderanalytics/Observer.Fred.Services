namespace LeaderAnalytics.Observer.Fred.Services;

public class AdaptiveClientModule : IAdaptiveClientModule
{
    public void Register(RegistrationHelper registrationHelper) => Register(registrationHelper, null); // yes it will throw

    public void Register(RegistrationHelper registrationHelper, IEnumerable<IEndPointConfiguration> endPoints)
    {
        ArgumentNullException.ThrowIfNull(registrationHelper);
        ArgumentNullException.ThrowIfNull(endPoints);

        registrationHelper

            // Always register endPoints first
            .RegisterEndPoints(endPoints)

            // EndPoint Validator
            .RegisterEndPointValidator<MSSQL_EndPointValidator>(EndPointType.DBMS, DatabaseProviderName.MSSQL)
            .RegisterEndPointValidator<MySQL_EndPointValidator>(EndPointType.DBMS, DatabaseProviderName.MySQL)

            // DbContextOptions
            .RegisterDbContextOptions<DbContextOptions_MSSQL>(DatabaseProviderName.MSSQL)
            .RegisterDbContextOptions<DbContextOptions_MySQL>(DatabaseProviderName.MySQL)

            // MSSQL
            .RegisterService<ObservationsService, IObservationsService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterService<SeriesService, ISeriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)

            // MySQL
            .RegisterService<ObservationsService, IObservationsService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL)
            .RegisterService<SeriesService, ISeriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL)

            // DbContext
            .RegisterDbContext<Db>(API_Name.Observer)

            // Migration Contexts
            .RegisterMigrationContext<Db_MSSQL>(API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterMigrationContext<Db_MySQL>(API_Name.Observer, DatabaseProviderName.MySQL)

            // Database Initializers
            .RegisterDatabaseInitializer<DatabaseInitializer>(API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterDatabaseInitializer<DatabaseInitializer>(API_Name.Observer, DatabaseProviderName.MySQL)

            // Service Manifests
            .RegisterServiceManifest<API_Manifest, IAPI_Manifest>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterServiceManifest<API_Manifest, IAPI_Manifest>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL);
    }
}
