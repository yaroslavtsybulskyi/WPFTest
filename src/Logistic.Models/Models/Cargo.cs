using System.Xml.Serialization;

namespace Logistic.Models
{
    public class Cargo
    {
        [XmlElement("Volume")]
        public double Volume { get; set; }
        [XmlElement("Weight")]
        public int Weight { get; set; }
        [XmlElement("Code")]
        public string Code { get; set; }
        [XmlElement("Id")]
        public Guid Id { get; set; }

        [XmlElement("Invoice")]
        public Invoice Invoice { get; set; }

        public Cargo()
        {
            this.Volume = 0d;
            this.Weight = 0;
            this.Code = "0000";
            this.Id = Guid.NewGuid();
            this.Invoice = new Invoice();
        }

        public Cargo(double volume, int weight, string code)
        {
            this.Volume = volume;
            this.Weight = weight;
            this.Code = code;
            this.Id = Guid.NewGuid();
            this.Invoice = new Invoice();
        }

        public Cargo(double volume, int weight, string code, Invoice invoice)
        {
            this.Volume = volume;
            this.Weight = weight;
            this.Code = code;
            this.Id = Guid.NewGuid();
            this.Invoice = invoice;
        }

        public Cargo(double volume, int weight)
        {
            this.Volume = volume;
            this.Weight = weight;
            this.Code = "0000";
            this.Id = Guid.NewGuid();
            this.Invoice = new Invoice();
        }

        public string GetInformation()
        {
            string textToReturn = $"Volume of the cargo: {this.Volume} m3 \nWeight of the cargo: {this.Weight} kg \nCargo Code: {this.Code}";
            return textToReturn;
        }
    }
}