namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface IReleasesService
{
    Task<RowOpResult> DownloadAllReleaseDates();
    Task<RowOpResult> DownloadAllReleases();
    Task<RowOpResult> DownloadRelease(string releaseID);
    Task<RowOpResult> DownloadReleaseDates(string releaseID);
    Task<RowOpResult> DownloadReleaseSeries(string releaseID);
    Task<RowOpResult> DownloadReleaseSources(string releaseID);
    Task<RowOpResult> SaveRelease(Release release, bool saveChanges = true);
    Task<RowOpResult> SaveReleaseDate(ReleaseDate releaseDate, bool saveChanges = true);
    Task<RowOpResult> SaveSource(Source source, bool saveChanges = true);
}