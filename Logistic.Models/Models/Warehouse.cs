using System.Xml.Serialization;

namespace Logistic.Models
{
    [XmlRoot("Warehouse", Namespace = "")]
    public class Warehouse : IEntity
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlArray("CargoList")]
        [XmlArrayItem("Cargo")]
        public List<Cargo>? CargoList { get; set; }

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