

namespace LeaderAnalytics.Observer.Fred.Services.Tests;

public class SeriesServiceTests: BaseTest
{
    public SeriesServiceTests(string currentProviderName) : base(currentProviderName)
    { 
    }

    [Test]
    public async Task DownloadSeriesTest()
    {
        string id = "GNP";
        RowOpResult result = await client.CallAsync(x => x.SeriesService.DownloadSeries(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.Series.Count(x => x.Symbol == id), Is.EqualTo(1));
    }

    [Test]
    public async Task DownloadCategoriesForSeriesTest()
    {
        string id = "EXJPUS";
        RowOpResult result = await client.CallAsync(x => x.SeriesService.DownloadCategoriesForSeries(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.SeriesCategories.Count(x => x.Symbol == id), Is.EqualTo(2));
    }

    [Test]
    public async Task DownloadSeriesReleaseTest()
    {
        string id = "IRA";
        RowOpResult result = await client.CallAsync(x => x.SeriesService.DownloadSeriesRelease(id));
        Assert.IsTrue(result.Success);
        Series s = db.Series.First(x => x.Symbol == id);
        Assert.That(s, Is.Not.Null);
        Assert.That(s.ReleaseID, Is.Not.Null);
    }

    [Test]
    public async Task DownloadSeriesTagsTest()
    {
        string id = "STLFSI";
        RowOpResult result = await client.CallAsync(x => x.SeriesService.DownloadSeriesTags(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.SeriesTags.Count(x => x.Symbol == id), Is.GreaterThan(0));
    }
}
