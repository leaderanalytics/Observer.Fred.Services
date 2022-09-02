namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface ISeriesService
{
    Task<RowOpResult> DownloadSeries(string symbol, string? releaseID = null);
    Task<RowOpResult> DownloadCategoriesForSeries(string symbol);
    Task<RowOpResult> DownloadSeriesRelease(string symbol);
    Task<RowOpResult> DownloadSeriesTags(string symbol);
    Task<RowOpResult> DownloadSeriesIfItDoesNotExist(string symbol);
    Task<RowOpResult> SaveSeries(Series series, bool saveChanges = true);
    Task<RowOpResult> SaveSeriesCategory(SeriesCategory seriesCategory, bool saveChanges = true);
}
