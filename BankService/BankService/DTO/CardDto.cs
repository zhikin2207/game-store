using System.Runtime.Serialization;

namespace BankService.DTO
{
	[DataContract]
	public class CardDto
	{
		[DataMember]
		public string CardNumber { get; set; }

		[DataMember]
		public string CVV2 { get; set; }

		[DataMember]
		public int ExpirationMonth { get; set; }

		[DataMember]
		public int ExpirationYear { get; set; }

		public AccountDto Account { get; set; }
	}
}