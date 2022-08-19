using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaderAnalytics.Observer.Fred.Services.Database.Migrations.MSSQL
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParentID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VintageDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ObsDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RelatedCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RelatedCategoryID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateReleased = table.Column<DateTime>(type: "datetime2(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    IsPressRelease = table.Column<bool>(type: "bit", nullable: false),
                    RTStart = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Units = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SeasonalAdj = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdated = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Popularity = table.Column<int>(type: "int", nullable: false),
                    RTStart = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SeriesCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesCategories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SeriesTags",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    GroupID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesTags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SourceReleases",
                columns: table => new
                {
                    SourceNativeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleaseNativeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleaseID = table.Column<int>(type: "int", nullable: true),
                    SourceID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceReleases", x => new { x.SourceNativeID, x.ReleaseNativeID });
                    table.ForeignKey(
                        name: "FK_SourceReleases_Releases_ReleaseID",
                        column: x => x.ReleaseID,
                        principalTable: "Releases",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SourceReleases_Sources_SourceID",
                        column: x => x.SourceID,
                        principalTable: "Sources",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_NativeID",
                table: "Categories",
                column: "NativeID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTags_CategoryID",
                table: "CategoryTags",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTags_GroupID",
                table: "CategoryTags",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ObsDate",
                table: "Observations",
                column: "ObsDate");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_Symbol",
                table: "Observations",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_VintageDate",
                table: "Observations",
                column: "VintageDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseDates_ReleaseID",
                table: "ReleaseDates",
                column: "ReleaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_NativeID",
                table: "Releases",
                column: "NativeID");

            migrationBuilder.CreateIndex(
                name: "IX_Series_Symbol",
                table: "Series",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesTags_GroupID",
                table: "SeriesTags",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesTags_Symbol",
                table: "SeriesTags",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_SourceReleases_ReleaseID",
                table: "SourceReleases",
                column: "ReleaseID");

            migrationBuilder.CreateIndex(
                name: "IX_SourceReleases_SourceID",
                table: "SourceReleases",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_NativeID",
                table: "Sources",
                column: "NativeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CategoryTags");

            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "RelatedCategories");

            migrationBuilder.DropTable(
                name: "ReleaseDates");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "SeriesCategories");

            migrationBuilder.DropTable(
                name: "SeriesTags");

            migrationBuilder.DropTable(
                name: "SourceReleases");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
