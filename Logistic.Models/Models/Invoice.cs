using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Logistic.Models
{
    public class Invoice
	{
		[XmlElement("Id")]
		public Guid Id { get; }
		[XmlElement("RecipientAddress")]
		public string RecipientAddress { get; set; }
		[XmlElement("SenderAddress")]
		public string SenderAddress { get; set; }
		[XmlElement("RecipientPhoneNumber")]
		public string RecipientPhoneNumber { get; set; }
		[XmlElement("SenderPhoneNumber")]
		public string SenderPhoneNumber { get; set; }

		public Invoice()
		{
			this.Id = Guid.NewGuid();
            this.RecipientAddress = "RecipientAddress";
            this.SenderAddress = "SenderAddress";
            this.RecipientPhoneNumber = "RecipientPhoneNumber";
            this.SenderPhoneNumber = "SenderPhoneNumber";
        }

		public Invoice(string recipientAddress, string senderAddress, string recipientPhoneNumber, string senderPhoneNumber)
		{
			this.Id = Guid.NewGuid();
			this.RecipientAddress = recipientAddress;
			this.SenderAddress = senderAddress;
			this.RecipientPhoneNumber = recipientPhoneNumber;
			this.SenderPhoneNumber = senderPhoneNumber;
		}
	}
}