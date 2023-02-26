using System;


namespace Logistic.ConsoleClient
{
    class Program
    {
        public static void Main(string[] args)
        {
           
            SuccessScenario();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("****************************************");

            ExceptionScenario();

            Console.ReadLine();

        }

        public static void SuccessScenario()
        {
            Vehicle car = new Vehicle(VehicleType.Car, 500, 600);
            Console.WriteLine(car.GetInformation());

            Cargo cargo1 = new Cargo(10, 5, "A1");
            Cargo cargo2 = new Cargo(15, 3, "B2");
            Cargo cargo3 = new Cargo(30, 22, "C3");
            Console.WriteLine("--------------------------------------");


            Cargo[] cargoToLoad = new Cargo[] { cargo1, cargo2, cargo3 };

            foreach (Cargo cargo in cargoToLoad)
            {
                car.LoadCargo(cargo);
            }

            Console.WriteLine(car.GetInformation());
        }

        public static void ExceptionScenario()
        {
            Vehicle ship = new Vehicle(VehicleType.Ship, 1000, 10000);
            Console.WriteLine(ship.GetInformation());

            Console.WriteLine("--------------------------------------");

            Cargo shipCargo1 = new Cargo(100, 500, "Ship-1");
            Cargo shipCargo2 = new Cargo(5000, 300, "Ship-2");
            Cargo shipCargo3 = new Cargo(700, 7000, "ship-3");
            Cargo shipCargo4 = new Cargo(5000, 200, "ship-4");

            Cargo[] shipCargoToLoad = new Cargo[] { shipCargo1, shipCargo2, shipCargo3, shipCargo4};

            foreach (Cargo cargo in shipCargoToLoad)
            {
                ship.LoadCargo(cargo);
            }

            Console.WriteLine("--------------------------------------");

            Console.WriteLine(ship.GetInformation());
        }
    }
}