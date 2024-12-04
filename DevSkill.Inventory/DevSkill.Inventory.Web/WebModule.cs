using Autofac;
using DevSkill.Inventory.Application;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Application.Services.RoleManagementService;
using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Application.Services.StockProduction;
using DevSkill.Inventory.Application.Services.StockTransferManagementService;
using DevSkill.Inventory.Application.Services.UserManagement;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Web.Data;
using DevSkill.Inventory.Web.Models;

namespace DevSkill.Inventory.Web
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InventoryDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", connectionString)
                .WithParameter("migrationAssembly", migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ItemRepository>()
                .As<IItemRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InventoryUnitOfWork>()
                .As<IInventoryUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ItemManagementService>()
                .As<IItemManagementService>()
                .InstancePerLifetimeScope();


            builder.RegisterType<WarehouseRepository>()
              .As<IWarehouseRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<WarehouseManagementService>()
              .As<IWarehouseManagementService>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockManagementService>()
              .As<IStockManagementService>()
              .InstancePerLifetimeScope();

            builder.RegisterType<StockRepository>()
            .As<IStockRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<StockconsumptionRepository>()
             .As<IStockConsumptionRepository>()
             .InstancePerLifetimeScope();

            builder.RegisterType<StockConsumptionManagementService>()
            .As<IStockConsumptionManagementService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<StockProductionManagementService> ()
          .As<IStockProductionManagementService>()
          .InstancePerLifetimeScope();


            builder.RegisterType<StockProductionRepository>()
            .As<IStockProductionRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<StockTransferManagementService>()
             .As<IStockTransferManagementService>()
             .InstancePerLifetimeScope();


            builder.RegisterType<StockTransferRepository>()
            .As<IStockTransferRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<EmailUtility>()
             .As<IEmailUtility>()
             .InstancePerLifetimeScope();


            builder.RegisterType<RoleManagementService>()
             .As<IRoleManagementService>()
             .InstancePerLifetimeScope();


            builder.RegisterType<RoleRepository>()
             .As<IRoleRepository>()
             .InstancePerLifetimeScope();

            builder.RegisterType<UserManagementService>()
           .As<IUserManagementService>()
           .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
             .As<IUserRepository>()
             .InstancePerLifetimeScope();


        }
    }
}
