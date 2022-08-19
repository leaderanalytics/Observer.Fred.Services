namespace LeaderAnalytics.Observer.Fred.Services.Tests;

internal class ReleasesServiceTests : BaseTest
{
    [Test]
    public async Task DownloadAllReleasesTest()
    {
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadAllReleases());
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.Releases.Any());
    }

    [Test]
    public async Task DownloadAllReleaseDatesTest()
    {
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadAllReleaseDates());
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.ReleaseDates.Any());
    }

    [Test]
    public async Task DownloadReleaseTest()
    {
        string id = "53";
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadRelease(id));
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.Releases.Any(x => x.NativeID == id));
    }

    [Test]
    public async Task DownloadReleaseDatesTest()
    {
        string id = "82";
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadReleaseDates(id));
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.ReleaseDates.Any(x => x.ReleaseID == id));
    }

    [Test]
    public async Task DownloadReleaseSeriesTest()
    {
        string id = "51";
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadReleaseSeries(id));
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.Series.Any(x => x.ReleaseID == id));
    }

    [Test]
    public async Task DownloadReleaseSourcesTest()
    {
        string id = "51";
        RowOpResult result = await client.CallAsync(x => x.ReleasesService.DownloadReleaseSources(id));
        Assert.IsTrue(result.Success);
        Assert.IsTrue(db.SourceReleases.Any(x => x.ReleaseNativeID == id));
    }
}
