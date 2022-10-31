﻿// <auto-generated />
using System;
using LeaderAnalytics.Observer.Fred.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LeaderAnalytics.Observer.Fred.Services.Database.Migrations.MySQL
{
    [DbContext(typeof(Db_MySQL))]
    partial class Db_MySQLModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("NativeID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ParentID")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("NativeID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.CategoryTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("GroupID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.Property<int>("Popularity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("GroupID");

                    b.ToTable("CategoryTags");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Observation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ObsDate")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("VintageDate")
                        .HasColumnType("datetime(0)");

                    b.HasKey("ID");

                    b.HasIndex("ObsDate");

                    b.HasIndex("Symbol");

                    b.HasIndex("VintageDate");

                    b.ToTable("Observations");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.RelatedCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("RelatedCategoryID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.ToTable("RelatedCategories");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Release", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsPressRelease")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Link")
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("NativeID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RTStart")
                        .HasColumnType("datetime(0)");

                    b.HasKey("ID");

                    b.HasIndex("NativeID");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.ReleaseDate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateReleased")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("ReleaseID")
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("ReleaseID");

                    b.ToTable("ReleaseDates");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Series", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Frequency")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastUpdated")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.Property<int>("Popularity")
                        .HasColumnType("int");

                    b.Property<DateTime>("RTStart")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("ReleaseID")
                        .HasColumnType("longtext");

                    b.Property<string>("SeasonalAdj")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("Units")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("Symbol");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.SeriesCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ID");

                    b.ToTable("SeriesCategories");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.SeriesTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(0)");

                    b.Property<string>("GroupID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.Property<int>("Popularity")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ID");

                    b.HasIndex("GroupID");

                    b.HasIndex("Symbol");

                    b.ToTable("SeriesTags");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Source", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<string>("NativeID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("NativeID");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.SourceRelease", b =>
                {
                    b.Property<string>("SourceNativeID")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ReleaseNativeID")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ReleaseID")
                        .HasColumnType("int");

                    b.Property<int?>("SourceID")
                        .HasColumnType("int");

                    b.HasKey("SourceNativeID", "ReleaseNativeID");

                    b.HasIndex("ReleaseID");

                    b.HasIndex("SourceID");

                    b.ToTable("SourceReleases");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.SourceRelease", b =>
                {
                    b.HasOne("LeaderAnalytics.Vyntix.Fred.Model.Release", null)
                        .WithMany("SourceReleases")
                        .HasForeignKey("ReleaseID");

                    b.HasOne("LeaderAnalytics.Vyntix.Fred.Model.Source", null)
                        .WithMany("SourceReleases")
                        .HasForeignKey("SourceID");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Release", b =>
                {
                    b.Navigation("SourceReleases");
                });

            modelBuilder.Entity("LeaderAnalytics.Vyntix.Fred.Model.Source", b =>
                {
                    b.Navigation("SourceReleases");
                });
#pragma warning restore 612, 618
        }
    }
}