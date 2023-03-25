using System;
namespace Logistic.ConsoleClient.Models
{
	public class Invoice
	{
		public Guid Id { get; }
		public string RecipientAddress { get; set; }
		public string SenderAddress { get; set; }
		public string RecipientPhoneNumber { get; set; }
		public string SenderPhoneNumber { get; set; }

		public Invoice()
		{
			this.Id = Guid.NewGuid();
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

