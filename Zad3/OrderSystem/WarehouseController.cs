using Microsoft.AspNetCore.Mvc;
using Zad3.OrderSystem.Models.DTOs;

namespace Zad3.OrderSystem
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseService _warehouseService;

        public WarehouseController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost("/addRealization")]
        public ActionResult<int> AddRealization([FromBody]AddProductDto request)
        {
            var result = _warehouseService.AddProduct(request);
            return Ok(result);
        }
    }
}
