namespace LeaderAnalytics.Observer.Fred.Services;

public class SeriesService : BaseService, ISeriesService
{
    public SeriesService(Db db, IObserverAPI_Manifest serviceManifest,  IFredClient fredClient) : base(db, serviceManifest, fredClient)
    {

    }

    public async Task<RowOpResult> DownloadSeries(string symbol, string? releaseID = null)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(symbol);
        Series series = await fredClient.GetSeries(symbol);
        RowOpResult result = new RowOpResult();

        if (series is null)
            result.Message = $"Series with symbol {symbol} was not found.";
        else
        {
            series.ReleaseID = releaseID;
            result = await SaveSeries(series, true);
        }
        return result;
    }

    public async Task<RowOpResult> DownloadCategoriesForSeries(string symbol)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(symbol);
        RowOpResult seriesResult = await DownloadSeriesIfItDoesNotExist(symbol);
        
        if (!seriesResult.Success)
            return seriesResult;
        
        RowOpResult result = new RowOpResult();
        List<Category> categores = await fredClient.GetCategoriesForSeries(symbol);
        
        if (categores?.Any() ?? false)
        {
            foreach (Category category in categores)
            {
                SeriesCategory seriesCategory = new SeriesCategory { Symbol = symbol, CategoryID = category.NativeID };
                await SaveSeriesCategory(seriesCategory, false);
            }
            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadSeriesRelease(string symbol)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(symbol);
        RowOpResult result = new RowOpResult();
        Release? release = await fredClient.GetReleaseForSeries(symbol);
        
        if (release is null)
        {
            result.Message = $"Release not found for series {symbol}";
            return result;
        }

        Series? series = db.Series.FirstOrDefault(x => x.Symbol == symbol);

        if (series is null)
        {
            RowOpResult seriesResult = await DownloadSeries(symbol, release.NativeID);

            if (!seriesResult.Success)
                return seriesResult;
        }
        else
        {
            series.ReleaseID = release.NativeID;
            await SaveSeries(series, true);
        }

        // release might already exist in which case we will get a dupe error.  Ignore it.
        await serviceManifest.ReleasesService.SaveRelease(release, true);
        result.Success = true;
        return result;
    }

    public async Task<RowOpResult> DownloadSeriesTags(string symbol)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(symbol);
        RowOpResult result = new RowOpResult();
        List<SeriesTag> seriesTags = await fredClient.GetSeriesTags(symbol);

        if (seriesTags?.Any() ?? false)
        {
            foreach (SeriesTag seriesTag in seriesTags)
                result = await SaveSeriesTag(seriesTag, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadSeriesIfItDoesNotExist(string symbol)
    {
        RowOpResult result = new RowOpResult();

        if (!await db.Series.AnyAsync(x => x.Symbol == symbol))
            return await DownloadSeries(symbol);

        result.Success = true;
        return result;
    }

    public async Task<RowOpResult> SaveSeries(Series series, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(series);
        RowOpResult result = new RowOpResult();
        
        if(string.IsNullOrEmpty(series.Symbol))
            throw new Exception($"{nameof(series.Symbol)} is required.");

        Series? dupe = await db.Series.FirstOrDefaultAsync(x => x.ID != series.ID && x.Symbol == series.Symbol );

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(series).State = series.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveSeriesCategory(SeriesCategory seriesCategory, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(seriesCategory);
        RowOpResult result = new RowOpResult();

        if (String.IsNullOrEmpty(seriesCategory.CategoryID))
            throw new Exception($"{nameof(SeriesCategory.CategoryID)} is required.");
        else if (string.IsNullOrEmpty(seriesCategory.Symbol))
            throw new Exception($"{nameof(SeriesCategory.Symbol)} is required.");

        SeriesCategory? dupe = await db.SeriesCategories.FirstOrDefaultAsync(x => x.ID != seriesCategory.ID  && seriesCategory.CategoryID == x.CategoryID && x.Symbol == seriesCategory.Symbol);

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(seriesCategory).State = seriesCategory.ID == 0 ? EntityState.Added : EntityState.Modified;
            
            if (saveChanges)
                await db.SaveChangesAsync();
            
            result.Success = true;
        }
        
        return result;
    }

    public async Task<RowOpResult> SaveSeriesTag(SeriesTag seriesTag, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(seriesTag);

        if (string.IsNullOrEmpty(seriesTag.Symbol))
            throw new Exception($"{nameof(seriesTag.Symbol)} is required.");
        else if (string.IsNullOrEmpty(seriesTag.Name))
            throw new Exception($"{nameof(seriesTag.Name)} is required.");
        else if (string.IsNullOrEmpty(seriesTag.GroupID))
            throw new Exception($"{nameof(seriesTag.GroupID)} is required.");

        RowOpResult result = new RowOpResult();
        SeriesTag? dupe = db.SeriesTags.FirstOrDefault(x => x.ID != seriesTag.ID && x.Name == seriesTag.Name && x.GroupID == seriesTag.GroupID);

        if (dupe != null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(seriesTag).State = seriesTag.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }
}
