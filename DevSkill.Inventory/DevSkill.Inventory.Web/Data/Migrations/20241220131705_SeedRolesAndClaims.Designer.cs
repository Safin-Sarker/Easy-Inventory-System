﻿// <auto-generated />
using System;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241220131705_SeedRolesAndClaims")]
    partial class SeedRolesAndClaims
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e"),
                            Name = "SuperAdmin",
                            NormalizedName = "SUPERADMIN"
                        },
                        new
                        {
                            Id = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c"),
                            Name = "Member",
                            NormalizedName = "MEMBER"
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = -1,
                            ClaimType = "Permission",
                            ClaimValue = "ItemCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -2,
                            ClaimType = "Permission",
                            ClaimValue = "ItemUpdate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -3,
                            ClaimType = "Permission",
                            ClaimValue = "ItemDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -4,
                            ClaimType = "Permission",
                            ClaimValue = "ItemGet",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -5,
                            ClaimType = "Permission",
                            ClaimValue = "ItemViewDetails",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -6,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -7,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionView",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -8,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -9,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionGetDetails",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -10,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionGeneratePDF",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -11,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -12,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionGet",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -13,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -14,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionGetDetails",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -15,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionGenerateReport",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -16,
                            ClaimType = "Permission",
                            ClaimValue = "TransferCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -17,
                            ClaimType = "Permission",
                            ClaimValue = "TransferView",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -18,
                            ClaimType = "Permission",
                            ClaimValue = "TransferDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -19,
                            ClaimType = "Permission",
                            ClaimValue = "TransferGetDetails",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -20,
                            ClaimType = "Permission",
                            ClaimValue = "TransferGenerateReport",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -21,
                            ClaimType = "Permission",
                            ClaimValue = "WarehouseCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -22,
                            ClaimType = "Permission",
                            ClaimValue = "WarehouseView",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -23,
                            ClaimType = "Permission",
                            ClaimValue = "WarehouseUpdate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -24,
                            ClaimType = "Permission",
                            ClaimValue = "WarehouseDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -25,
                            ClaimType = "Permission",
                            ClaimValue = "UserCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -26,
                            ClaimType = "Permission",
                            ClaimValue = "UserView",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -27,
                            ClaimType = "Permission",
                            ClaimValue = "UserUpdate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -28,
                            ClaimType = "Permission",
                            ClaimValue = "UserDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -29,
                            ClaimType = "Permission",
                            ClaimValue = "RoleCreate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -30,
                            ClaimType = "Permission",
                            ClaimValue = "RoleView",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -31,
                            ClaimType = "Permission",
                            ClaimValue = "RoleUpdate",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -32,
                            ClaimType = "Permission",
                            ClaimValue = "RoleDelete",
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        },
                        new
                        {
                            Id = -101,
                            ClaimType = "Permission",
                            ClaimValue = "ItemViewDetails",
                            RoleId = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c")
                        },
                        new
                        {
                            Id = -102,
                            ClaimType = "Permission",
                            ClaimValue = "ConsumptionView",
                            RoleId = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c")
                        },
                        new
                        {
                            Id = -103,
                            ClaimType = "Permission",
                            ClaimValue = "ProductionGet",
                            RoleId = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c")
                        },
                        new
                        {
                            Id = -104,
                            ClaimType = "Permission",
                            ClaimValue = "TransferView",
                            RoleId = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c")
                        },
                        new
                        {
                            Id = -105,
                            ClaimType = "Permission",
                            ClaimValue = "WarehouseView",
                            RoleId = new Guid("3c1ae0ba-096f-4ad8-ab20-36632634663c")
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermanentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PresentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "692ff217-eee3-48d4-a6ec-77a64c33cd30",
                            Email = "superadmin@example.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                            NormalizedUserName = "SUPERADMIN@EXAMPLE.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEB0RQMRnrY9/96LuU1GP/QnCmzRVHWZBUHhwH09A9QyPWCqnVcoBGa3GfNxmr5aOag==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f12b9222-1d56-4ca1-a966-17b52ad20a5d",
                            Status = "Active",
                            TwoFactorEnabled = false,
                            UserName = "superadmin@example.com"
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0"),
                            RoleId = new Guid("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                        });
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserToken", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationRoleClaim", b =>
                {
                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserClaim", b =>
                {
                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserLogin", b =>
                {
                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUserToken", b =>
                {
                    b.HasOne("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("DevSkill.Inventory.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}