namespace LeaderAnalytics.Observer.Fred.Services;

public class SeriesService : BaseService, ISeriesService
{
    public SeriesService(Db db, IAPI_Manifest serviceManifest, IFredClient fredClient) : base(db, serviceManifest, fredClient)
    {

    }

    public async Task<IEnumerable<Series>> GetLocalSeries(int skip, int take, string searchTitle)
    {
        return await db.Series.Where(x => string.IsNullOrEmpty(searchTitle) || EF.Functions.Like(x.Title, $"%{searchTitle}%"))
            .OrderBy(x => x.Title).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<RowOpResult> SaveLocalSeries(string symbol)
    {
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
}
