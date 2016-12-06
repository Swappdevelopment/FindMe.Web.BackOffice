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
    [Migration("20161206072444_Mig_010")]
    partial class Mig_010
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

            modelBuilder.Entity("FindMe.Data.Models.Role", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("Status");

                    b.Property<string>("UID")
                        .IsRequired()
                        .HasColumnName("UID")
                        .HasMaxLength(128);

                    b.HasKey("ID");

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

            modelBuilder.Entity("FindMe.Data.Models.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("AccessFailedCount");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreationTimeUtc");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("EmailConfirmed");

                    b.Property<bool>("IsValidated")
                        .HasColumnName("IsValidated");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("LockoutEnabled");

                    b.Property<DateTime?>("LockoutEndDateUtc")
                        .IsRequired()
                        .HasColumnName("LockoutEndDateUtc");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedTimeUtc");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnName("PasswordHash")
                        .HasMaxLength(64);

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

                    b.Property<bool>("Confirmed")
                        .HasColumnName("Confirmed");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnName("IPAddress")
                        .HasMaxLength(64);

                    b.Property<short>("Status");

                    b.Property<long>("User_Id")
                        .HasColumnName("User_Id");

                    b.HasKey("ID");

                    b.HasIndex("User_Id", "IPAddress")
                        .IsUnique();

                    b.ToTable("UserIPs");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserLogin", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("AttemptResult")
                        .HasColumnName("AttemptResult");

                    b.Property<long>("IPAddress_Id")
                        .HasColumnName("IPAddress_Id");

                    b.Property<short>("Status");

                    b.Property<DateTime>("TimeDate")
                        .HasColumnName("TimeDate");

                    b.HasKey("ID");

                    b.HasIndex("IPAddress_Id", "TimeDate")
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

            modelBuilder.Entity("FindMe.Data.Models.UserIP", b =>
                {
                    b.HasOne("FindMe.Data.Models.User", "User")
                        .WithMany("UserIPs")
                        .HasForeignKey("User_Id");
                });

            modelBuilder.Entity("FindMe.Data.Models.UserLogin", b =>
                {
                    b.HasOne("FindMe.Data.Models.UserIP", "IPAddress")
                        .WithMany("UserLogins")
                        .HasForeignKey("IPAddress_Id");
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
