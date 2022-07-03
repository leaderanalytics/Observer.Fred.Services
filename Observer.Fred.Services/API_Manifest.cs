namespace LeaderAnalytics.Observer.Fred.Services;

public class API_Manifest : ServiceManifestFactory, IAPI_Manifest
{
    public IObservationsService ObservationsService => Create<IObservationsService>();
    public ISeriesService SeriesService => Create<ISeriesService>();
}
