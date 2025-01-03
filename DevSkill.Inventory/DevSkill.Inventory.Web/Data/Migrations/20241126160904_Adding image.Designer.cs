﻿// <auto-generated />
using System;
using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    [DbContext(typeof(InventoryDbContext))]
    [Migration("20241126160904_Adding image")]
    partial class Addingimage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OpeningStock")
                        .HasColumnType("int");

                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReorderLevel")
                        .HasColumnType("int");

                    b.Property<bool?>("TrackInventory")
                        .HasColumnType("bit");

                    b.Property<string>("UnitOfMeasure")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Stock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockConsumed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("StockConsumptionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StockConsumptionId");

                    b.ToTable("StockConsumeds");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockConsumption", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("VoucherDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VoucherNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.ToTable("StockConsumptions");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockProduced", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("StockProductionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("StockProductionId");

                    b.ToTable("StockProduceds");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockProduction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("VoucherDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VoucherNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.ToTable("StockProductions");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SourceWarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("VoucherDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VoucherNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SourceWarehouseId");

                    b.ToTable("StockTransfers");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransferItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DestinationWarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("StockTransferId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DestinationWarehouseId");

                    b.HasIndex("ProductId");

                    b.HasIndex("StockTransferId");

                    b.ToTable("StockTransferItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Manager")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Stock", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany("Stocks")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany("Stocks")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockConsumed", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockConsumption", "StockConsumption")
                        .WithMany("StockConsumeds")
                        .HasForeignKey("StockConsumptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("StockConsumption");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockConsumption", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockProduced", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockProduction", "StockProduction")
                        .WithMany("StockProducedItems")
                        .HasForeignKey("StockProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("StockProduction");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockProduction", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "SourceWarehouse")
                        .WithMany("SourceTransfers")
                        .HasForeignKey("SourceWarehouseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SourceWarehouse");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransferItem", b =>
                {
                    b.HasOne("DevSkill.Inventory.Domain.Entities.Warehouse", "DestinationWarehouse")
                        .WithMany("DestinationTransfers")
                        .HasForeignKey("DestinationWarehouseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.Item", "Product")
                        .WithMany("StockTransferItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DevSkill.Inventory.Domain.Entities.StockTransfer", "StockTransfer")
                        .WithMany("TransferItems")
                        .HasForeignKey("StockTransferId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DestinationWarehouse");

                    b.Navigation("Product");

                    b.Navigation("StockTransfer");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Item", b =>
                {
                    b.Navigation("StockTransferItems");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockConsumption", b =>
                {
                    b.Navigation("StockConsumeds");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockProduction", b =>
                {
                    b.Navigation("StockProducedItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.StockTransfer", b =>
                {
                    b.Navigation("TransferItems");
                });

            modelBuilder.Entity("DevSkill.Inventory.Domain.Entities.Warehouse", b =>
                {
                    b.Navigation("DestinationTransfers");

                    b.Navigation("SourceTransfers");

                    b.Navigation("Stocks");
                });
#pragma warning restore 612, 618
        }
    }
}
