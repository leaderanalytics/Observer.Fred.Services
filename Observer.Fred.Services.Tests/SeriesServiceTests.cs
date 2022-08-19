namespace LeaderAnalytics.Observer.Fred.Services.Tests;


public class SeriesServiceTests: BaseTest
{
    [Test]
    public async Task DownloadSeriesCategoriesForCategoryTest()
    {
        string id = "125";
        RowOpResult result = await client.CallAsync(x => x.SeriesService.DownloadSeriesCategoriesForCategory(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.SeriesCategories.Count(x => x.CategoryID == id).Equals(47));
    }
}
