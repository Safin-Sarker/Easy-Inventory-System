using AutoMapper;
using DevSkill.Inventory.Application.Services.StockProduction;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using DevSkill.Inventory.Application.Services.StockTransferManagementService;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockTransferModels;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevSkill.Inventory.Web.Areas.Admin.Models.StockProduction;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class StockTransferController : Controller
    {

        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IStockManagementService _stockManagementService;
        private readonly IStockTransferManagementService _stockTransferManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockTransferController> _logger;

        public StockTransferController(IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IStockManagementService stockManagementService,
            IStockTransferManagementService stockTransferManagementService,
            IMapper mapper,
            ILogger<StockTransferController> logger)
        {
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _stockManagementService = stockManagementService;
            _stockTransferManagementService = stockTransferManagementService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "CanViewTransfer")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> StockTransferJsonData([FromBody] StockTransferListModel model)
        {
            var result = await _stockTransferManagementService.GetStockTransfersAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("VoucherNumber")
            );

            var itemJsonData = new  
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                        HttpUtility.HtmlEncode(record.VoucherNumber),
                        record.VoucherDate.ToString("yyyy-MM-dd"),
                         record.VoucherDate.ToString("yyyy-MM-dd"), // Assuming TransferDate is available
                        HttpUtility.HtmlEncode(record.SourceWarehouse?.Name ?? "N/A"),
                        HttpUtility.HtmlEncode(record?.Details),
                        HttpUtility.HtmlEncode(record.CreatedBy),
                        record.Id.ToString() 
                        }
                    ).ToArray()
            };

            return Json(itemJsonData);
        }

        [HttpGet]
        [Route("/Admin/StockTransfer/GetItemsByWarehouse")]
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

        [HttpGet]
        public async Task<JsonResult> GetAllWarehousesExcept(Guid excludeWarehouseId)
        {
            var warehouses = await _warehouseManagementService.GetAllWarehousesExceptAsync(excludeWarehouseId);
            return Json(warehouses.Select(w => new { id = w.Id, name = w.Name }));
        }



        [Authorize(Policy = "CanCreateTransfer")]
        public async Task<IActionResult> Create()
        {
            var items = await _itemManagementService.GetAllAsync();

            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            var voucherNumber = await _stockTransferManagementService.GetVoucherNumberAsync();

            var viewModel = new StockTransferCreateModel
            {
                VoucherNumber = voucherNumber,
                VoucherDate = DateTime.Now,
                Products = new SelectList(items, "Id", "Name"),
                SourceWarehouses = new SelectList(warehouses, "Id", "Name"),
                DestinationWarehouses = new SelectList(warehouses, "Id", "Name"),
                CreatedBy = User.Identity.Name
            };

            return View(viewModel);

        }

        private async Task PopulateViewModelAsync(StockTransferCreateModel viewModel)
        {
            var items = await _itemManagementService.GetAllAsync();

            var warehouses = await _warehouseManagementService.GetWarehouseListAsync();

            viewModel.Products = new SelectList(items, "Id", "Name");
            viewModel.SourceWarehouses = new SelectList(warehouses, "Id", "Name");
            viewModel.DestinationWarehouses = new SelectList(warehouses, "Id", "Name");
        }

        [HttpPost, ValidateAntiForgeryToken,Authorize(Policy = "CanCreateTransfer")]
        public async Task<IActionResult> Create(StockTransferCreateModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var stockTransfer = _mapper.Map<StockTransfer>(viewModel);

                try
                {
                    bool result = await _stockTransferManagementService.StockTransferAsync(stockTransfer);

                    if (result)
                    {
                        await _stockTransferManagementService.CreateStockTransferAsync(stockTransfer);

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Stock Transfer Created Successfully",
                            Type = ResponseType.success
                        });

                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex) 
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Stock Transfer Creation Failed due to an error: ",
                        Type = ResponseType.Danger
                    });

                    await PopulateViewModelAsync(viewModel);
                    return View(viewModel);
                }
            }
            TempData.Put("ResponseMessage", new ResponseModel
            {
                Message = "Stock Transfer Creation Failed",
                Type = ResponseType.Danger
            });

            await PopulateViewModelAsync(viewModel);
            return View(viewModel);
        }

        [Authorize(Policy = "CanGetTransferDetails")]
        public async Task<IActionResult> Details(Guid id)
        {
            var stockTransfer = await _stockTransferManagementService.GetStockTransferByIdAsync(id);

            if (stockTransfer == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<StockTransferDetailsModel>(stockTransfer);

            return View(model); 
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanDeleteTransfer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _stockTransferManagementService.DeleteItemAsync(id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Stock Transfer Deleted Successfully",
                    Type = ResponseType.success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Stock Transfer Delete Failed",
                    Type = ResponseType.Danger
                });

                _logger.LogError(ex, "Stock Transfer Delete Failed");
            }

            return View();
        }

        [HttpGet, Authorize(Policy = "CanGenerateTransferReport")]
        public async Task<IActionResult> ExportToPdf(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID");
            }

            var stockTransfer = await _stockTransferManagementService.GetByIdAsync(id);

            if (stockTransfer == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<StockTransferDetailsModel>(stockTransfer);

            return new ViewAsPdf("ExportToPdf", viewModel)
            {
                FileName = $"StockTransfer_{viewModel.VoucherNumber}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                CustomSwitches = "--disable-smart-shrinking"
            };
        }



    }


}
