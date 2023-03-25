using System.Collections.Generic;
using Logistic.ConsoleClient.Models;
using Logistic.ConsoleClient.Repositories;
using Logistic.ConsoleClient.Services;

namespace Logistic.ConsoleClient
{
    public class AppInfrastructureBuilder
    {
        public Dictionary<string, object> Build()
        {
            var services = new Dictionary<string, object>();

            var vehicleRepository = new InMemoryRepository<Vehicle>(vehicle => vehicle.Id);
            var vehicleService = new VehicleService(vehicleRepository);

            var warehouseRepository = new InMemoryRepository<Warehouse>(warehouse => warehouse.Id);
            var warehouseService = new WarehouseService(warehouseRepository);

            var jsonVehicleRepository = new JsonRepository<Vehicle>();
            var xmlVehicleRepository = new XmlRepository<Vehicle>();
            var jsonWarehouseRepository = new JsonRepository<Warehouse>();
            var xmlWarehouseRepository = new XmlRepository<Warehouse>();
            var vehicleReportService = new ReportService<Vehicle>(jsonVehicleRepository, xmlVehicleRepository);
            var warehouseReportService = new ReportService<Warehouse>(jsonWarehouseRepository, xmlWarehouseRepository);

            services.Add("VehicleService", vehicleService);
            services.Add("WarehouseService", warehouseService);
            services.Add("VehicleRepository", vehicleRepository);
            services.Add("WarehouseRepository", warehouseRepository);
            services.Add("JsonVehicleRepository", jsonVehicleRepository);
            services.Add("XmlVehicleRepository", xmlVehicleRepository);
            services.Add("JsonWarehouseRepository", jsonWarehouseRepository);
            services.Add("XmlWarehouseRepository", xmlWarehouseRepository);
            services.Add("VehicleReportService", vehicleReportService);
            services.Add("WarehouseReportService", warehouseReportService);


            return services;
        }
    }
}
