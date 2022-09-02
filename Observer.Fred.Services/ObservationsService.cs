namespace LeaderAnalytics.Observer.Fred.Services;

public class ObservationsService : BaseService, IObservationsService
{
    public ObservationsService(Db db, IObserverAPI_Manifest downloaderServices, IFredClient fredClient) : base(db, downloaderServices, fredClient)
    {
    }

    

    public async Task<RowOpResult> DownloadObservations(string symbol)
    {
        RowOpResult seriesResult = await serviceManifest.SeriesService.DownloadSeriesIfItDoesNotExist(symbol);
        
        if (!seriesResult.Success)
            return seriesResult;

        RowOpResult result = new RowOpResult();

        DateTime lastVintageDate = (await db.Observations.Where(x => x.Symbol == symbol).MaxAsync(x => (DateTime?)x.VintageDate)) ?? new DateTime(1776, 7, 4);
        List<Vintage> vintages = (await fredClient.GetVintageDates(symbol, lastVintageDate.AddDays(1)));

        if (vintages?.Any() ?? false)
        {
            List<Observation>  observations = await fredClient.GetObservations(symbol, vintages.Select(x => x.VintageDate)?.ToList());

            if (observations?.Any() ?? false)
            {
                await db.Observations.AddRangeAsync(observations);
                await db.SaveChangesAsync();
            }
        }
        result.Success = true;
        return result;
    }
}
