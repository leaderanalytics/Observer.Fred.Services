namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface IObservationsService
{
    Task<List<Observation>> GetLocalObservations(string symbol);
    Task<List<Observation>> GetLocalObservations(string symbol, int skip, int take);
    Task<RowOpResult> UpdateLocalObservations(string symbols);
    Task<RowOpResult> DeleteLocalObservations(string symbol);
}
