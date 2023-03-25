using System;
using Logistic.ConsoleClient.Enums;
using Logistic.ConsoleClient.Models;
using Logistic.ConsoleClient.Repositories;
using Logistic.ConsoleClient.Services;

namespace Logistic.ConsoleClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            var infrastructureBuilder = new AppInfrastructureBuilder();
            var services = infrastructureBuilder.Build();

            var vehicleService = (VehicleService)services["VehicleService"];
            var warehouseService = (WarehouseService)services["WarehouseService"];
            var vehicleReportService = (ReportService<Vehicle>)services["VehicleReportService"];
            var warehouseReportService = (ReportService<Warehouse>)services["WarehouseReportService"];
            var vehicleRepository = (InMemoryRepository<Vehicle>)services["VehicleRepository"];
            var warehouseRepository = (InMemoryRepository<Warehouse>)services["WarehouseRepository"];

            bool exit = true;

            while(exit)
            {
            Console.WriteLine("List of commands: add, get-all, load-cargo, unload-cargo, create-report, load-report, exit");
            Console.WriteLine("Enter your command");
            var input = Console.ReadLine();

                switch (input.ToLower())
                {
                    case "add":

                        bool isEntityCorrect;

                        do
                        {
                            Console.WriteLine("You can add vehicle and warehouse. Choose your option");
                            var addInput = Console.ReadLine();

                            switch (addInput.ToLower())
                            {
                                case "vehicle":

                                    VehicleType vehicleType = VehicleType.Car;
                                    bool isValidVehicleType;

                                    do
                                    {
                                        Console.WriteLine("Enter vehicle type: car, plane, ship, train");
                                        var vehicleTypeInput = Console.ReadLine();

                                        switch (vehicleTypeInput.ToLower())
                                        {
                                            case "car":
                                                vehicleType = VehicleType.Car;
                                                isValidVehicleType = true;
                                                break;
                                            case "train":
                                                vehicleType = VehicleType.Train;
                                                isValidVehicleType = true;
                                                break;
                                            case "plane":
                                                vehicleType = VehicleType.Plane;
                                                isValidVehicleType = true;
                                                break;
                                            case "ship":
                                                vehicleType = VehicleType.Ship;
                                                isValidVehicleType = true;
                                                break;
                                            default:
                                                Console.WriteLine("Not specified vehicle");
                                                isValidVehicleType = false;
                                                break;
                                        }
                                    } while (!isValidVehicleType);


                                    Console.WriteLine("Enter maximal cargo weight in Kg. For example - 500");
                                    int maxCargoWeightKg = int.Parse(Console.ReadLine());

                                    Console.WriteLine("Enter maximal cargo volume for vehicle.");
                                    double maxCargoVolume = double.Parse(Console.ReadLine());

                                    var vehicle = new Vehicle(vehicleType, maxCargoWeightKg, maxCargoVolume);
                                    vehicleService.Create(vehicle);
                                    isEntityCorrect = true;

                                    break;

                                case "warehouse":
                                    var warehouse = new Warehouse();
                                    warehouseService.Create(warehouse);
                                    isEntityCorrect = true;
                                    break;
                                default:
                                    Console.WriteLine("Not specified entity. Try again");
                                    isEntityCorrect = false;
                                    break;
                            }

                        } while (!isEntityCorrect);
                        exit = true;
                        break;

                    case "get-all":

                        bool isGetAllCorrect;

                        do
                        {
                            Console.WriteLine("Please, enter the entity: vehicle or warehouse");
                            var getAllInput = Console.ReadLine();

                            switch (getAllInput.ToLower())
                            {
                                case "vehicle":
                                    var vehicles = vehicleService.GetAll();
                                    foreach (var vehicle in vehicles)
                                    {
                                        Console.WriteLine(vehicle);
                                    }
                                    isGetAllCorrect = true;
                                    break;
                                case "warehouse":
                                    var warehouses = warehouseService.GetAll();

                                    foreach (var warehouse in warehouses)
                                    {
                                        Console.WriteLine("warehouse");
                                    }

                                    isGetAllCorrect = true;
                                    break;

                                default:
                                    Console.WriteLine("Incorrect entity. Please try again");
                                    isGetAllCorrect = false;
                                    break;
                            }
                        } while (!isGetAllCorrect);
                        exit = true;
                        break;

                    case "load-cargo":
                        bool isCargoLoaded;
                        do
                        {

                            Console.WriteLine("Please, specify where you want to add cargo: vehicle or warehouse");
                            var loadCargoInput = Console.ReadLine();

                            switch (loadCargoInput.ToLower())
                            {
                                case "vehicle":
                                    try
                                    {
                                        Console.WriteLine("Please, enter vehicle id");
                                        int vehicleId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Please enter cargo weight");
                                        int cargoWeight = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Please, enter cargo volume");
                                        double cargoVolume = double.Parse(Console.ReadLine());

                                        Cargo cargo = new Cargo(cargoVolume, cargoWeight);
                                        var vehicle = vehicleService.GetById(vehicleId);
                                        vehicle.LoadCargo(cargo);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }

                                    isCargoLoaded = true;
                                    break;
                                case "warehouse":
                                    try
                                    {
                                        Console.WriteLine("Please, enter warehouse id");
                                        int warehouseId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Please enter cargo weight");
                                        int cargoWeight = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Please, enter cargo volume");
                                        double cargoVolume = double.Parse(Console.ReadLine());

                                        Cargo cargo = new Cargo(cargoVolume, cargoWeight);
                                        warehouseService.LoadCargo(cargo, warehouseId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }

                                    isCargoLoaded = true;
                                    break;

                                default:
                                    Console.WriteLine("You can load cargo to vehicle or warehouse only. Try again");
                                    isCargoLoaded = false;
                                    break;
                            }
                        } while (!isCargoLoaded);
                        exit = true;
                        break;

                    case "unload-cargo":
                        bool isCargoUnloaded;
                        do
                        {

                            Console.WriteLine("Specify what entity you want to unload: vehicle or warehouse");
                            var unloadCargoInput = Console.ReadLine();
                            switch (unloadCargoInput.ToLower())
                            {
                                case "vehicle":
                                    try
                                    {
                                        Console.WriteLine("Enter vehicle id");
                                        int vehicleId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter cargo id");
                                        Guid cargoId = Guid.Parse(Console.ReadLine());
                                        vehicleService.UnloadCargo(cargoId, vehicleId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }

                                    isCargoUnloaded = true;
                                    break;

                                case "warehouse":
                                    try
                                    {
                                        Console.WriteLine("Enter warehouse id");
                                        int warehouseId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter cargo id");
                                        Guid cargoId = Guid.Parse(Console.ReadLine());
                                        warehouseService.UnloadCargo(cargoId, warehouseId);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }

                                    isCargoUnloaded = true;
                                    break;

                                default:
                                    Console.WriteLine("You can unload cargo from vehicle or warehouse only. Try again");
                                    isCargoUnloaded = false;
                                    break;
                            }

                        } while (!isCargoUnloaded);
                        exit = true;
                        break;

                    case "create-report":
                        bool isReportGenerated;
                        do
                        {

                            Console.WriteLine("Select entity to generate report for");
                            var createReportInput = Console.ReadLine();
                            switch (createReportInput.ToLower())
                            {
                                case "vehicle":
                                    ReportType reportTypeVehicle = ReportType.Json;
                                    bool isReportTypeSpecifiedVehicle;
                                    do
                                    {
                                        Console.WriteLine("Please, specify the report: xml or json");
                                        var reportTypeInput = Console.ReadLine();

                                        switch (reportTypeInput.ToLower())
                                        {
                                            case "json":
                                                reportTypeVehicle = ReportType.Json;
                                                List<Vehicle> vehiclesJson = vehicleService.GetAll().ToList();
                                                vehicleReportService.CreateReport(vehiclesJson, ReportType.Json);
                                                isReportTypeSpecifiedVehicle = true;
                                                break;

                                            case "xml":
                                                reportTypeVehicle = ReportType.Xml;
                                                List<Vehicle> vehiclesXml = vehicleService.GetAll().ToList();
                                                vehicleReportService.CreateReport(vehiclesXml, ReportType.Xml);
                                                isReportTypeSpecifiedVehicle = true;
                                                break;

                                            default:
                                                Console.WriteLine("Please, specify the report type: json or xml");
                                                isReportTypeSpecifiedVehicle = false;
                                                break;
                                        }

                                    } while (!isReportTypeSpecifiedVehicle);

                                    isReportGenerated = true;

                                    break;

                                case "warehouse":
                                    ReportType reportTypeWarehouse = ReportType.Json;
                                    bool isReportTypeSpecifiedWarehouse;
                                    do
                                    {
                                        Console.WriteLine("Please, specify the report: xml or json");
                                        var reportTypeInput = Console.ReadLine();

                                        switch (reportTypeInput.ToLower())
                                        {
                                            case "json":
                                                reportTypeWarehouse = ReportType.Json;
                                                List<Warehouse> warehousesJson = warehouseService.GetAll().ToList();
                                                warehouseReportService.CreateReport(warehousesJson, ReportType.Json);
                                                isReportTypeSpecifiedWarehouse = true;
                                                break;
                                            case "xml":
                                                reportTypeWarehouse = ReportType.Xml;
                                                List<Warehouse> warehousesXml = warehouseService.GetAll().ToList();
                                                warehouseReportService.CreateReport(warehousesXml, ReportType.Xml);
                                                isReportTypeSpecifiedWarehouse = true;
                                                break;
                                            default:
                                                Console.WriteLine("Please, specify the report type: json or xml");
                                                isReportTypeSpecifiedWarehouse = false;
                                                break;
                                        }

                                    } while (!isReportTypeSpecifiedWarehouse);
                                    isReportGenerated = true;
                                    break;

                                default:
                                    Console.WriteLine("Please, specify the correct entity to generate report for");
                                    isReportGenerated = false;
                                    break;
                            }
                        } while (!isReportGenerated);
                        exit = true;
                        break;

                    case "load-report":
                        Console.WriteLine("Enter the report name");
                        var loadReportInput = Console.ReadLine();
                        vehicleReportService.LoadReport(loadReportInput);
                        exit = true;
                        break;

                    case "exit":
                        exit = false;
                        break;

                    default:
                        Console.WriteLine("Command not recognized.Try again");
                        exit = true;
                        break;
                } 

            }

        }

    }
}