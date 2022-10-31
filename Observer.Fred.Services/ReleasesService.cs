using LeaderAnalytics.Observer.Fred.Services.Domain;
using LeaderAnalytics.Vyntix.Fred.Model;

namespace LeaderAnalytics.Observer.Fred.Services;

public class ReleasesService : BaseService, IReleasesService
{
    public ReleasesService(Db db, IObserverAPI_Manifest serviceManifest, IFredClient fredClient) : base(db, serviceManifest, fredClient)
    {

    }

    public async Task<RowOpResult> DownloadAllReleases()
    {
        RowOpResult result = new RowOpResult();
        List<Release> releases = await fredClient.GetAllReleases();

        if (releases?.Any() ?? false)
        {
            foreach (Release release in releases)
                await SaveRelease(release, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadAllReleaseDates()
    {
        RowOpResult result = new RowOpResult();

        List<ReleaseDate> dates = await fredClient.GetAllReleaseDates(null, true);

        if (dates?.Any() ?? false)
        {
            foreach (var grp in dates.GroupBy(x => x.ReleaseID))
            {
                await DownloadReleaseIfItDoesNotExist(grp.Key);

                foreach (ReleaseDate releaseDate in grp)
                    await SaveReleaseDate(releaseDate, false);
            }
            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    


    public async Task<RowOpResult> DownloadRelease(string releaseID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(releaseID); // replaced by ArgumentException.ThrowIfNullorEmpty(...) in .net 7
        RowOpResult result = new RowOpResult();
        Release? release = await fredClient.GetRelease(releaseID);

        if (release is not null)
            result = await SaveRelease(release, true);

        return result;
    }

    public async Task<RowOpResult> DownloadReleaseDates(string releaseID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(releaseID);
        RowOpResult result = new RowOpResult();
        await DownloadReleaseIfItDoesNotExist(releaseID);
        DateTime? maxDate = db.ReleaseDates.Where(x => x.ReleaseID == releaseID).Max(x => x == null ? null as DateTime? : x.DateReleased);
        List<ReleaseDate> dates = await fredClient.GetReleaseDatesForRelease(releaseID, maxDate, true);

        if (dates?.Any() ?? false)
        {
            foreach (ReleaseDate releaseDate in dates)
                await SaveReleaseDate(releaseDate, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadReleaseSeries(string releaseID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(releaseID);
        RowOpResult result = new RowOpResult();
        await DownloadReleaseIfItDoesNotExist(releaseID);
        List<Series> seriess = await fredClient.GetSeriesForRelease(releaseID);

        if (seriess?.Any() ?? false)
        {
            foreach (Series series in seriess)
            {
                Series? existing = await db.Series.FirstOrDefaultAsync(x => x.Symbol == series.Symbol);

                if (existing is not null)
                {
                    existing.ReleaseID = series.ReleaseID;
                    await serviceManifest.SeriesService.SaveSeries(existing, false);
                }
                else
                    await serviceManifest.SeriesService.SaveSeries(series, false);
            }
            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadReleaseSources(string releaseID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(releaseID);
        RowOpResult result = new RowOpResult();
        await DownloadReleaseIfItDoesNotExist(releaseID);
        List<Source> sources = await fredClient.GetSourcesForRelease(releaseID);

        if (sources?.Any() ?? false)
        {
            foreach (Source source in sources)
                await SaveSource(source, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadSourceReleases(string sourceID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(sourceID);
        RowOpResult result = new RowOpResult();
        await DownloadSourceIfItDoesNotExist(sourceID);
        List<Release> releases = await fredClient.GetReleasesForSource(sourceID);

        if (releases?.Any() ?? false)
        {
            foreach (Release release in releases)
                await SaveRelease(release, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadAllSources()
    {
        RowOpResult result = new RowOpResult();
        List<Source> sources = await fredClient.GetSources();

        if (sources?.Any() ?? false)
        {
            foreach (Source source in sources)
                await SaveSource(source, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadSource(string sourceID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(sourceID); // replaced by ArgumentException.ThrowIfNullorEmpty(...) in .net 7
        RowOpResult result = new RowOpResult();
        Source? source = await fredClient.GetSource(sourceID);

        if (source is not null)
        {
            result = await SaveSource(source, true);
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveRelease(Release release, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(release);

        if (string.IsNullOrEmpty(release.NativeID))
            throw new Exception($"{nameof(release.NativeID)} is required.");
        else if (string.IsNullOrEmpty(release.Name))
            throw new Exception($"{nameof(release.Name)} is required.");

        RowOpResult result = new RowOpResult();
        Release? dupe = db.Releases.FirstOrDefault(x => x.ID != release.ID && x.NativeID == release.NativeID);

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(release).State = release.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (release.SourceReleases?.Any() ?? false)
            {
                foreach (SourceRelease sr in release.SourceReleases)
                {
                    sr.ReleaseNativeID = release.NativeID; // safety check

                    // check for dupe
                    if (db.SourceReleases.Any(x => x.SourceNativeID == sr.SourceNativeID && x.ReleaseNativeID == sr.ReleaseNativeID))
                        continue;

                    // make sure the source exists
                    if (!db.Sources.Any(x => x.NativeID == sr.ReleaseNativeID))
                        continue;

                    db.Entry(sr).State = EntityState.Added;
                }
            }

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveReleaseDate(ReleaseDate releaseDate, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(releaseDate);

        if (string.IsNullOrEmpty(releaseDate.ReleaseID))
            throw new Exception($"{nameof(releaseDate.ReleaseID)} is required.");

        RowOpResult result = new RowOpResult();
        ReleaseDate? dupe = db.ReleaseDates.FirstOrDefault(x => x.ID != releaseDate.ID && x.ReleaseID == releaseDate.ReleaseID && x.DateReleased == releaseDate.DateReleased);

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(releaseDate).State = releaseDate.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveSource(Source source, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (string.IsNullOrEmpty(source.NativeID))
            throw new Exception($"{nameof(source.NativeID)} is required.");
        else if (string.IsNullOrEmpty(source.Name))
            throw new Exception($"{nameof(source.Name)} is required.");

        RowOpResult result = new RowOpResult();
        Source? dupe = db.Sources.FirstOrDefault(x => x.ID != source.ID && x.NativeID == source.NativeID);

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(source).State = source.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (source.SourceReleases?.Any() ?? false)
            {
                foreach (SourceRelease sr in source.SourceReleases)
                {
                    sr.SourceNativeID = source.NativeID; // safety check

                    // check for dupe
                    if (db.SourceReleases.Any(x => x.SourceNativeID == sr.SourceNativeID && x.ReleaseNativeID == sr.ReleaseNativeID))
                        continue;

                    // make sure the release exists
                    if (!db.Releases.Any(x => x.NativeID == sr.ReleaseNativeID))
                        continue;

                    db.Entry(sr).State = EntityState.Added;
                }
            }

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    private async Task DownloadReleaseIfItDoesNotExist(string releaseID)
    {
        if ((await db.Releases.FirstOrDefaultAsync(x => x.NativeID == releaseID)) is null)
            if (!(await DownloadRelease(releaseID)).Success)
                throw new Exception($"Invalid releaseID: {releaseID}");
    }

    private async Task DownloadSourceIfItDoesNotExist(string sourceID)
    {
        if ((await db.Sources.FirstOrDefaultAsync(x => x.NativeID == sourceID)) is null)
            if (!(await DownloadSource(sourceID)).Success)
                throw new Exception($"Invalid sourceID: {sourceID}");
    }
}
