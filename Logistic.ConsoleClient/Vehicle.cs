using System;

namespace Logistic.ConsoleClient
{
    
    public class Vehicle
	{

		public const double ConvertionRateKgPnd = 2.2;

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
			for (int i = 0; i < this.Cargos.Length; i++)
			{
				this.Cargos[i] = new Cargo();
			}

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
				int numberOfCargo = 0;
				double totalCargoVolume = 0;
				int totalCargoWeight = 0;

				foreach (Cargo i in Cargos)
				{
				if (i.Free == false)
				{
					numberOfCargo += 1;
				}
				
				}


				foreach (Cargo i in Cargos)
				{
					totalCargoVolume += i.Volume;
					totalCargoWeight += i.Weight;
					
				}

				if(totalCargoVolume > 0d && totalCargoWeight > 0)
				{

				return $"Vehicle Type: {this.Type}\nNumber: {this.Number}\nMaxCargoWeightInKg: {this.MaxCargoWeightKg} kg" +
                    $"\nMaxCargoWeightInPnd: {this.MaxCargoWeightPnd} pnd \nMaxCargoVolume: {this.MaxCargoVolume} m3 \nNumber of Cargo: {numberOfCargo} " +
                    $"\nTotal Cargo Volume: {totalCargoVolume} m3 \nTotal Cargo Weight: {totalCargoWeight} kg";
            }
			else
				{
                return $"Vehicle Type: {this.Type}\nNumber: {this.Number}\nMaxCargoWeightInKg: {this.MaxCargoWeightKg} kg " +
                    $"\nMaxCargoWeightInPnd: {this.MaxCargoWeightPnd} pnd \nMaxCargoVolume: {this.MaxCargoVolume} m3 \nNo cargo loaded";
            }
		}

		public void LoadCargo(Cargo cargo)
		{
			try
			{
				int cargoWeightAfterLoading = this.CargoWeightLeftKg - cargo.Weight;

				if (cargoWeightAfterLoading < 0)
				{
					throw new Overweight();
				}
				else
				{
                    this.CargoWeightLeftKg -= cargo.Weight;
                }

				
				double cargoVolumeAfterLoading = this.CargoVolumeLeft - cargo.Volume;

				if (cargoVolumeAfterLoading < 0)
				{
					throw new MoreVolumeNeeded();
				}
				else
				{
                    this.CargoVolumeLeft -= cargo.Volume;
                }

                for (int i = 0; i < this.Cargos.Length; i++)
                {
                    if (this.Cargos[i].Free == true)
                    {
                        this.Cargos[i] = cargo;
						this.Cargos[i].Free = false;
                        break;
                    }
                }
            }
			catch (MoreVolumeNeeded)
			{
				Console.WriteLine($"Cargo is {cargo.Volume} m3 and {cargo.Weight} kg. More volume needed");
			}
			catch (Overweight)
			{
				Console.WriteLine($"Cargo is {cargo.Volume} m3 and {cargo.Weight} kg. Vehicle is overweight");
			}
		}

	}
}

