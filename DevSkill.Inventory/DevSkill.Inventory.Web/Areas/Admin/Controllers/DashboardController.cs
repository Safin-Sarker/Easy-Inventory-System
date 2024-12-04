using AutoMapper;
using DevSkill.Inventory.Application.Services.StockProduction;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DashboardController : Controller
    {
        private readonly IItemManagementService _itemManagementService;
        private readonly IWarehouseManagementService _warehouseManagementService;
        private readonly IStockManagementService _stockManagementService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IItemManagementService itemManagementService,
            IWarehouseManagementService warehouseManagementService,
            IStockManagementService stockManagementService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            _itemManagementService = itemManagementService;
            _warehouseManagementService = warehouseManagementService;
            _stockManagementService = stockManagementService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }


       
        public async Task<IActionResult> Index()
        {

            var totalWarehouseCount = await _warehouseManagementService.GetTotalWarehouseCountAsync();
            var totalProductCount = await _itemManagementService.GetTotalProductasync();
            var totalServiceCount = await _itemManagementService.GetTotalServiceasync();

            var model = new DashbordModel
            {
                TotalWarehouses = totalWarehouseCount,
                TotalProducts = totalProductCount,
                TotalServices = totalServiceCount
            };
            return View(model);
        }



    }
}
