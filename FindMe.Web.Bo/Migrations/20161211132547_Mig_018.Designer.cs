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
    [Migration("20161211132547_Mig_018")]
    partial class Mig_018
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<long>("Category_Id")
                        .HasColumnName("Category_Id");

                    b.Property<long>("Client_Id")
                        .HasColumnName("Client_Id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(64);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

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

                    b.Property<string>("PhysAddress")
                        .HasColumnName("PhysAddress")
                        .HasMaxLength(512);

                    b.Property<long>("Region_Id")
                        .HasColumnName("Region_Id");

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Category_Id");

                    b.HasIndex("Name");

                    b.HasIndex("Region_Id");

                    b.HasIndex("Client_Id", "Code")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressImage", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Address_Id")
                        .HasColumnName("Address_Id");

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

                    b.Property<string>("ImgAlt")
                        .HasColumnName("ImgAlt")
                        .HasMaxLength(128);

                    b.Property<int>("ImgHeight")
                        .HasColumnName("ImgHeight");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnName("ImgUrl")
                        .HasMaxLength(256);

                    b.Property<int>("ImgWidth")
                        .HasColumnName("ImgWidth");

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

                    b.Property<string>("ThmbAlt")
                        .IsRequired()
                        .HasColumnName("ThmbAlt")
                        .HasMaxLength(128);

                    b.Property<int>("ThmbHeight")
                        .HasColumnName("ThmbHeight");

                    b.Property<string>("ThmbUrl")
                        .IsRequired()
                        .HasColumnName("ThmbUrl")
                        .HasMaxLength(256);

                    b.Property<int>("ThmbWidth")
                        .HasColumnName("ThmbWidth");

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Name", "ImgHeight", "ImgWidth")
                        .IsUnique();

                    b.ToTable("AddressImages");
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

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<short>("Status");

                    b.Property<DateTime?>("ToUtc")
                        .HasColumnName("ToUtc");

                    b.Property<short>("Type")
                        .HasColumnName("Type");

                    b.Property<string>("Url")
                        .HasColumnName("Url")
                        .HasMaxLength(256);

                    b.HasKey("ID");

                    b.HasIndex("Address_Id", "Type", "FromUtc")
                        .IsUnique();

                    b.ToTable("AddressLinks");
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

            modelBuilder.Entity("FindMe.Data.Models.Category", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<short>("Level")
                        .HasColumnName("Level");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<long?>("Parent_Id")
                        .HasColumnName("Parent_Id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnName("Path")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.HasIndex("Parent_Id");

                    b.ToTable("Categorys");
                });

            modelBuilder.Entity("FindMe.Data.Models.Client", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<string>("PhysAddress")
                        .HasColumnName("PhysAddress")
                        .HasMaxLength(512);

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("FindMe.Data.Models.Contact", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Client_Id")
                        .HasColumnName("Client_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnName("Value")
                        .HasMaxLength(512);

                    b.HasKey("ID");

                    b.HasIndex("UID")
                        .IsUnique();

                    b.HasIndex("Client_Id", "Name")
                        .IsUnique();

                    b.ToTable("Contacts");
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

            modelBuilder.Entity("FindMe.Data.Models.Region", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("Code")
                        .HasMaxLength(128);

                    b.Property<long>("Country_Id")
                        .HasColumnName("Country_Id");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<short>("Level")
                        .HasColumnName("Level");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<long>("Parent_Id")
                        .HasColumnName("Parent_Id");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnName("Path")
                        .HasMaxLength(128);

                    b.Property<int>("Seqn")
                        .HasColumnName("Seqn");

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Country_Id");

                    b.HasIndex("Name");

                    b.HasIndex("Parent_Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("FindMe.Data.Models.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

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
                        .HasMaxLength(256);

                    b.Property<string>("Data")
                        .HasColumnName("Data")
                        .HasMaxLength(2048);

                    b.Property<string>("Desc")
                        .HasColumnName("Desc")
                        .HasMaxLength(512);

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

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasMaxLength(128);

                    b.Property<short>("Status");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
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
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("EmailConfirmed");

                    b.Property<string>("FName")
                        .HasColumnName("FName")
                        .HasMaxLength(128);

                    b.Property<bool>("IsValidated")
                        .HasColumnName("IsValidated");

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
                        .HasMaxLength(256);

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

                    b.HasOne("FindMe.Data.Models.Client", "Client")
                        .WithMany("Addresses")
                        .HasForeignKey("Client_Id");

                    b.HasOne("FindMe.Data.Models.Region", "Region")
                        .WithMany("Addresses")
                        .HasForeignKey("Region_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressImage", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("AddressImages")
                        .HasForeignKey("Address_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.AddressLink", b =>
                {
                    b.HasOne("FindMe.Data.Models.Address", "Address")
                        .WithMany("AddressLinks")
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

            modelBuilder.Entity("FindMe.Data.Models.Category", b =>
                {
                    b.HasOne("FindMe.Data.Models.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("Parent_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.Contact", b =>
                {
                    b.HasOne("FindMe.Data.Models.Client", "Client")
                        .WithMany("Contacts")
                        .HasForeignKey("Client_Id");
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

            modelBuilder.Entity("FindMe.Data.Models.Region", b =>
                {
                    b.HasOne("FindMe.Data.Models.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("Country_Id");

                    b.HasOne("FindMe.Data.Models.Region", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("Parent_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.SysParDetail", b =>
                {
                    b.HasOne("FindMe.Data.Models.SysParMaster", "SysParMaster")
                        .WithMany("SysParDetails")
                        .HasForeignKey("SysParMaster_Id");
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
