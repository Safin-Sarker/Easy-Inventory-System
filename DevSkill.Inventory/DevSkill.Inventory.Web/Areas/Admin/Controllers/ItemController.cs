using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]

    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IMapper _mapper;
        private readonly IStockManagementService _stockManagementService;

        public ItemController(ILogger<ItemController> logger,
            IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IMapper mapper,
            IStockManagementService stockManagementService)
        {
            _logger = logger;
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _mapper = mapper;
            _stockManagementService = stockManagementService;
        }

        [Authorize(Policy = "CanGetItem")]
        public async Task<IActionResult> Index()
        {
 
            var model = new ItemListModel();
            model.ItemType = await _itemManagementService.GetDistinctItemTypesAsync();
            model.Category = await _itemManagementService.GetDistinctCategoriesAsync();
            return View(model);
        }


        [Authorize(Policy = "CanCreateItem")]
        public async Task<IActionResult> Create()
        {
            var warehouseDtos = await _warehouseManagementService.GetWarehouseListAsync();

          
            var WarehouseDataModel = _mapper.Map<List<WarehouseDataModel>>(warehouseDtos);

            var model = new ItemCreateModel
            {
                Warehouses = WarehouseDataModel
            };

            return View(model);
        }

   

        [HttpPost]
        public async Task<JsonResult> ItemJsonData([FromBody] ItemListModel model)
        {

            var result = await _itemManagementService.GetItemsAsyncSp(model.PageIndex, model.PageSize, model.SearchItem,
                  model.FormatSortExpression("Name"));


            var ItemJsonData = new
            {

                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                record.ProductCode?.ToString(),
                                HttpUtility.HtmlEncode(record.ItemType),
                                record.OpeningStock?.ToString(),
                                HttpUtility.HtmlEncode( record?.UnitOfMeasure),
                                HttpUtility.HtmlEncode(record?.Category),
                                record.Image != null ? record.Image : "null",
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
            return Json(ItemJsonData);


        }

        [HttpGet]
        [Authorize(Policy = "CanUpdateItem")]
        public async Task<IActionResult> Update(Guid id)
        {
            var existingItem = await _itemManagementService.GetItemByIdAsync(id);

            if (existingItem == null)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Item not found",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }

            // Map the item to the update model
            var model = _mapper.Map<ItemUpdateModel>(existingItem);

            model.ItemTypeList = await _itemManagementService.GetDistinctItemTypesAsync();
            model.CategoryList = await _itemManagementService.GetDistinctCategoriesAsync();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Policy = "CanCreateItem")]
        public async Task<IActionResult> Create(ItemCreateModel model, string commit)
        {
            if (ModelState.IsValid)
            {
                var newItem = _mapper.Map<Item>(model);
                newItem.Id = Guid.NewGuid();

                try
                {
                    if (model.Image != null && model.Image.Length > 0)
                    {

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items");
                        Directory.CreateDirectory(uploadsFolder); 

                        var uniqueFileName = $"{newItem.Id}_{Path.GetFileName(model.Image.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(fileStream);
                        }

                        newItem.Image = $"/images/items/{uniqueFileName}";
                    }

                    await _itemManagementService.CreateItemAsync(newItem);

                    if (model.TrackInventory && model.Warehouses != null)
                    {
                        var stockEntries = model.Warehouses
                            .Where(w => w.IsSelected && w.Quantity.HasValue && w.Quantity.Value > 0)
                            .Select(w => new Stock
                            {
                                Id = Guid.NewGuid(),
                                ItemId = newItem.Id,
                                WarehouseId = w.Id,
                                Quantity = w.Quantity.Value,
                                UnitPrice = w.UnitPrice ?? 0
                            })
                            .ToList();

                        if (stockEntries.Any())
                        {
                            await _stockManagementService.CreateStockAsync(stockEntries);
                        }
                    }


                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product Created Successfully",
                        Type = ResponseType.success
                    });

                    if (commit == "SaveAndNew")
                    {
                        return RedirectToAction("Create");
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product Creation Failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Product Creation Failed");
                }
            }

            return RedirectToAction("Create");
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanUpdateItem")]
        public async Task<IActionResult> Update(ItemUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = await _itemManagementService.GetItemByIdAsync(model.Id);

                    if (existingItem == null)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Item not found",
                            Type = ResponseType.Danger
                        });
                        return RedirectToAction("Index");
                    }
                    _mapper.Map(model, existingItem);

                    if (model.NewImage != null && model.NewImage.Length > 0)
                    {
       
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/items");
                        Directory.CreateDirectory(uploadsFolder); 

                        
                        var uniqueFileName = $"{existingItem.Id}_{Path.GetFileName(model.NewImage.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                       
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.NewImage.CopyToAsync(fileStream);
                        }

                   
                        if (!string.IsNullOrEmpty(existingItem.Image))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingItem.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }


                        existingItem.Image = $"/images/items/{uniqueFileName}";
                    }

                    await _itemManagementService.UpdateItemAsync(existingItem);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Item updated successfully",
                        Type = ResponseType.success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Item update failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Item update failed");
                }
            }

            model.ItemTypeList = await _itemManagementService.GetDistinctItemTypesAsync();
            model.CategoryList = await _itemManagementService.GetDistinctCategoriesAsync();

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken,Authorize(Policy = "CanDeleteItem")]
        public async Task<IActionResult> Delete(Guid id)
        {
                try
                {
                    await _itemManagementService.DeleteItemAsync(id);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product Deleted Successfully",
                        Type = ResponseType.success
                    });
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product Delete Failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Product Delete Failed");
                }

            return RedirectToAction("Index");

        }

        [Authorize(Policy = "CanViewItemDetails")]
        public async Task<IActionResult> Details(Guid id)
        {
            // Fetch the item details from the service
            var item = await _itemManagementService.GetItemByIdAsync(id);

            if (item == null)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Item not found!",
                    Type = ResponseType.Danger
                });
                return RedirectToAction("Index");
            }

            var warehouses = await _warehouseManagementService.GetWarehouseDataByItemIdAsync(id);

            // Map the WarehouseDataDto to WarehouseDataModel
            var warehouseDataModels = warehouses.Select(dto => new WarehouseDataModel
            {
                Name = dto.Name,
                Quantity = dto.Quantity
            }).ToList();

            var model = new ItemDetailsModel
            {
                Id=item.Id,
                Name = item.Name,
                ProductCode = item.ProductCode,
                ProductCategory = item.Category,
                Description = item.Description,
                InventoryTracking = item.TrackInventory ?? false,
                UnitOfMeasure = item.UnitOfMeasure,
                Warehouses = warehouseDataModels 
            };

            return View(model);
        }



    }



}
