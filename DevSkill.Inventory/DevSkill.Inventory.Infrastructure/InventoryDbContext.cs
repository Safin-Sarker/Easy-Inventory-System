using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Infrastructure
{
    public class InventoryDbContext:DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public InventoryDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }



        public DbSet<Item>Items { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Stock>Stocks { get; set; }
        public DbSet<StockConsumption> StockConsumptions { get; set; }
        public DbSet<StockConsumed> StockConsumeds { get; set; }
        public DbSet<StockProduction> StockProductions { get; set; }
        public DbSet<StockProduced> StockProduceds { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<StockTransferItem> StockTransferItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure StockTransfer to StockTransferItem relationship with NoAction delete behavior
            modelBuilder.Entity<StockTransfer>()
                .HasMany(st => st.TransferItems)
                .WithOne(sti => sti.StockTransfer)
                .HasForeignKey(sti => sti.StockTransferId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure SourceWarehouse relationship with NoAction delete behavior
            modelBuilder.Entity<StockTransfer>()
                .HasOne(st => st.SourceWarehouse)
                .WithMany(w => w.SourceTransfers)
                .HasForeignKey(st => st.SourceWarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure DestinationWarehouseId in StockTransferItem with NoAction delete behavior
            modelBuilder.Entity<StockTransferItem>()
                .HasOne(sti => sti.DestinationWarehouse)
                .WithMany(w => w.DestinationTransfers)
                .HasForeignKey(sti => sti.DestinationWarehouseId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StockTransferItem>()
                .HasOne(sti => sti.Product)
                .WithMany(p => p.StockTransferItems)
                .HasForeignKey(sti => sti.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Optional: Configure StockTransferId with NoAction if cascading delete on StockTransfer is not required
            modelBuilder.Entity<StockTransferItem>()
                .HasOne(sti => sti.StockTransfer)
                .WithMany(st => st.TransferItems)
                .HasForeignKey(sti => sti.StockTransferId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }



    }
}
