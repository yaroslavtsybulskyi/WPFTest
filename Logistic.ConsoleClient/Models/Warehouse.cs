using System;
namespace Logistic.ConsoleClient.Models
{
	public class Warehouse
	{
		private static int lastId = 0;
		public int Id { get; set; }
		public List<Cargo>? CargoList {get; set;}

		public Warehouse()
		{
			lastId++;
			this.Id = lastId;
			this.CargoList = new List<Cargo>();
		}

		public Warehouse(List<Cargo> cargos)
		{
            lastId++;
            this.Id = lastId;
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

