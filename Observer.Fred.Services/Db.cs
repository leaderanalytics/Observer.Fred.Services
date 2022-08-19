namespace LeaderAnalytics.Observer.Fred.Services;

public class Db_MSSQL : Db, IMigrationContext
{
    public Db_MSSQL(DbContextOptions options) : base(options)
    {

    }
}

public class Db_MySQL : Db, IMigrationContext
{
    public Db_MySQL(DbContextOptions options) : base(options)
    {

    }
}

public class Db : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTag> CategoryTags { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<RelatedCategory> RelatedCategories { get; set; }
    public DbSet<Release> Releases { get; set; }
    public DbSet<ReleaseDate> ReleaseDates { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<SeriesCategory> SeriesCategories { get; set; }
    public DbSet<SeriesTag> SeriesTags { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<SourceRelease> SourceReleases { get; set; }

    public Db(Func<IDbContextOptions> optionsFactory) : base(optionsFactory().Options)
    {
        InitalizeContext();
    }

    public Db(DbContextOptions options) : base(options)
    {
        // this constructor is required.
        InitalizeContext();
    }

    protected string GetDateTypeForProvider() => Database.IsSqlServer() ? "datetime2(0)" : Database.IsMySql() ? "datetime(0)" : throw new Exception("Database not recognized.");

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        // Category
        mb.Entity<Category>().Ignore(x => x.Series);
        mb.Entity<Category>().Ignore(x => x.Children);
        mb.Entity<Category>().Ignore(x => x.Related);
        mb.Entity<Category>().Ignore(x => x.CategoryTags);
        mb.Entity<Category>().HasIndex(x => x.NativeID);
        
        // CategoryTag
        mb.Entity<CategoryTag>().Property(x => x.CreatedDate).HasColumnType(GetDateTypeForProvider());
        mb.Entity<CategoryTag>().Ignore(x => x.CreatedDateString).Ignore(x => x.NativeID);
        mb.Entity<CategoryTag>().HasIndex(x => x.CategoryID);
        mb.Entity<CategoryTag>().HasIndex(x => x.GroupID);

        // Observation
        mb.Entity<Observation>().Ignore(x => x.Vintage).Ignore(x => x.VintageDateString).Ignore(x => x.ObsDateString);
        mb.Entity<Observation>().Property(x => x.ObsDate).HasColumnType(GetDateTypeForProvider());
        mb.Entity<Observation>().Property(x => x.VintageDate).HasColumnType(GetDateTypeForProvider());
        mb.Entity<Observation>().HasIndex(x => x.Symbol);
        mb.Entity<Observation>().HasIndex(x => x.ObsDate);
        mb.Entity<Observation>().HasIndex(x => x.VintageDate);

        // RelatedCategory

        // Release
        mb.Entity<Release>().Property(x => x.RTStart).HasColumnType(GetDateTypeForProvider());
        mb.Entity<Release>().HasIndex(x => x.NativeID);

        // ReleaseDate
        mb.Entity<ReleaseDate>().Property(x => x.DateReleased).HasColumnType(GetDateTypeForProvider());
        mb.Entity<ReleaseDate>().Ignore(x => x.DateReleaseString);
        mb.Entity<ReleaseDate>().HasIndex(x => x.ReleaseID);

        // Series
        mb.Entity<Series>().Property(x => x.RTStart).HasColumnType(GetDateTypeForProvider());
        mb.Entity<Series>().HasIndex(x => x.Symbol);

        // SeriesTag
        mb.Entity<SeriesTag>().Property(x => x.CreatedDate).HasColumnType(GetDateTypeForProvider());
        mb.Entity<SeriesTag>().Ignore(x => x.CreatedDateString).Ignore(x => x.NativeID);
        mb.Entity<SeriesTag>().HasIndex(x => x.Symbol);
        mb.Entity<SeriesTag>().HasIndex(x => x.GroupID);

        // Source
        mb.Entity<Source>().HasIndex(x => x.NativeID);

        // SourceRelease
        mb.Entity<SourceRelease>().HasKey(x => new { x.SourceNativeID, x.ReleaseNativeID });
    }

    protected virtual void InitalizeContext()
    {
        // https://blog.oneunicorn.com/2012/03/12/secrets-of-detectchanges-part-3-switching-off-automatic-detectchanges/
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        Database.SetCommandTimeout(360);
    }
}
