namespace LeaderAnalytics.Observer.Fred.Services;

public class API_Manifest : ServiceManifestFactory, IObserverAPI_Manifest
{
    public IObservationsService ObservationsService => Create<IObservationsService>();
    public IReleasesService ReleasesService => Create<IReleasesService>();
    public ISeriesService SeriesService => Create<ISeriesService>();
    public ICategoriesService CategoriesService => Create<ICategoriesService>();
}
