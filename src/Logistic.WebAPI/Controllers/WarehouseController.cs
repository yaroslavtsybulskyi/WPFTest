using Logistic.Core.Services;
using Logistic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IService<Warehouse> _warehouseService;

        private readonly ILogger<WarehouseController> _logger;

        public WarehouseController(ILogger<WarehouseController> logger, IService<Warehouse> warehouseService)
        {
            _warehouseService = warehouseService;
            _logger = logger;
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id)
        {
            var warehouse = _warehouseService.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }

        [HttpGet("get-all")]
        public IEnumerable<Warehouse> GetAll()
        {
            return _warehouseService.GetAll();
        }

        [HttpPost]
        public IActionResult Create(Warehouse warehouse)
        {
            _warehouseService.Create(warehouse);
            return Ok(warehouse);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _warehouseService.Delete(id);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Warehouse warehouse)
        {
            _warehouseService.Update(warehouse);
            return Ok(warehouse);
        }

        [HttpPost("load-cargo")]
        public IActionResult LoadCargo([FromBody] Cargo cargo, int warehouseId)
        {
            _warehouseService.LoadCargo(cargo, warehouseId);
            return Ok();
        }

        [HttpPost("unload-cargo")]
        public IActionResult UnloadCargo(Guid cargoId, int warehouseId)
        {
            _warehouseService.UnloadCargo(cargoId, warehouseId);
            return Ok();
        }
    }
}
