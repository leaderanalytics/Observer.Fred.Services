namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface IObserverAPI_Manifest
{
    IObservationsService ObservationsService { get; }
    IReleasesService ReleasesService { get; }
    ISeriesService SeriesService { get; }
    ICategoriesService CategoriesService { get; }
}
