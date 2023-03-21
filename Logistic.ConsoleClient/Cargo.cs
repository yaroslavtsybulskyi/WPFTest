using System;
namespace Logistic.ConsoleClient
{
	public class Cargo
	{
		public double Volume { get; set; }
		public int Weight { get; set; }
		public string Code { get; set; }

		public Cargo()
		{
			this.Volume = 0d;
			this.Weight = 0;
			this.Code = "0000";
		}

		public Cargo(double volume, int weight, string code)
		{
			this.Volume = volume;
			this.Weight = weight;
			this.Code = code;
		}

		public string GetInformation()
		{
			string textToReturn = $"Volume of the cargo: {this.Volume} m3 \nWeight of the cargo: {this.Weight} kg \nCargo Code: {this.Code}";
			return textToReturn;
		}
	}
}

