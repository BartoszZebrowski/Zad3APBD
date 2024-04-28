using Microsoft.AspNetCore.Mvc;

namespace Zad3.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseController() 
        {

        }

        public IActionResult X(WarhouseDto request)
        {
            _warehouseService.X();
        }

    }
}
