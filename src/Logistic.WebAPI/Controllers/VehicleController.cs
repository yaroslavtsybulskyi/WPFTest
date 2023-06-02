using Logistic.Core.Services;
using Logistic.DAL;
using Logistic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IService<Vehicle> _vehicleService;

        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ILogger<VehicleController> logger, IService<Vehicle> vehicleService)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        [HttpGet("get-all")]
        public IEnumerable<Vehicle> GetAll()
        {
            return _vehicleService.GetAll();
        }

        [HttpPost]
        public IActionResult Create(Vehicle vehicle) 
        { 
            _vehicleService.Create(vehicle);
            return Ok(vehicle);
        }

        [HttpGet("get-by-id")]
        public IActionResult GetById(int id) 
        {
            var vehicle = _vehicleService.GetById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            _vehicleService.Delete(id);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Vehicle vehicle) 
        {
            _vehicleService.Update(vehicle);
            return Ok(vehicle);
        }

        [HttpPost("load-cargo")]
        public IActionResult LoadCargo([FromBody] Cargo cargo, int vehicleId)
        {
            _vehicleService.LoadCargo(cargo, vehicleId);  
            return Ok();
        }

        [HttpPost("unload-cargo")]
        public IActionResult UnloadCargo([FromBody] Guid cargoId, int vehicleId)
        {
            _vehicleService.UnloadCargo(cargoId, vehicleId);
            return Ok();
        }
    }
}