using System;
using Logistic.ConsoleClient;
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
            var userInput = Console.ReadLine();
            var input = userInput.Split(' ');

                switch (input[0].ToLower())
                {
                    case "add":
                        ExecuteAdd(input);
                        exit = true;
                        break;

                    case "get-all":
                        ExecuteGet(input);
                        exit = true;
                        break;
                    case "load-cargo":
                        ExecuteLoad(input);
                        exit = true;
                        break;

                    case "unload-cargo":
                        ExecuteUnload(input);
                        exit = true;
                        break;

                    case "create-report":
                        ExecuteCreate(input);
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

            void ExecuteAdd(string [] input)
            {
          
                switch (input[1].ToLower())
                {
                    case "vehicle":

                        VehicleType vehicleType = VehicleType.Car;
                        
                            Console.WriteLine("Enter vehicle type: car, plane, ship, train");
                            var vehicleTypeInput = Console.ReadLine();

                            switch (vehicleTypeInput.ToLower())
                            {
                                case "car":
                                    vehicleType = VehicleType.Car;
                                    break;
                                case "train":
                                    vehicleType = VehicleType.Train;
                                    break;
                                case "plane":
                                    vehicleType = VehicleType.Plane;
                                    break;
                                case "ship":
                                    vehicleType = VehicleType.Ship;
                                    break;
                                default:
                                    Console.WriteLine("Not specified vehicle");
                                    break;
                            }

                        Console.WriteLine("Enter maximal cargo weight in Kg. For example - 500");
                        int maxCargoWeightKg = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter maximal cargo volume for vehicle.");
                        double maxCargoVolume = double.Parse(Console.ReadLine());

                        var vehicle = new Vehicle(vehicleType, maxCargoWeightKg, maxCargoVolume);
                        vehicleService.Create(vehicle);
                        break;

                    case "warehouse":
                        var warehouse = new Warehouse();
                        warehouseService.Create(warehouse);
                        break;
                    default:
                        Console.WriteLine("Not specified entity. Try again");
                        break;
                }

            }

            void ExecuteGet(string[] input)
            {
                
                    switch (input[1].ToLower())
                    {
                        case "vehicle":
                            var vehicles = vehicleService.GetAll();
                            foreach (var vehicle in vehicles)
                            {
                                Console.WriteLine(vehicle);
                            }
                            
                            break;

                        case "warehouse":
                            var warehouses = warehouseService.GetAll();

                            foreach (var warehouse in warehouses)
                            {
                                Console.WriteLine(warehouse);
                            }

                            break;

                        default:
                            Console.WriteLine("Incorrect entity. Please try again");
                            break;
                    }
                
            }

            void ExecuteLoad(string[] input)
            {
             
                    switch (input[1].ToLower())
                    {
                        case "vehicle":
                            try
                            {
                                int vehicleId = int.Parse(input[2]);
                                Console.WriteLine("Please enter cargo weight");
                                int cargoWeight = int.Parse(Console.ReadLine());
                                Console.WriteLine("Please, enter cargo volume");
                                double cargoVolume = double.Parse(Console.ReadLine());

                                Cargo cargo = new Cargo(cargoVolume, cargoWeight);
                                var vehicle = vehicleService.GetById(vehicleId);
                                vehicleService.LoadCargo(cargo, vehicleId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            break;
                        case "warehouse":
                            try
                            {
                                int warehouseId = int.Parse(input[2]);
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

                            break;

                        default:
                            Console.WriteLine("You can load cargo to vehicle or warehouse only. Try again");
                            break;
                    }
            }

            void ExecuteUnload(string[] input)
            {
                
                    switch (input[1].ToLower())
                    {
                        case "vehicle":
                            try
                            {
                                int vehicleId = int.Parse(input[2]);
                                Console.WriteLine("Enter cargo id");
                                Guid cargoId = Guid.Parse(Console.ReadLine());
                                vehicleService.UnloadCargo(cargoId, vehicleId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            break;

                        case "warehouse":
                            try
                            {
                                int warehouseId = int.Parse(input[2]);
                                Console.WriteLine("Enter cargo id");
                                Guid cargoId = Guid.Parse(Console.ReadLine());
                                warehouseService.UnloadCargo(cargoId, warehouseId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }

                            break;

                        default:
                            Console.WriteLine("You can unload cargo from vehicle or warehouse only. Try again");
                            break;
                    }

            }

            void ExecuteCreate(string[] input)
            {
    
                    switch (input[1].ToLower())
                    {
                        case "vehicle":

                            try
                            {

                                ReportType reportTypeVehicle = (ReportType)Enum.Parse(typeof(ReportType), input[2], ignoreCase: true);

                                List<Vehicle> vehicles = vehicleService.GetAll().ToList();
                                vehicleReportService.CreateReport(vehicles, reportTypeVehicle);
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid report type: {input[2]}. Please specify either 'json' or 'xml'.");
                            }

                        break;

                        case "warehouse":
                            try
                            {

                            ReportType reportTypeVehicle = (ReportType)Enum.Parse(typeof(ReportType), input[2], ignoreCase: true);

                            List<Vehicle> vehicles = vehicleService.GetAll().ToList();
                            vehicleReportService.CreateReport(vehicles, reportTypeVehicle);
                            }

                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid report type: {input[2]}. Please specify either 'json' or 'xml'.");
                            }
                        break;

                        default:
                            Console.WriteLine("Please, specify the correct entity to generate report for");
                            break;
                    }
               
            }

        }

    }
}