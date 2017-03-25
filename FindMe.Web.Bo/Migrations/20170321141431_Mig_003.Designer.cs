using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FindMe.Web.App;
using Swapp.Data;
using FindMe.Data;

namespace FindMe.Web.Bo.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20170321141431_Mig_003")]
    partial class Mig_003
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("FindMe.Data.Models.AccessToken", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("InvalidPsswrdFormat")
                        .HasColumnName("InvalidPsswrdFormat");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<long>("RefreshToken_Id")
                        .HasColumnName("RefreshToken_Id");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("RefreshToken_Id", "Value")
                        .IsUnique();

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("FindMe.Data.Models.Address", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("Category_Id")
                        .HasColumnName("Category_Id");

                    b.Property<long?>("CityDetail_Id")
                        .IsRequired()
                        .HasColumnName("CityDetail_Id");

                    b.Property<short>("ClientType")
                        .HasColumnName("ClientType");

                    b.Property<long>("Client_Id")
                        .HasColumnName("Client_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("FlgPassport")
                        .HasColumnName("FlgPassport");

                    b.Property<bool>("FlgRecByFbFans")
                        .HasColumnName("FlgRecByFbFans");

                    b.Property<string>("ImpID")
                        .HasColumnName("ImpID")
                        .HasMaxLength(32);

                    b.Property<bool>("IsImported");

                    b.Property<double?>("Latitude")
                        .HasColumnName("Latitude");

                    b.Property<double?>("Longitude")
                        .HasColumnName("Longitude");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Name_Comp")
                        .IsRequired()
                        .HasColumnName("Name_Comp")
                        .HasMaxLength(128);

                    b.Property<string>("Slug")
                        .HasColumnName("Slug")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.Property<long>("TripAdvisorID")
                        .HasColumnName("TripAdvisorID");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Category_Id");

                    b.HasIndex("CityDetail_Id");

                    b.HasIndex("Name_Comp");

                    b.HasIndex("Client_Id", "UID")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("FindMe.Data.Models.Address_LangDesc", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<long>("Language_Id")
                        .HasColumnName("Language_Id");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(4096);

                    b.HasKey("ID");

                    b.HasIndex("Language_Id");

                    b.HasIndex("Address_Id", "Language_Id")
                        .IsUnique();

                    b.ToTable("Address_LangDescs");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressContact", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<DateTime>("FromUtc")
                        .HasColumnName("FromUtc");

                    b.Property<short>("Group")
                        .HasColumnName("Group");

                    b.Property<bool>("IsImported");

                    b.Property<string>("Link")
                        .HasColumnName("Link")
                        .HasMaxLength(256);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<string>("Text")
                        .HasColumnName("Text")
                        .HasMaxLength(1024);

                    b.Property<DateTime?>("ToUtc")
                        .HasColumnName("ToUtc");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "FromUtc", "Name", "Seqn")
                        .IsUnique();

                    b.ToTable("AddressContacts");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressFile", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("Alt")
                        .HasColumnName("Alt")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnName("Format")
                        .HasMaxLength(32);

                    b.Property<int>("Height")
                        .HasColumnName("Height");

                    b.Property<bool>("IsDefault")
                        .HasColumnName("IsDefault");

                    b.Property<bool>("IsHidden")
                        .HasColumnName("IsHidden");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.Property<int>("Width")
                        .HasColumnName("Width");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "UID", "Height", "Width")
                        .IsUnique();

                    b.ToTable("AddressFiles");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressIsFeatured", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<DateTime>("FromUtc")
                        .HasColumnName("FromUtc");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<DateTime?>("ToUtc")
                        .HasColumnName("ToUtc");

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Type", "FromUtc")
                        .IsUnique();

                    b.ToTable("AddressIsFeatureds");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressLink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<DateTime>("FromUtc")
                        .HasColumnName("FromUtc");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.Property<DateTime?>("ToUtc")
                        .HasColumnName("ToUtc");

                    b.Property<short>("Type");

                    b.Property<string>("Url")
                        .HasColumnName("Url")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Name", "FromUtc")
                        .IsUnique();

                    b.ToTable("AddressLinks");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressOrder", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<bool>("IsImported");

                    b.Property<short>("Key")
                        .HasColumnName("Key");

                    b.Property<short>("Status");

                    b.Property<int>("Value")
                        .HasColumnName("Value");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Key")
                        .IsUnique();

                    b.ToTable("AddressOrders");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTag", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<bool>("IsMain")
                        .HasColumnName("IsMain");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<long>("Tag_Id")
                        .HasColumnName("Tag_Id");

                    b.HasKey("ID");

                    b.HasIndex("Tag_Id");

                    b.HasIndex("Address_Id", "Tag_Id")
                        .IsUnique();

                    b.ToTable("AddressTags");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressThumbnail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alt")
                        .HasColumnName("Alt")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnName("Format")
                        .HasMaxLength(32);

                    b.Property<int>("Height")
                        .HasColumnName("Height");

                    b.Property<long>("Image_Id")
                        .HasColumnName("Image_Id");

                    b.Property<bool>("IsImported");

                    b.Property<string>("Mode")
                        .IsRequired()
                        .HasColumnName("Mode")
                        .HasMaxLength(64);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<int>("Width")
                        .HasColumnName("Width");

                    b.HasKey("ID");

                    b.HasIndex("Image_Id", "Height", "Width")
                        .IsUnique();

                    b.ToTable("AddressThumbnails");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTripAdWidget", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<DateTime?>("EndDateUtc")
                        .HasColumnName("EndDateUtc");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<DateTime>("SttDateUtc")
                        .HasColumnName("SttDateUtc");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "SttDateUtc")
                        .IsUnique();

                    b.ToTable("AddressTripAdWidgets");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTripAdWidgetValue", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsImported");

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<long>("TripAdWidget_Id")
                        .HasColumnName("TripAdWidget_Id");

                    b.Property<string>("Value")
                        .HasColumnName("Value")
                        .HasMaxLength(512);

                    b.HasKey("ID");

                    b.HasIndex("TripAdWidget_Id", "Seqn")
                        .IsUnique();

                    b.ToTable("AddressTripAdWidgetValues");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BgFx")
                        .HasColumnName("BgFx")
                        .HasMaxLength(16);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("IconClass")
                        .HasColumnName("IconClass")
                        .HasMaxLength(64);

                    b.Property<string>("IconFx")
                        .HasColumnName("IconFx")
                        .HasMaxLength(16);

                    b.Property<bool>("IsImported");

                    b.Property<short>("Level")
                        .HasColumnName("Level");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<long?>("Parent_Id")
                        .HasColumnName("Parent_Id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnName("Path")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Parent_Id");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("Categorys");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category_Lang", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Category_Id")
                        .HasColumnName("Category_Id");

                    b.Property<string>("ColTag")
                        .IsRequired()
                        .HasColumnName("ColTag")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<long>("Language_Id")
                        .HasColumnName("Language_Id");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(128);

                    b.Property<string>("Value_Comp")
                        .IsRequired()
                        .HasColumnName("Value_Comp")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Language_Id");

                    b.HasIndex("Value_Comp");

                    b.HasIndex("Category_Id", "Language_Id", "ColTag")
                        .IsUnique();

                    b.ToTable("Category_Langs");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category_LangDesc", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Category_Id")
                        .HasColumnName("Category_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<long>("Language_Id")
                        .HasColumnName("Language_Id");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(4096);

                    b.HasKey("ID");

                    b.HasIndex("Language_Id");

                    b.HasIndex("Category_Id", "Language_Id")
                        .IsUnique();

                    b.ToTable("Category_LangDescs");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityDetail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<long>("District_Id")
                        .HasColumnName("District_Id");

                    b.Property<long?>("Group_Id")
                        .HasColumnName("Group_Id");

                    b.Property<bool>("IsImported");

                    b.Property<double>("Latitude")
                        .HasColumnName("Latitude");

                    b.Property<double>("Longitude")
                        .HasColumnName("Longitude");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Name_Comp")
                        .IsRequired()
                        .HasColumnName("Name_Comp")
                        .HasMaxLength(128);

                    b.Property<long>("Region_Id")
                        .HasColumnName("Region_Id");

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<string>("Slug")
                        .HasColumnName("Slug")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("District_Id");

                    b.HasIndex("Group_Id");

                    b.HasIndex("Name_Comp");

                    b.HasIndex("Region_Id");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("CityDetails");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityDistrict", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Country_Id")
                        .HasColumnName("Country_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<double>("Latitude")
                        .HasColumnName("Latitude");

                    b.Property<double>("Longitude")
                        .HasColumnName("Longitude");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Name_Comp")
                        .IsRequired()
                        .HasColumnName("Name_Comp")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Country_Id");

                    b.HasIndex("Name_Comp");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("CityDistricts");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityGroup", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Country_Id")
                        .HasColumnName("Country_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<double>("Latitude")
                        .HasColumnName("Latitude");

                    b.Property<double>("Longitude")
                        .HasColumnName("Longitude");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Name_Comp")
                        .IsRequired()
                        .HasColumnName("Name_Comp")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Country_Id");

                    b.HasIndex("Name_Comp");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("CityGroups");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityRegion", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Country_Id")
                        .HasColumnName("Country_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<double>("Latitude")
                        .HasColumnName("Latitude");

                    b.Property<double>("Longitude")
                        .HasColumnName("Longitude");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Country_Id");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("CityRegions");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityRegion_Lang", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColTag")
                        .IsRequired()
                        .HasColumnName("ColTag")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<long>("Language_Id")
                        .HasColumnName("Language_Id");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<long>("Region_Id")
                        .HasColumnName("Region_Id");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(128);

                    b.Property<string>("Value_Comp")
                        .IsRequired()
                        .HasColumnName("Value_Comp")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Language_Id");

                    b.HasIndex("Value_Comp");

                    b.HasIndex("Region_Id", "Language_Id", "ColTag")
                        .IsUnique();

                    b.ToTable("CityRegion_Langs");
                });

            modelBuilder.Entity("FindMe.Data.Models.Client", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Civility")
                        .HasColumnName("Civility")
                        .HasMaxLength(32);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<string>("FName")
                        .HasColumnName("FName")
                        .HasMaxLength(128);

                    b.Property<string>("FName_Comp")
                        .HasColumnName("FName_Comp")
                        .HasMaxLength(128);

                    b.Property<string>("ImpID")
                        .HasColumnName("ImpID")
                        .HasMaxLength(32);

                    b.Property<bool>("IsImported");

                    b.Property<string>("LName")
                        .HasColumnName("LName")
                        .HasMaxLength(128);

                    b.Property<string>("LName_Comp")
                        .HasColumnName("LName_Comp")
                        .HasMaxLength(128);

                    b.Property<string>("LegalName")
                        .IsRequired()
                        .HasColumnName("LegalName")
                        .HasMaxLength(128);

                    b.Property<string>("LegalName_Comp")
                        .IsRequired()
                        .HasColumnName("LegalName_Comp")
                        .HasMaxLength(128);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<bool>("Paid")
                        .HasColumnName("Paid");

                    b.Property<string>("PhysAddress")
                        .HasColumnName("PhysAddress")
                        .HasMaxLength(512);

                    b.Property<string>("RememberToken")
                        .HasColumnName("RememberToken")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("FName_Comp");

                    b.HasIndex("LName_Comp");

                    b.HasIndex("LegalName_Comp");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("FindMe.Data.Models.Country", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Capital")
                        .HasColumnName("Capital")
                        .HasMaxLength(32);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(32);

                    b.Property<string>("Currency")
                        .HasColumnName("Currency")
                        .HasMaxLength(32);

                    b.Property<bool>("IsImported");

                    b.Property<string>("Iso")
                        .HasColumnName("Iso")
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("PhoneCode")
                        .IsRequired()
                        .HasColumnName("PhoneCode")
                        .HasMaxLength(32);

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Countrys");
                });

            modelBuilder.Entity("FindMe.Data.Models.DayOpen", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<short>("Day")
                        .HasColumnName("Day");

                    b.Property<short>("HoursFrom")
                        .HasColumnName("HoursFrom");

                    b.Property<short>("HoursTo")
                        .HasColumnName("HoursTo");

                    b.Property<bool>("IsImported");

                    b.Property<short>("MinutesFrom")
                        .HasColumnName("MinutesFrom");

                    b.Property<short>("MinutesTo")
                        .HasColumnName("MinutesTo");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Day", "HoursFrom", "MinutesFrom")
                        .IsUnique();

                    b.ToTable("DaysOpen");
                });

            modelBuilder.Entity("FindMe.Data.Models.Idendity", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Client_Id")
                        .HasColumnName("Client_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<long>("IdendityRef_Id")
                        .HasColumnName("IdendityRef_Id");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.HasIndex("Client_Id");

                    b.HasIndex("IdendityRef_Id", "Client_Id")
                        .IsUnique();

                    b.ToTable("Idenditys");
                });

            modelBuilder.Entity("FindMe.Data.Models.IdendityRef", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<bool>("IsImported");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("IdendityRefs");
                });

            modelBuilder.Entity("FindMe.Data.Models.IPAddress", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Confirmed")
                        .HasColumnName("Confirmed");

                    b.Property<bool>("IsImported");

                    b.Property<short>("Status");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(64);

                    b.HasKey("ID");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("IPAddresses");
                });

            modelBuilder.Entity("FindMe.Data.Models.Language", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(32);

                    b.Property<bool>("IsImported");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("FindMe.Data.Models.Logging", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<string>("Exception")
                        .HasColumnName("Exception")
                        .HasMaxLength(4096);

                    b.Property<bool>("IsImported");

                    b.Property<short>("LogLevel")
                        .HasColumnName("LogLevel");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("Loggings");
                });

            modelBuilder.Entity("FindMe.Data.Models.Rating", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<long>("IPAddress_Id")
                        .HasColumnName("IPAddress_Id");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<short>("Value")
                        .HasColumnName("Value");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id");

                    b.HasIndex("IPAddress_Id", "Address_Id")
                        .IsUnique();

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("FindMe.Data.Models.RatingOverride", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

                    b.Property<int?>("ClickCount");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<DateTime>("FromUtc")
                        .HasColumnName("FromUtc");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<DateTime?>("ToUtc")
                        .HasColumnName("ToUtc");

                    b.Property<short>("Value")
                        .HasColumnName("Value");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "FromUtc")
                        .IsUnique();

                    b.ToTable("RatingOverrides");
                });

            modelBuilder.Entity("FindMe.Data.Models.RefreshToken", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("ClientType")
                        .HasColumnName("ClientType");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<DateTime>("LastActivityTime")
                        .HasColumnName("LastActivityTime");

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnName("LastLoginTime");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<long>("User_Id")
                        .HasColumnName("User_Id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("User_Id", "Value")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("FindMe.Data.Models.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsImported");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(64);

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("FindMe.Data.Models.SysParDetail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(64);

                    b.Property<string>("Data")
                        .HasColumnName("Data")
                        .HasMaxLength(2048);

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<bool>("Editable");

                    b.Property<bool>("IsImported");

                    b.Property<short>("Status");

                    b.Property<long>("SysParMaster_Id")
                        .HasColumnName("SysParMaster_Id");

                    b.HasKey("ID");

                    b.HasIndex("SysParMaster_Id", "Code")
                        .IsUnique();

                    b.ToTable("SysParDetails");
                });

            modelBuilder.Entity("FindMe.Data.Models.SysParMaster", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(128);

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<bool>("Editable");

                    b.Property<bool>("IsImported");

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("SysParMasters");
                });

            modelBuilder.Entity("FindMe.Data.Models.Tag", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("FindMe.Data.Models.Tag_Lang", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColTag")
                        .IsRequired()
                        .HasColumnName("ColTag")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<bool>("IsImported");

                    b.Property<long>("Language_Id")
                        .HasColumnName("Language_Id");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<long>("Tag_Id")
                        .HasColumnName("Tag_Id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(64);

                    b.Property<string>("Value_Comp")
                        .IsRequired()
                        .HasColumnName("Value_Comp")
                        .HasMaxLength(64);

                    b.HasKey("ID");

                    b.HasIndex("Language_Id");

                    b.HasIndex("Value_Comp");

                    b.HasIndex("Tag_Id", "Language_Id", "ColTag")
                        .IsUnique();

                    b.ToTable("Tag_Langs");
                });

            modelBuilder.Entity("FindMe.Data.Models.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("AccessFailedCount");

                    b.Property<string>("ContactNumber");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email")
                        .HasMaxLength(255);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("EmailConfirmed");

                    b.Property<string>("FName")
                        .HasColumnName("FName")
                        .HasMaxLength(128);

                    b.Property<bool>("IsEmailValidated")
                        .HasColumnName("IsEmailValidated");

                    b.Property<bool>("IsImported");

                    b.Property<string>("LName")
                        .HasColumnName("LName")
                        .HasMaxLength(128);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("LockoutEnabled");

                    b.Property<DateTime?>("LockoutEndDateUtc")
                        .HasColumnName("LockoutEndDateUtc");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("PasswordHash")
                        .HasMaxLength(512);

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnName("UserName")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UID")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserIP", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<long>("IPAddress_Id")
                        .HasColumnName("IPAddress_Id");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<long>("User_Id")
                        .HasColumnName("User_Id");

                    b.HasKey("ID");

                    b.HasIndex("IPAddress_Id");

                    b.HasIndex("User_Id", "IPAddress_Id")
                        .IsUnique();

                    b.ToTable("UserIPs");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserLogin", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AttemptResult")
                        .HasColumnName("AttemptResult");

                    b.Property<bool>("IsImported");

                    b.Property<short>("Status");

                    b.Property<DateTime>("TimeDate")
                        .HasColumnName("TimeDate");

                    b.Property<long>("UserIP_Id")
                        .HasColumnName("UserIP_Id");

                    b.HasKey("ID");

                    b.HasIndex("UserIP_Id");

                    b.HasIndex("TimeDate", "UserIP_Id")
                        .IsUnique();

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserRole", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsImported");

                    b.Property<long>("Role_Id")
                        .HasColumnName("Role_Id");

                    b.Property<short>("Status");

                    b.Property<long>("User_Id")
                        .HasColumnName("User_Id");

                    b.HasKey("ID");

                    b.HasIndex("Role_Id");

                    b.HasIndex("User_Id", "Role_Id")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserToken", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddData")
                        .HasColumnName("AddData")
                        .HasMaxLength(256);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<short>("EmailSentStatus")
                        .HasColumnName("EmailSentStatus");

                    b.Property<DateTime?>("EmailSentStatusTime")
                        .HasColumnName("EmailSentStatusTime");

                    b.Property<bool>("IsImported");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.Property<long?>("User_Id")
                        .HasColumnName("User_Id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnName("Value")
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("User_Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("FindMe.Data.Models.AccessToken", b =>
                {
                    b.HasOne("FindMe.Data.Models.RefreshToken", "RefreshToken")
                        .WithMany("AccessTokens")
                        .HasForeignKey("RefreshToken_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Address", b =>
                {
                    b.HasOne("FindMe.Data.Models.Category", "Category")
                        .WithMany("Addresses")
                        .HasForeignKey("Category_Id");

                    b.HasOne("FindMe.Data.Models.CityDetail", "CityDetail")
                        .WithMany("Addresses")
                        .HasForeignKey("CityDetail_Id");

                    b.HasOne("FindMe.Data.Models.Client", "Client")
                        .WithMany("Addresses")
                        .HasForeignKey("Client_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Address_LangDesc", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("LangDescs")
                        .HasForeignKey("Address_Id");

                    b.HasOne("FindMe.Data.Models.Language", "Language")
                        .WithMany("AddressLangDescs")
                        .HasForeignKey("Language_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressContact", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("Contacts")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressFile", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("Files")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressIsFeatured", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("IsFeatureds")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressLink", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("Links")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressOrder", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("Orders")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTag", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("AddressTags")
                        .HasForeignKey("Address_Id");

                    b.HasOne("FindMe.Data.Models.Tag", "Tag")
                        .WithMany("AddressTags")
                        .HasForeignKey("Tag_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressThumbnail", b =>
                {
                    b.HasOne("FindMe.Data.Models.AddressFile", "Image")
                        .WithMany("Thumbnails")
                        .HasForeignKey("Image_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTripAdWidget", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("TripAdWidgets")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressTripAdWidgetValue", b =>
                {
                    b.HasOne("FindMe.Data.Models.AddressTripAdWidget", "TripAdWidget")
                        .WithMany("Values")
                        .HasForeignKey("TripAdWidget_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category", b =>
                {
                    b.HasOne("FindMe.Data.Models.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("Parent_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category_Lang", b =>
                {
                    b.HasOne("FindMe.Data.Models.Category", "Category")
                        .WithMany("CategoryLangs")
                        .HasForeignKey("Category_Id");

                    b.HasOne("FindMe.Data.Models.Language", "Language")
                        .WithMany("CategoryLangs")
                        .HasForeignKey("Language_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Category_LangDesc", b =>
                {
                    b.HasOne("FindMe.Data.Models.Category", "Category")
                        .WithMany("CategoryLangDescs")
                        .HasForeignKey("Category_Id");

                    b.HasOne("FindMe.Data.Models.Language", "Language")
                        .WithMany("CategoryLangDescs")
                        .HasForeignKey("Language_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityDetail", b =>
                {
                    b.HasOne("FindMe.Data.Models.CityDistrict", "District")
                        .WithMany("CityDetails")
                        .HasForeignKey("District_Id");

                    b.HasOne("FindMe.Data.Models.CityGroup", "Group")
                        .WithMany("CityDetails")
                        .HasForeignKey("Group_Id");

                    b.HasOne("FindMe.Data.Models.CityRegion", "Region")
                        .WithMany("CityDetails")
                        .HasForeignKey("Region_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityDistrict", b =>
                {
                    b.HasOne("FindMe.Data.Models.Country", "Country")
                        .WithMany("CityDistricts")
                        .HasForeignKey("Country_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityGroup", b =>
                {
                    b.HasOne("FindMe.Data.Models.Country", "Country")
                        .WithMany("CityGroups")
                        .HasForeignKey("Country_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityRegion", b =>
                {
                    b.HasOne("FindMe.Data.Models.Country", "Country")
                        .WithMany("CityRegions")
                        .HasForeignKey("Country_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.CityRegion_Lang", b =>
                {
                    b.HasOne("FindMe.Data.Models.Language", "Language")
                        .WithMany("CityRegionLangs")
                        .HasForeignKey("Language_Id");

                    b.HasOne("FindMe.Data.Models.CityRegion", "Region")
                        .WithMany("CityRegionLangs")
                        .HasForeignKey("Region_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.DayOpen", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("DaysOpen")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Idendity", b =>
                {
                    b.HasOne("FindMe.Data.Models.Client", "Client")
                        .WithMany("Idenditys")
                        .HasForeignKey("Client_Id");

                    b.HasOne("FindMe.Data.Models.IdendityRef", "IdendityRef")
                        .WithMany("Idenditys")
                        .HasForeignKey("IdendityRef_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Rating", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("Ratings")
                        .HasForeignKey("Address_Id");

                    b.HasOne("FindMe.Data.Models.IPAddress", "IPAddress")
                        .WithMany("Ratings")
                        .HasForeignKey("IPAddress_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.RatingOverride", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("RatingOverrides")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.RefreshToken", b =>
                {
                    b.HasOne("FindMe.Data.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("User_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.SysParDetail", b =>
                {
                    b.HasOne("FindMe.Data.Models.SysParMaster", "SysParMaster")
                        .WithMany("SysParDetails")
                        .HasForeignKey("SysParMaster_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Tag_Lang", b =>
                {
                    b.HasOne("FindMe.Data.Models.Language", "Language")
                        .WithMany("TagLangs")
                        .HasForeignKey("Language_Id");

                    b.HasOne("FindMe.Data.Models.Tag", "Tag")
                        .WithMany("TagLangs")
                        .HasForeignKey("Tag_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserIP", b =>
                {
                    b.HasOne("FindMe.Data.Models.IPAddress", "IPAddress")
                        .WithMany("UserIPs")
                        .HasForeignKey("IPAddress_Id");

                    b.HasOne("FindMe.Data.Models.User", "User")
                        .WithMany("UserIPs")
                        .HasForeignKey("User_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserLogin", b =>
                {
                    b.HasOne("FindMe.Data.Models.UserIP", "UserIP")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserIP_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserRole", b =>
                {
                    b.HasOne("FindMe.Data.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("Role_Id");

                    b.HasOne("FindMe.Data.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("User_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserToken", b =>
                {
                    b.HasOne("FindMe.Data.Models.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("User_Id");
                });
        }
    }
}
