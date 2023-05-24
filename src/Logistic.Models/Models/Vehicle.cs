using System.Text.Json.Serialization;

namespace Logistic.Models
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
        public List<Cargo> Cargos { get; set; }

        public Vehicle()
        {
            
        }
        public Vehicle(VehicleType type, int maxCargoWeightKg, double maxCargoVolume)
        {
            this.Type = type;
            this.Number = "AA11";
            this.MaxCargoWeightKg = maxCargoWeightKg;
            this.MaxCargoWeightPnd = ConvertionRateKgPnd * maxCargoWeightKg;
            this.MaxCargoVolume = maxCargoVolume;
            this.Cargos = new List<Cargo>();
        }

        public Vehicle(VehicleType type, string number, int maxCargoWeightKg, double maxCargoVolume) 
        {
            this.Type = type;
            this.Number = number;
            this.MaxCargoWeightKg = maxCargoWeightKg;
            this.MaxCargoWeightPnd = ConvertionRateKgPnd * maxCargoWeightKg;
            this.MaxCargoVolume = maxCargoVolume;
            this.Cargos = new List<Cargo>();
        }
        [JsonConstructor]
        public Vehicle(int id, VehicleType type, string number, int maxCargoWeightKg, double maxCargoWeightPnd, double maxCargoVolume, List<Cargo> cargos)
        {
            this.Id = id;
            this.Type = type;
            this.Number = number;
            this.MaxCargoWeightKg = maxCargoWeightKg;
            this.MaxCargoWeightPnd = maxCargoWeightPnd;
            this.MaxCargoVolume = maxCargoVolume;
            this.Cargos = cargos;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Type: {Type}, Number: {Number}, MaxCargoWeight (kg): {MaxCargoWeightKg}, MaxCargoWeight (pnd): {MaxCargoWeightPnd}, MaxCargoVolume: {MaxCargoVolume}";
        }
    }
}