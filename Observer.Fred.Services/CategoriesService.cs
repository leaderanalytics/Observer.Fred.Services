namespace LeaderAnalytics.Observer.Fred.Services;

public class CategoriesService : BaseService, ICategoriesService
{
    public CategoriesService(Db db, IObserverAPI_Manifest downloaderServices, IFredClient fredClient) : base(db, downloaderServices, fredClient)
    {

    }

    public async Task<RowOpResult> DownloadCategory(string categoryID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(categoryID);
        RowOpResult result = new RowOpResult();
        Category category = await fredClient.GetCategory(categoryID);
        
        if (category != null)
            result = await SaveCategory(category);

        return result;
    }

    public async Task<RowOpResult> DownloadCategoryChildren(string categoryID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(categoryID);
        RowOpResult result = new RowOpResult();
        List<Category> categories = await fredClient.GetCategoryChildren(categoryID);

        if (categories?.Any() ?? false)
        {
            foreach (Category category in categories)
                result = await SaveCategory(category, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadRelatedCategories(string parentID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(parentID);
        RowOpResult result = new RowOpResult();
        List<RelatedCategory> relatedCategories = await fredClient.GetRelatedCategories(parentID);

        if (relatedCategories?.Any() ?? false)
        {
            foreach (RelatedCategory r in relatedCategories)
                await SaveRelatedCategory(r, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> DownloadCategorySeries(string categoryID)
    {
        ArgumentNullException.ThrowIfNull(categoryID);
        RowOpResult result = new RowOpResult();
        Category? category = await db.Categories.FirstOrDefaultAsync(x => x.NativeID == categoryID);

        if (category is null)
        {
            RowOpResult categoryResult = await DownloadCategory(categoryID);

            if (!categoryResult.Success)
                return categoryResult;
        }

        List<Series> series = await fredClient.GetSeriesForCategory(categoryID, true);

        if (series?.Any() ?? false)
        {
            foreach (Series s in series)
            {
                await serviceManifest.SeriesService.SaveSeries(s, false);
                await serviceManifest.SeriesService.SaveSeriesCategory(new SeriesCategory { Symbol = s.Symbol, CategoryID = categoryID }, false);
            }

            await db.SaveChangesAsync();
        }
        result.Success = true;
        return result;
    }

    public async Task<RowOpResult> DownloadCategoryTags(string categoryID)
    {
        ExtensionMethods.ThrowIfNullOrEmpty(categoryID);
        RowOpResult result = new RowOpResult();
        List<CategoryTag> categoryTags = await fredClient.GetCategoryTags(categoryID);

        if (categoryTags?.Any() ?? false)
        {
            foreach(CategoryTag categoryTag in categoryTags)
                result = await SaveCategoryTag(categoryTag, false);

            await db.SaveChangesAsync();
            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveCategory(Category category, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(category);
        
        if (string.IsNullOrEmpty(category.NativeID))
            throw new Exception($"{nameof(category.NativeID)} is required.");
        else if(string.IsNullOrEmpty(category.Name))
            throw new Exception($"{nameof(category.Name)} is required.");

        RowOpResult result = new RowOpResult();
        Category? dupe = db.Categories.FirstOrDefault(x => x.ID != category.ID && x.NativeID == category.NativeID);

        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(category).State = category.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveCategoryTag(CategoryTag categoryTag, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(categoryTag);

        if (string.IsNullOrEmpty(categoryTag.CategoryID))
            throw new Exception($"{nameof(categoryTag.CategoryID)} is required.");
        else if (string.IsNullOrEmpty(categoryTag.Name))
            throw new Exception($"{nameof(categoryTag.Name)} is required.");
        else if (string.IsNullOrEmpty(categoryTag.GroupID))
            throw new Exception($"{nameof(categoryTag.GroupID)} is required.");

        RowOpResult result = new RowOpResult();
        CategoryTag? dupe = db.CategoryTags.FirstOrDefault(x => x.ID != categoryTag.ID && x.Name == categoryTag.Name && x.GroupID == categoryTag.GroupID);

        if (dupe != null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(categoryTag).State = categoryTag.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();

            result.Success = true;
        }
        return result;
    }

    public async Task<RowOpResult> SaveRelatedCategory(RelatedCategory category, bool saveChanges = true)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (string.IsNullOrEmpty(category.CategoryID))
            throw new Exception($"{nameof(category.CategoryID)}  is required.");
        else if (string.IsNullOrEmpty(category.RelatedCategoryID))
            throw new Exception($"{nameof(category.RelatedCategoryID)}  is required.");

        RowOpResult result = new RowOpResult();
        RelatedCategory? dupe = db.RelatedCategories.FirstOrDefault(x => x.CategoryID == category.CategoryID && x.RelatedCategoryID == category.RelatedCategoryID);
        
        if (dupe is not null)
            result.Message = $"Duplicate with ID {dupe.ID} was found.";
        else
        {
            db.Entry(category).State = category.ID == 0 ? EntityState.Added : EntityState.Modified;

            if (saveChanges)
                await db.SaveChangesAsync();
            
            result.Success = true;
        }
     
        return result;
    }
}
