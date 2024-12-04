using AutoMapper;
using DevSkill.Inventory.Application.Services.StockConsumption;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockConsumption_Model;
using System.Web;
using DevSkill.Inventory.Application.Services.StockProduction;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockProduction;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class StockProductionController : Controller
    {
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IStockManagementService _stockManagementService;
        private readonly IStockProductionManagementService _stockproductionManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockProductionController> _logger;

        public StockProductionController(IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IStockManagementService stockManagementService,
            IStockProductionManagementService stockproductionManagementService,
            IMapper mapper,
            ILogger<StockProductionController> logger)
        {
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _stockManagementService = stockManagementService;
            _stockproductionManagementService = stockproductionManagementService;
            _mapper = mapper;
            _logger = logger;
        }


        [Authorize(Policy = "CanViewProduction")]
        public IActionResult Index()
        {
            var model = new StockProductionCreateModel();

            return View(model);
        }

        public async Task<JsonResult> StockProductionJsonData([FromBody] StockConsumptionListModel model)
        {

            var result = await _stockproductionManagementService.GetStockProductionsAsync(model.PageIndex, model.PageSize, model.Search,
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

       
        [Route("/Admin/StockProduction/GetItemsByWarehouse")]
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

        [Authorize(Policy = "CanCreateProduction")]
        public async Task<IActionResult> Create()
        {
            var items = await _itemManagementService.GetAllAsync();

            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            var voucherNumber = await _stockproductionManagementService.GetVoucherNumberAsync();

            var viewModel = new StockProductionCreateModel
            {
                VoucherNumber = voucherNumber,
                VoucherDate = DateTime.Now,
                Items = new SelectList(items, "Id", "Name"),
                Warehouses = new SelectList(warehouses, "Id", "Name"),
                CreatedBy = User.Identity.Name
            };

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken,Authorize(Policy = "CanCreateProduction")]
        public async Task<IActionResult> Create(StockProductionCreateModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new StockProduction entity based on the ViewModel
                var stockProduction = new StockProduction
                {
                    Id = Guid.NewGuid(),
                    VoucherNumber = viewModel.VoucherNumber,
                    VoucherDate = viewModel.VoucherDate,
                    WarehouseId = viewModel.WarehouseId,
                    Details = viewModel.Details,
                    CreatedBy = viewModel.CreatedBy,

                    StockProducedItems = viewModel.StockProducedItems.Select(produced => new StockProduced
                    {
                        Id = Guid.NewGuid(),
                        StockProductionId = Guid.NewGuid(),
                        ItemId = produced.ItemId,
                        Quantity = produced.Quantity,
                        UnitPrice = produced.UnitPrice
                    }).ToList()
                };

                try
                {
                    // Call the stock production management service to handle the business logic
                    var result = await _stockproductionManagementService.CreateStockProductionAsync(stockProduction);

                    if (result)
                    {
                        // Optionally add the record to the database if it's not handled in CreateStockProductionAsync
                        await _stockproductionManagementService.AddStockProductionAsync(stockProduction);

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Stock Production Created Successfully",
                            Type = ResponseType.success
                        });

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Stock Production Creation Failed due to an error: ",
                        Type = ResponseType.Danger
                    });

                    await PopulateViewModelAsync(viewModel);
                    return View(viewModel);
                }
            }

            // Handle the case where the model state is invalid
            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = "Stock Production Creation Failed",
                Type = ResponseType.Danger
            });

            await PopulateViewModelAsync(viewModel);
            return View(viewModel);
        }


        [HttpGet, Authorize(Policy = "CanGetProductionDetails")]
        public async Task<IActionResult> Details(Guid id)
        {
            var stockproduction = await _stockproductionManagementService.GetIdByAsync(id);

            if (stockproduction == null)
            {
                return NotFound();
            }

            var viewModel = new StockProductionDetailsModel
            {
                Id = stockproduction.Id,
                VoucherNumber = stockproduction.VoucherNumber,
                VoucherDate = stockproduction.VoucherDate,
                WarehouseName = stockproduction.Warehouse?.Name,
                CreatedBy = stockproduction.CreatedBy,
                Details = stockproduction.Details,
                StockProducedItems = stockproduction.StockProducedItems.Select(sc => new StockProducedDetailsModel
                {
                    ItemName = sc.Item.Name,
                    Quantity = sc.Quantity,
                    UnitPrice = sc.UnitPrice
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanDeleteProduction")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _stockproductionManagementService.DeleteItemAsync(id);

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

                _logger.LogError(ex, "Stock Consumption Delete Failed");
            }

            return View();
        }

        [HttpGet, Authorize(Policy = "CanGenerateProductionReport")]
        public async Task<IActionResult> ExportToPdf(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID");
            }
            // Fetch the stock consumption details by ID
            var stockproduction = await _stockproductionManagementService.GetIdByAsync(id);

            if (stockproduction == null)
            {
                return NotFound();
            }

            var viewModel = new StockProductionDetailsModel
            {
                Id = stockproduction.Id,
                VoucherNumber = stockproduction.VoucherNumber,
                VoucherDate = stockproduction.VoucherDate,
                WarehouseName = stockproduction.Warehouse?.Name,
                CreatedBy = stockproduction.CreatedBy,
                Details = stockproduction.Details,
                StockProducedItems = stockproduction.StockProducedItems.Select(sc => new StockProducedDetailsModel
                {
                    ItemName = sc.Item.Name,
                    Quantity = sc.Quantity,
                    UnitPrice = sc.UnitPrice
                }).ToList()
            };

            // Return the view as PDF using Rotativa's ViewAsPdf method
            return new ViewAsPdf("ExportToPdf", viewModel)
            {
                FileName = $"StockIssue_{viewModel.VoucherNumber}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }




        private async Task PopulateViewModelAsync(StockProductionCreateModel viewModel)
        {
            var items = await _itemManagementService.GetAllAsync();
            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            viewModel.Items = new SelectList(items, "Id", "Name");
            viewModel.Warehouses = new SelectList(warehouses, "Id", "Name");
        }

    }
}
