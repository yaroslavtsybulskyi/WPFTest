using Logistic.Core.Services;
using Logistic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Logistic.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService<Vehicle> _vehicleReportService;
        private readonly IReportService<Warehouse> _warehouseReportService;

        private readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger, IReportService<Vehicle> vehicleReportService, IReportService<Warehouse> warehouseReportService)
        {
            _vehicleReportService = vehicleReportService;
            _logger = logger;
            _warehouseReportService = warehouseReportService;
        }

        [HttpPost("create-vehicle-report")]
        public IActionResult CreateVehicleReport([FromBody] List<Vehicle> vehicles, [FromQuery] ReportType reportFormat)
        {
            try
            {
                _vehicleReportService.CreateReport(vehicles, reportFormat);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("load-vehicle-report")]
        public IActionResult LoadVehicleReport(string fileName, [FromQuery] ReportType reportFormat)
        {
            try
            {
                List<Vehicle> vehicleReport = _vehicleReportService.LoadReport(fileName);

                string reportContent = string.Empty;
                string contentType = string.Empty;

                switch (reportFormat)
                {
                    case ReportType.Json:
                        reportContent = JsonSerializer.Serialize(vehicleReport);
                        contentType = "application/json";
                        break;
                    case ReportType.Xml:
                        var xmlSerializer = new XmlSerializer(typeof(List<Vehicle>));
                        using (var stringWriter = new StringWriter())
                        {
                            xmlSerializer.Serialize(stringWriter, vehicleReport);
                            reportContent = stringWriter.ToString();
                        }
                        contentType = "application/xml";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported report format: {reportFormat}");
                }

                var contentDisposition = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                Response.Headers.Add("Content-Type", contentType);

                byte[] fileBytes = Encoding.UTF8.GetBytes(reportContent);

                return File(fileBytes, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-warehouse-report")]
        public IActionResult CreateWarehouseReport([FromBody] List<Warehouse> warehouses, [FromQuery] ReportType reportFormat)
        {
            try
            {
                _warehouseReportService.CreateReport(warehouses, reportFormat);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("load-warehouse-report")]
        public IActionResult LoadWarehouseReport(string fileName, [FromQuery] ReportType reportFormat)
        {
            try
            {

                List<Warehouse> warehouseReport = _warehouseReportService.LoadReport(fileName);

                string reportContent = string.Empty;
                string contentType = string.Empty;

                switch (reportFormat)
                {
                    case ReportType.Json:
                        reportContent = JsonSerializer.Serialize(warehouseReport);
                        contentType = "application/json";
                        break;
                    case ReportType.Xml:
                        var xmlSerializer = new XmlSerializer(typeof(List<Warehouse>));
                        using (var stringWriter = new StringWriter())
                        {
                            xmlSerializer.Serialize(stringWriter, warehouseReport);
                            reportContent = stringWriter.ToString();
                        }
                        contentType = "application/xml";
                        break;
                    default:
                        throw new ArgumentException($"Unsupported report format: {reportFormat}");
                }

                var contentDisposition = new ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                Response.Headers.Add("Content-Type", contentType);

                byte[] fileBytes = Encoding.UTF8.GetBytes(reportContent);

                return File(fileBytes, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

