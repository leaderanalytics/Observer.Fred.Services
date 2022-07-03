namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface IAPI_Manifest
{
    IObservationsService ObservationsService { get; }
    ISeriesService SeriesService { get; }
}
