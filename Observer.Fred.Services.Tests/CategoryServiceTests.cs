namespace LeaderAnalytics.Observer.Fred.Services.Tests;

public class CategoryServiceTests : BaseTest
{
    [Test]
    public async Task DownloadCategoryTest()
    {
        string id = "125";
        RowOpResult result = await client.CallAsync(x => x.CategoriesService.DownloadCategory(id));
        Assert.IsTrue(result.Success);
    }

    [Test]
    public async Task DownloadCategoryChildrenTest()
    {
        string id = "13";
        RowOpResult result = await client.CallAsync(x => x.CategoriesService.DownloadCategoryChildren(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.Categories.Count(x => x.ParentID == id).Equals(6));
    }

    [Test]
    public async Task DownloadRelatedCategoriesTest()
    {
        string id = "32073";
        RowOpResult result = await client.CallAsync(x => x.CategoriesService.DownloadRelatedCategories(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.RelatedCategories.Count(x => x.CategoryID == id).Equals(7));
    }

    [Test]
    public async Task DownloadCategoryTagsTest()
    {
        string id = "125";
        RowOpResult result = await client.CallAsync(x => x.CategoriesService.DownloadCategoryTags(id));
        Assert.IsTrue(result.Success);
        Assert.That(db.CategoryTags.Count(x => x.CategoryID == id).Equals(25));
    }
}