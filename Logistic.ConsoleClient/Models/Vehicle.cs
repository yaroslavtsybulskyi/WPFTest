using System;
using Logistic.ConsoleClient.Enums;
using Logistic.ConsoleClient.Models;
using Logistic.ConsoleClient.Repositories;

namespace Logistic.ConsoleClient
{
    
    public class Vehicle : IEntity
	{

		public const double ConvertionRateKgPnd = 2.2;

		public int Id { get; set; }
		public VehicleType Type { get; set; }
        public string Number { get; set; }
        public int MaxCargoWeightKg { get; set; }
        public double MaxCargoWeightPnd { get; set; }
        public double MaxCargoVolume { get; set; }
        public Cargo[] Cargos { get; set; }
		public int CargoWeightLeftKg { get; set; }
		public double CargoVolumeLeft { get; set; }

		
		public Vehicle(VehicleType type, int maxCargoWeightKg, double maxCargoVolume)
		{
			this.Type = type;
			this.Number = "AA11";
			this.MaxCargoWeightKg = maxCargoWeightKg;
			this.MaxCargoWeightPnd = ConvertionRateKgPnd * maxCargoWeightKg;
			this.MaxCargoVolume = maxCargoVolume;
			this.CargoVolumeLeft = this.MaxCargoVolume;
			this.CargoWeightLeftKg = this.MaxCargoWeightKg;
			this.Cargos = new Cargo[100];

		}

		public string GetCargoVolumeLeft()
		{
			double volumeUsed = 0d;
			foreach (Cargo i in Cargos)
			{
				volumeUsed += i.Volume;
			}

			double volumeLeft = this.MaxCargoVolume - volumeUsed;
			return $"Volume left: {volumeLeft}";
		}

		public string GetCargoWeightLeft(WeightUnit weightUnit)
		{
			if (weightUnit == WeightUnit.Kilograms)
			{
				int kgUsed = 0;
				foreach (Cargo i in Cargos)
				{
					kgUsed += i.Weight;
				}

				int weightLeftKg = this.MaxCargoWeightKg - kgUsed;
				return $"Weight Left: {weightLeftKg} kg";

			}
			else
			{
				int usedInKg = 0;
				double pndUsed = 0d;
				foreach (Cargo i in Cargos)
				{
					usedInKg += i.Weight;
				}

				pndUsed = ConvertionRateKgPnd * usedInKg;

				double weightLeftPnd = this.MaxCargoWeightPnd - pndUsed;
				return $"Weight Left: {weightLeftPnd} pnd";

            }

		}

		public string GetInformation()
		{
				double totalCargoVolume = 0;
				int totalCargoWeight = 0;
				int count = this.Cargos.Count(n => n != null);

				foreach (Cargo i in this.Cargos)
				{
				if (i != null)

				{
                    totalCargoVolume += i.Volume;
                    totalCargoWeight += i.Weight;
                }
					
				}

				if(totalCargoVolume > 0d && totalCargoWeight > 0)
				{

				return $"Vehicle Type: {this.Type}\nNumber: {this.Number}\nMaxCargoWeightInKg: {this.MaxCargoWeightKg} kg" +
                    $"\nMaxCargoWeightInPnd: {this.MaxCargoWeightPnd} pnd \nMaxCargoVolume: {this.MaxCargoVolume} m3 \nNumber of Cargo: {count} " +
                    $"\nTotal Cargo Volume: {totalCargoVolume} m3 \nTotal Cargo Weight: {totalCargoWeight} kg";
				}
				else
				{
                return $"Vehicle Type: {this.Type}\nNumber: {this.Number}\nMaxCargoWeightInKg: {this.MaxCargoWeightKg} kg " +
                    $"\nMaxCargoWeightInPnd: {this.MaxCargoWeightPnd} pnd \nMaxCargoVolume: {this.MaxCargoVolume} m3 \nNo cargo loaded";
				}
		}

		
        public override string ToString()
        {
            return $"Id: {Id}, Type: {Type}, Number: {Number}, MaxCargoWeight (kg): {MaxCargoWeightKg}, MaxCargoWeight (pnd): {MaxCargoWeightPnd}, MaxCargoVolume: {MaxCargoVolume}, CargoWeightLeft (kg): {CargoWeightLeftKg}, CargoVolumeLeft: {CargoVolumeLeft}";
        }


    }
}

