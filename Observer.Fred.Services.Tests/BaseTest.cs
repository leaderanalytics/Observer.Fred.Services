using LeaderAnalytics.AdaptiveClient.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace LeaderAnalytics.Observer.Fred.Services.Tests;

[TestFixture]
public abstract class BaseTest
{
    protected ILifetimeScope scope;
    protected IAdaptiveClient<IObserverAPI_Manifest> client;
    protected readonly List<IEndPointConfiguration> endPoints;
    protected Db db;
    private IHost host;


    public BaseTest()
    {
        endPoints = EndPointUtilities.LoadEndPoints("appsettings.development.json").ToList();

        host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(builder =>  builder.AddJsonFile("appsettings.development.json", false))
            .ConfigureServices((config, services) => {
                string apiKey = config.Configuration.GetValue<string>("FredAPI_Key");
                services.AddFredClient()
                .UseAPIKey(apiKey)
                .UseConfig(x => new FredClientConfig { MaxDownloadRetries = 1 });
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>((config, containerBuilder) =>
            {
                RegistrationHelper registrationHelper = new RegistrationHelper(containerBuilder);
                registrationHelper.RegisterModule(new LeaderAnalytics.Observer.Fred.Services.AdaptiveClientModule(endPoints));
                containerBuilder.RegisterModule(new LeaderAnalytics.Observer.Fred.Services.AutofacModule());
            }).Build();
    }

    [SetUp]
    protected virtual async Task Setup()
    {
        scope = host.Services.GetAutofacRoot().BeginLifetimeScope();
        client = scope.Resolve<IAdaptiveClient<IObserverAPI_Manifest>>();
        ResolutionHelper resolutionHelper = scope.Resolve<ResolutionHelper>();
        IEndPointConfiguration ep = endPoints.First(x => x.IsActive && x.API_Name == API_Name.Observer && x.ProviderName == DatabaseProviderName.MSSQL);
        db = resolutionHelper.ResolveDbContext(ep) as Db;
        IDatabaseUtilities databaseUtilities = scope.Resolve<IDatabaseUtilities>();
        await databaseUtilities.DropDatabase(ep);
        DatabaseValidationResult result = await databaseUtilities.CreateOrUpdateDatabase(ep);
        
        if (!result.DatabaseWasCreated)
            throw new Exception("Database was not created.");
    }
}
