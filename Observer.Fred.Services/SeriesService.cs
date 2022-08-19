namespace LeaderAnalytics.Observer.Fred.Services;

public class SeriesService : BaseService, ISeriesService
{
    public SeriesService(Db db, IObserverAPI_Manifest serviceManifest, IFredClient fredClient) : base(db, serviceManifest, fredClient)
    {

    }

    public async Task<IEnumerable<Series>> GetLocalSeries(int skip, int take, string searchTitle)
    {
        return await db.Series.Where(x => string.IsNullOrEmpty(searchTitle) || EF.Functions.Like(x.Title, $"%{searchTitle}%"))
            .OrderBy(x => x.Title).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<RowOpResult> SaveLocalSeries(string symbol)
    {
        // this is probably obsolete
        // see saveseries below

        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentNullException(nameof(symbol));

        symbol = symbol.ToUpper();
        RowOpResult result = new RowOpResult();
        Series series = await fredClient.GetSeries(symbol);

        if (series == null)
        {
            result.Message = $"Invalid symbol: {symbol}";
            return result;
        }

        Series dupe = await db.Series.FirstOrDefaultAsync(x => x.Symbol == symbol);

        if (dupe != null)
        {
            series.ID = dupe.ID;
            db.Entry(series).State = EntityState.Modified;
        }
        else
            db.Entry(series).State = EntityState.Added;

        await db.SaveChangesAsync();

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


    public async Task<RowOpResult> DownloadSeriesCategoriesForCategory(string categoryID)
    {
        ArgumentNullException.ThrowIfNull(categoryID);
        RowOpResult result = new RowOpResult();
        List<SeriesCategory> seriesCategories = await fredClient.GetSeriesForCategory(categoryID, true);

        if (seriesCategories?.Any() ?? false)
        {
            foreach (SeriesCategory s in seriesCategories)
                await SaveSeriesCategory(s, false);

            await db.SaveChangesAsync();
        }
        result.Success = true;
        return result;
    }

    public async Task<RowOpResult> DeleteLocalSeries(string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentNullException(nameof(symbol));

        RowOpResult result = new RowOpResult();
        symbol = symbol.ToUpper();
        Series series = await db.Series.FirstOrDefaultAsync(x => x.Symbol == symbol);

        if (series == null)
        {
            result.Message = $"Invalid symbol: {symbol}";
            return result;
        }
        await serviceManifest.ObservationsService.DeleteLocalObservations(symbol);
        // ObservationsService calls SaveChanges.
        result.Success = true;
        return result;
    }

    public async Task<IEnumerable<string>> GetLocalSeriesSymbols()
    {
        return await db.Series.Select(x => x.Symbol).ToArrayAsync();
    }

    public async Task<RowOpResult> SaveSeriesCategory(SeriesCategory seriesCategory, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(seriesCategory);
        RowOpResult result = new RowOpResult();

        if (String.IsNullOrEmpty(seriesCategory.CategoryID))
            throw new Exception($"{nameof(SeriesCategory.CategoryID)} is required.");
        else if (string.IsNullOrEmpty(seriesCategory.Symbol))
            throw new Exception($"{nameof(SeriesCategory.Symbol)} is required.");
        else if (seriesCategory.ID != 0)
            throw new Exception("SeriesCategories can not be modified.  To change a SeriesCategory, delete the existing and create a new one.");

        SeriesCategory? dupe = await db.SeriesCategories.FirstOrDefaultAsync(x => seriesCategory.CategoryID == x.CategoryID && x.Symbol == seriesCategory.Symbol);

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
}
