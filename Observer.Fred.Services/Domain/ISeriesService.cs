namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface ISeriesService
{
    Task<RowOpResult> DownloadSeriesCategoriesForCategory(string categoryID);
    Task<IEnumerable<Series>> GetLocalSeries(int skip, int take, string searchTitle);
    Task<IEnumerable<string>> GetLocalSeriesSymbols();
    Task<RowOpResult> SaveLocalSeries(string symbol);
    Task<RowOpResult> SaveSeries(Series series, bool saveChanges = true);
    Task<RowOpResult> SaveSeriesCategory(SeriesCategory seriesCategory, bool saveChanges = true);
    Task<RowOpResult> DeleteLocalSeries(string symbol);
}
