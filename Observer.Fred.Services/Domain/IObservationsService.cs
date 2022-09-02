namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface IObservationsService
{
    Task<RowOpResult> DownloadObservations(string symbol);
}
