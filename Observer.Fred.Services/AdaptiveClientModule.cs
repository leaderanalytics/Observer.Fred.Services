namespace LeaderAnalytics.Observer.Fred.Services;

public class AdaptiveClientModule : IAdaptiveClientModule
{
    private readonly IEnumerable<IEndPointConfiguration> endPoints;

    public AdaptiveClientModule(IEnumerable<IEndPointConfiguration> endPoints) => this.endPoints = endPoints ?? throw new ArgumentNullException(nameof(endPoints));

    public void Register(RegistrationHelper registrationHelper)
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

            // DbContext
            .RegisterDbContext<Db>(API_Name.Observer)

            // Migration Contexts
            .RegisterMigrationContext<Db_MSSQL>(API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterMigrationContext<Db_MySQL>(API_Name.Observer, DatabaseProviderName.MySQL)

            // Database Initializers
            .RegisterDatabaseInitializer<DatabaseInitializer>(API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterDatabaseInitializer<DatabaseInitializer>(API_Name.Observer, DatabaseProviderName.MySQL)

            // Service Manifests
            .RegisterServiceManifest<API_Manifest, IObserverAPI_Manifest>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterServiceManifest<API_Manifest, IObserverAPI_Manifest>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL)

            // Services - MSSQL
            .RegisterService<CategoriesService, ICategoriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterService<ObservationsService, IObservationsService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterService<ReleasesService, IReleasesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterService<SeriesService, ISeriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)


            // Services - MySQL
            .RegisterService<CategoriesService, ICategoriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL)
            .RegisterService<ObservationsService, IObservationsService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL)
            .RegisterService<ReleasesService, IReleasesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MSSQL)
            .RegisterService<SeriesService, ISeriesService>(EndPointType.DBMS, API_Name.Observer, DatabaseProviderName.MySQL);
    }
}
