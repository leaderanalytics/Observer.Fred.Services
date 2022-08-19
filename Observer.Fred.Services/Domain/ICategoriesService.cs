namespace LeaderAnalytics.Observer.Fred.Services.Domain;

public interface ICategoriesService
{
    Task<RowOpResult> DownloadCategory(string categoryID);
    Task<RowOpResult> DownloadCategoryChildren(string categoryID);
    Task<RowOpResult> DownloadCategoryTags(string categoryID);
    Task<RowOpResult> DownloadRelatedCategories(string parentID);
    Task<RowOpResult> SaveCategory(Category category, bool saveChanges = true);
    Task<RowOpResult> SaveCategoryTag(CategoryTag categoryTag, bool saveChanges = true);
}
