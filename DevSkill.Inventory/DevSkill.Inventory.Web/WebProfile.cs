using AutoMapper;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels;
using DevSkill.Inventory.Web.Areas.Admin.Models.User_Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<WarehouseCreateModel, Warehouse>()                    
                .ForMember(dest => dest.city,
                    opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src => src.ContactNumber));

            CreateMap<WarehouseUpdateModel, Warehouse>()
                .ForMember(dest => dest.city,
                    opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Phone,
                    opt => opt.MapFrom(src => src.ContactNumber));

            CreateMap<Warehouse, WarehouseUpdateModel>()
                .ForMember(dest => dest.Location, 
                    opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.ContactNumber, 
                    opt => opt.MapFrom(src => src.Phone));

            CreateMap<WarehouseDto, WarehouseDataModel>();

            CreateMap<ItemCreateModel, Item>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Item, ItemUpdateModel>()
              .ForMember(dest => dest.ExistingImagePath, opt => opt.MapFrom(src => src.Image)) 
              .ForMember(dest => dest.TrackInventory, opt => opt.MapFrom(src => src.TrackInventory ?? false));

            CreateMap<ItemUpdateModel, Item>()
            .ForMember(dest => dest.Image, opt => opt.Ignore()) 
            .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<AddUserModel, ApplicationUser>();

            CreateMap<ApplicationUser, EditUserModel>()
              .ForMember(dest => dest.ContactNo, opt => opt.MapFrom(src => src.PhoneNumber))
              .ReverseMap();


            CreateMap<StockTransferCreateModel, StockTransfer>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.TransferItems, opt => opt.MapFrom(src => src.TransferItems));

            // Map TransferItem to StockTransferItem
            CreateMap<StockTransferItemCreateModel, StockTransferItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DestinationWarehouseId, opt => opt.MapFrom(src => src.DestinationWarehouseId));

            CreateMap<StockTransfer, StockTransferDetailsModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VoucherNumber, opt => opt.MapFrom(src => src.VoucherNumber))
            .ForMember(dest => dest.VoucherDate, opt => opt.MapFrom(src => src.VoucherDate))
            .ForMember(dest => dest.SourceWarehouseId, opt => opt.MapFrom(src => src.SourceWarehouseId))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ForMember(dest => dest.SourceWarehouseName, opt => opt.MapFrom(src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : string.Empty))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.TransferItems, opt => opt.MapFrom(src => src.TransferItems));

            CreateMap<StockTransferItem, StockTransferItemsDetailsModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DestinationWarehouseId, opt => opt.MapFrom(src => src.DestinationWarehouseId))
                .ForMember(dest => dest.DestinationWarehouseName, opt => opt.MapFrom(src => src.DestinationWarehouse != null ? src.DestinationWarehouse.Name : string.Empty));


            // Map StockTransfer to StockTransferDetailsModel
            CreateMap<StockTransfer, StockTransferDetailsModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.VoucherNumber, opt => opt.MapFrom(src => src.VoucherNumber))
                .ForMember(dest => dest.VoucherDate, opt => opt.MapFrom(src => src.VoucherDate))
                .ForMember(dest => dest.SourceWarehouseName, opt => opt.MapFrom(src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : string.Empty))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.TransferItems, opt => opt.MapFrom(src => src.TransferItems));

            // Map StockTransferItem to StockTransferItemsDetailsModel
            CreateMap<StockTransferItem, StockTransferItemsDetailsModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.DestinationWarehouseName, opt => opt.MapFrom(src => src.DestinationWarehouse != null ? src.DestinationWarehouse.Name : string.Empty));







        }
    }
}
