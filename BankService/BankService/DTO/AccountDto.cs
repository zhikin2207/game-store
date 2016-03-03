using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BankService.DTO
{
	[DataContract]
	public class AccountDto
	{
		[DataMember]
		public string Number { get; set; }

		[DataMember]
		public decimal Balance { get; set; }

		[DataMember]
		public ICollection<CardDto> Cards { get; set; }
	}
}