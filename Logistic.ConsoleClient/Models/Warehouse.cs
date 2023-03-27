using System;
namespace Logistic.ConsoleClient.Models
{
	public class Warehouse  : IEntity
	{

		public int Id { get; set; }
		public List<Cargo>? CargoList {get; set;}

		public Warehouse()
		{
			this.CargoList = new List<Cargo>();
		}

		public Warehouse(List<Cargo> cargos)
		{

			this.CargoList = cargos;

        }
        public override string ToString()
        {
            string cargoListStr = "";
            if (CargoList != null)
            {
                cargoListStr = string.Join(", ", CargoList);
            }
            return $"Id: {Id}, CargoList: [{cargoListStr}]";
        }

    }
}

