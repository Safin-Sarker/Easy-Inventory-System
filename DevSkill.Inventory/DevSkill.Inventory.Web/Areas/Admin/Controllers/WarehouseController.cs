using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.Warehouse_Models;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class WarehouseController : Controller
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IMapper _mapper;

        public WarehouseController(ILogger<WarehouseController> logger, IWarehouseManagementService warehouseManagementService, IMapper mapper)
        {
            _logger = logger;
            _warehouseManagementService = warehouseManagementService;
            _mapper = mapper;
        }

        [Authorize(Policy = "CanViewWarehouses")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> WarehouseJsonData([FromBody] WarehouseListModel model)
        {

            var result = await _warehouseManagementService.GetWarehousesAsync(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Name", "Manager", "City"));

            var WarehouseJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Name),
                                HttpUtility.HtmlEncode(record.Manager),
                                HttpUtility.HtmlEncode(record.city),
                                HttpUtility.HtmlEncode(record.Phone),
                                record.Id.ToString()
                        }
               ).ToArray()
            };

            return Json(WarehouseJsonData);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanCreateWarehouse")]
        public async Task<IActionResult> CreateWarehouse(WarehouseCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var Warehouse = _mapper.Map<Warehouse>(model);

                Warehouse.Id = Guid.NewGuid();


                try
                {
                    await _warehouseManagementService.CreateWarehouseAsync(Warehouse);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse Created Successfully",
                        Type = ResponseType.success
                    });
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse Creation Failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Warehouse Creation Failed");
                }

            }
            return RedirectToAction("Index");


        }

        [Authorize(Policy = "CanUpdateWarehouse")]
        public async Task<IActionResult> Update(Guid id)
        {
            Warehouse warehouse = await _warehouseManagementService.GetWarehouseAsync(id);

            var model= _mapper.Map<WarehouseUpdateModel>(warehouse);

            return View (model);

        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanUpdateWarehouse")]
        public async Task<IActionResult> Update(WarehouseUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var Warehouse = _mapper.Map<Warehouse>(model);



                try
                {
                    await _warehouseManagementService.UpdateWarehouseAsync(Warehouse);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse Updated Successfully",
                        Type = ResponseType.success
                    });
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Warehouse Update Failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Warehouse Update Failed");
                }

            }
            return View();

        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanDeleteWarehouse")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _warehouseManagementService.DeleteWarehouse(id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Warehouse deleted successfuly",
                    Type = ResponseType.success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Warehouse delete failed",
                    Type = ResponseType.Danger
                });

                _logger.LogError(ex, "Warehouse delete failed");
            }
            return View();
        }

    }
}
