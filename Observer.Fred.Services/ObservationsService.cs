namespace LeaderAnalytics.Observer.Fred.Services;

public class ObservationsService : BaseService, IObservationsService
{
    public ObservationsService(Db db, IObserverAPI_Manifest downloaderServices, IFredClient fredClient) : base(db, downloaderServices, fredClient)
    {
    }

    public async Task<List<Observation>> GetLocalObservations(string symbol, int skip, int take)
    {
        return await ObservationsQuery(symbol)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<Observation>> GetLocalObservations(string symbol)
    {
        return await ObservationsQuery(symbol).ToListAsync();
    }

    public async Task<RowOpResult> UpdateLocalObservations(string symbol)
    {
        RowOpResult result = new RowOpResult();
        DateTime lastVintageDate = (await db.Observations.Where(x => x.Symbol == symbol).MaxAsync(x => (DateTime?)x.VintageDate)) ?? new DateTime(1776, 7, 4);
        List<Vintage> vintages = (await fredClient.GetVintageDates(symbol, lastVintageDate.AddDays(1)));
        List<Observation> observations = null;

        if (vintages?.Any() ?? false)
        {
            observations = await fredClient.GetObservations(symbol, vintages.Select(x => x.VintageDate)?.ToList());

            if (observations?.Any() ?? false)
            {
                await db.Observations.AddRangeAsync(observations);
                await db.SaveChangesAsync();
            }
        }
        result.Success = true;
        return result;
    }

    public async Task<RowOpResult> DeleteLocalObservations(string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentNullException(nameof(symbol));

        RowOpResult result = new RowOpResult();
        symbol = symbol.ToUpper();
        db.Observations.RemoveRange(db.Observations.Where(x => x.Symbol == symbol));
        await db.SaveChangesAsync();
        return result;
    }

    private IQueryable<Observation> ObservationsQuery(string symbol)
    {
        return from o in db.Observations
               where o.Symbol == symbol
               select o;
    }
}
