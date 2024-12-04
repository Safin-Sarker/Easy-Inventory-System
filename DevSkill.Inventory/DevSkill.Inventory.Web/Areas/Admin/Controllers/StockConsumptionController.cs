using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Rotativa.AspNetCore;
using System.Collections;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class StockConsumptionController : Controller
    {
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IStockManagementService _stockManagementService;
        private readonly IStockConsumptionManagementService _stockConsumptionManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockConsumptionController> _logger;

        public StockConsumptionController(IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IStockManagementService stockManagementService,
            IStockConsumptionManagementService stockConsumptionManagementService,
            IMapper mapper,
            ILogger<StockConsumptionController> logger)
        {
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _stockManagementService = stockManagementService;
            _stockConsumptionManagementService = stockConsumptionManagementService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "CanViewConsumption")]
        public IActionResult Index()
        {
            var model = new StockConsumptionCreateModel();

            return View(model);
        }



        public async Task<JsonResult> StockConsumptionJsonData([FromBody] StockConsumptionListModel model)
        {

            var result = await _stockConsumptionManagementService.GetStockConsumptionsAsync(model.PageIndex, model.PageSize, model.Search,
                  model.FormatSortExpression("VoucherNumber"));


            var ItemJsonData = new
            {

                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.VoucherNumber),
                                record.VoucherDate.ToString("yyyy-MM-dd"),
                                HttpUtility.HtmlEncode(record.Warehouse?.Name),
                                HttpUtility.HtmlEncode(record?.Details),
                                HttpUtility.HtmlEncode(record.CreatedBy),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(ItemJsonData);


        }

        [HttpGet]
        [Route("/Admin/StockConsumption/GetItemsByWarehouse")]

        public async Task<IActionResult> GetItemsByWarehouse(Guid warehouseId)
        {
            try
            {
                var items = await _stockManagementService.GetItemsForWarehouseAsync(warehouseId);

                return Json(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching items for warehouse {WarehouseId}", warehouseId);
                return StatusCode(500, "Error fetching items.");
            }
        }


        [Authorize(Policy = "CanCreateConsumption")]
        public async Task<IActionResult> Create()
        {
            var items = await _itemManagementService.GetAllAsync();

            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            var voucherNumber = await _stockConsumptionManagementService.GetVoucherNumberAsync();

            var viewModel = new StockConsumptionCreateModel
            {
                VoucherNumber = voucherNumber,
                VoucherDate = DateTime.Now,
                Items = new SelectList(items, "Id", "Name"), 
                Warehouses = new SelectList(warehouses, "Id", "Name"),
                CreatedBy = User.Identity.Name                     
            };

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken,Authorize(Policy = "CanCreateConsumption")]
        public async Task<IActionResult> Create(StockConsumptionCreateModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new StockConsumption entity based on the ViewModel
                var stockConsumption = new StockConsumption
                {
                    Id = Guid.NewGuid(),
                    VoucherNumber = viewModel.VoucherNumber,
                    VoucherDate = viewModel.VoucherDate,
                    WarehouseId = viewModel.WarehouseId,
                    Details = viewModel.Details,
                    CreatedBy = viewModel.CreatedBy,

                    StockConsumeds = viewModel.StockConsumeds.Select(consumed => new StockConsumed
                    {
                        Id = Guid.NewGuid(),
                        StockConsumptionId = Guid.NewGuid(),
                        ItemId = consumed.ItemId,
                        Quantity = consumed.Quantity,
                        UnitPrice = consumed.UnitPrice
                    }).ToList()   
                };

                try
                {
                    var result = await _stockConsumptionManagementService.CreateStockConsumptionAsync(stockConsumption);


                    if (result)
                    {

                        await _stockConsumptionManagementService.AddStockConsumptionAsync(stockConsumption);

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                             Message = "Stock Consumption Created Successfully",
                             Type = ResponseType.success
                        });

                        return RedirectToAction("Index");
                    }

                   
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                      {
                          Message = "Stock Consumption Creation Failed because of Insufficient stock for the selected item and warehouse",
                          Type = ResponseType.Danger
                      });

                    await PopulateViewModelAsync(viewModel);  
                    return View(viewModel);
                }
            }

            TempData.Put("ResponseMessage", new ResponseModel
               {
                  Message = "Stock Consumption Creation Failed",
                    Type = ResponseType.Danger
               });

            await PopulateViewModelAsync(viewModel);
            return View(viewModel);
        }


        [HttpGet, Authorize(Policy = "CanGetConsumptionDetails")]
        public async Task<IActionResult> Details(Guid id)
        {
            var stockConsumption = await _stockConsumptionManagementService.GetIdByAsync(id);

            if (stockConsumption == null)
            {
                return NotFound();
            }

            var viewModel = new StockconsumptionDeailsModel
            {
                Id=stockConsumption.Id,
                VoucherNumber = stockConsumption.VoucherNumber,
                VoucherDate = stockConsumption.VoucherDate,
                WarehouseName = stockConsumption.Warehouse?.Name,
                CreatedBy = stockConsumption.CreatedBy,
                Details = stockConsumption.Details,
                StockConsumeds = stockConsumption.StockConsumeds.Select(sc => new StockConsumedDetailsModel
                {
                    ItemName = sc.Item.Name,
                    Quantity = sc.Quantity,
                    UnitPrice = sc.UnitPrice
                }).ToList()
            };

            return View(viewModel);  
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanDeleteConsumption")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _stockConsumptionManagementService.DeleteItemAsync(id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Stock Consumption Deleted Successfully",
                    Type = ResponseType.success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Stock Consumption Delete Failed",
                    Type = ResponseType.Danger
                });

                _logger.LogError(ex, "Product Delete Failed");
            }

            return View();
        }


        [HttpGet, Authorize(Policy = "CanGenerateConsumptionPDF")]
        public async Task<IActionResult> ExportToPdf(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID");
            }
            var stockConsumption = await _stockConsumptionManagementService.GetIdByAsync(id);

            if (stockConsumption == null)
            {
                return NotFound();
            }

            var viewModel = new StockconsumptionDeailsModel
            {
                VoucherNumber = stockConsumption.VoucherNumber,
                VoucherDate = stockConsumption.VoucherDate,
                WarehouseName = stockConsumption.Warehouse?.Name,
                CreatedBy = stockConsumption.CreatedBy,
                Details = stockConsumption.Details,
                StockConsumeds = stockConsumption.StockConsumeds.Select(sc => new StockConsumedDetailsModel
                {
                    ItemName = sc.Item.Name,
                    Quantity = sc.Quantity,
                    UnitPrice = sc.UnitPrice
                }).ToList()
            };

            return new ViewAsPdf("ExportToPdf", viewModel)
            {
                FileName = $"StockIssue_{viewModel.VoucherNumber}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }








        private async Task PopulateViewModelAsync(StockConsumptionCreateModel viewModel)
            {
                var items = await _itemManagementService.GetAllAsync();
                var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

                viewModel.Items = new SelectList(items, "Id", "Name");
                viewModel.Warehouses = new SelectList(warehouses, "Id", "Name");
            }



    }
}

