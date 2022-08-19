namespace LeaderAnalytics.Observer.Fred.Services;

public abstract class BaseService
{
    protected readonly Db db;
    protected readonly IFredClient fredClient;
    protected readonly IObserverAPI_Manifest serviceManifest;

    public BaseService(Db db, IObserverAPI_Manifest serviceManifest, IFredClient fredClient)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
        this.fredClient = fredClient ?? throw new ArgumentNullException(nameof(fredClient));
        this.serviceManifest = serviceManifest ?? throw new ArgumentNullException(nameof(serviceManifest));
    }
}
